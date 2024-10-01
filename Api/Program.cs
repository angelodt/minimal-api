using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Interfaces;
using MinimalApi.Dominio.ModelViews;
using minimal_api.Dominio.Servicos;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.DTOs;
using MinimalApi.Infraestrutura.DB;
using MinimalApi.Dominio.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MinimalApi.Dominio.Entidades;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;

#region Builder
var builder = WebApplication.CreateBuilder(args);

var jwtKey = builder.Configuration.GetSection("Jwt")["Key"].ToString();
if(string.IsNullOrEmpty(jwtKey)) jwtKey = "123456";

builder.Services.AddAuthentication(option => {
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option => {
    option.TokenValidationParameters = new TokenValidationParameters {
        ValidateLifetime = true,
        ValidateIssuer = false,
            ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))

    };
});
builder.Services.AddAuthorization();

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();
builder.Services.AddScoped<IVeiculoServico, VeiculoServico>();

builder.Services.AddDbContext<DbContexto>(options => {
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql"))
    );
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Name = "Autorization",
        Type = SecuritySchemeType.Http,
        Scheme= "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT aqui {token}"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement 
    {
        {
            new OpenApiSecurityScheme 
            {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");

//Realizar Login
app.MapPost("/login",([FromBody]LoginDTO loginDTO, IAdministradorServico administradorServico) => {
    var administrador = administradorServico.Login(loginDTO);
    if(administrador!=null) {

        string token = GerarTokenJwt(administrador);
        return Results.Ok(new AdministradorLogado {
            Email = administrador.Email,
            Perfil= administrador.Perfil,
            Token = token
        });
    } else 
        return Results.Unauthorized();
}).AllowAnonymous().WithTags("Home");

#endregion

#region Administrador

string GerarTokenJwt(Administrador administrador) {
    if(string.IsNullOrEmpty(jwtKey)) {
        return string.Empty;
    }
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    var credentials = new SigningCredentials(securityKey , SecurityAlgorithms.HmacSha256);    
    var claims = new List<Claim>(){
        new Claim("Email", administrador.Email),
        new Claim("Perfil", administrador.Perfil),
        new Claim(ClaimTypes.Role, administrador.Perfil)
    };

    var token= new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credentials
    );
    
    return new JwtSecurityTokenHandler().WriteToken(token).ToString();
}
//Cadastrar Administrador
app.MapPost("administrador/",([FromBody]AdministradorDTO administradorDTO, IAdministradorServico administradorServico) => {
    
    var validarAdminstrador = new ErrosValidacao();
    IAdministradorServico.ValidarAdminstrador(administradorDTO).ForEach(validarAdminstrador.MensagensErrosList.Add);
        
    if(validarAdminstrador.MensagensErrosList.Count > 0)
    {
        return Results.BadRequest(validarAdminstrador);
    } else {
        var adm = administradorServico.Incluir(IAdministradorServico.AdministradorConverter(administradorDTO));
        
        AdministradorModelView admView = IAdministradorServico.AdministradorConverter(adm);

        return Results.Created($"/administrador/{adm.Id}", admView);
        
    }    
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador"})
.WithTags("Administradores");

app.MapGet("/administrador/{id}",([FromRoute]int? id, IAdministradorServico administradorServico) => {
    var adm = administradorServico.BuscarPorId((int)id);
    
    if(adm == null) {
        return Results.NotFound();
    }  else {
        AdministradorModelView admView = IAdministradorServico.AdministradorConverter(adm);
        return Results.Ok(admView);
    } 

}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador"})
.WithTags("Administradores");

app.MapGet("/administradores",([FromQuery]int? pagina, IAdministradorServico administradorServico) => {
    if(pagina == null) {
        pagina = 1;
    }
    var administradores = administradorServico.Todos((int)pagina);
    if(administradores.Count > 0) {

        List<AdministradorModelView> admView = new List<AdministradorModelView>();
        administradores.ForEach(a => admView.Add(IAdministradorServico.AdministradorConverter(a)));

        return Results.Ok(admView);
    } else 
    {
        return Results.NotFound();
    }
    
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador"})
.WithTags("Administradores");
#endregion

#region Veiculo

ErrosValidacao ValidarVeiculoDTO(VeiculoDTO veiculoDTO) {

    var validacaoVeiculo = new ErrosValidacao();
    IVeiculoServico.ValidarDadosVeículo(veiculoDTO).ForEach(validacaoVeiculo.MensagensErrosList.Add);
    return validacaoVeiculo;     
}

app.MapPost("/veiculo", ([FromBody] VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) => {
    
    var validacaoVeiculo = ValidarVeiculoDTO(veiculoDTO);

    if(validacaoVeiculo.MensagensErrosList.Count>0) {
        return Results.BadRequest(validacaoVeiculo);
    } else {
        var veiculo = new Veiculo {
            Nome = veiculoDTO.Nome,
            Marca = veiculoDTO.Marca,
            Ano = veiculoDTO.Ano
        };
        veiculoServico.Incluir(veiculo);
        return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
    }    
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador, Editor"})
.WithTags("Veiculos");

app.MapGet("/veiculo/{id}",([FromRoute]int? id, IVeiculoServico veiculoServico) => {
    var veiculo = veiculoServico.BuscarPorId((int)id);
    
    if(veiculo == null) {
        return Results.NotFound();
    }  else return Results.Ok(veiculo);

})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador, Editor"})
.WithTags("Veiculos");

app.MapPut("/veiculo/{id}",([FromRoute]int? id, [FromBody]VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) => {
    
    var validacaoVeiculo = ValidarVeiculoDTO(veiculoDTO);

    if(validacaoVeiculo.MensagensErrosList.Count > 0) {
        return Results.BadRequest(validacaoVeiculo);
    } else {
        var veiculoBD = veiculoServico.BuscarPorId((int)id);
        
        if(veiculoBD == null) {
            return Results.NotFound();
        }  else {
            veiculoBD.Ano = veiculoDTO.Ano;
            veiculoBD.Marca = veiculoDTO.Marca;
            veiculoBD.Nome = veiculoDTO.Nome;
            veiculoServico.Atualizar(veiculoBD);
            return Results.Ok(veiculoDTO);
        }
    }     

})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador"})
.WithTags("Veiculos");

app.MapDelete("/veiculo/{id}",([FromRoute]int? id, IVeiculoServico veiculoServico) => {
    var veiculoBD = veiculoServico.BuscarPorId((int)id);
    
    if(veiculoBD == null) {
        return Results.NotFound();
    }  else {
        veiculoServico.Apagar(veiculoBD);
        return Results.NoContent();
    }
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador"})
.WithTags("Veiculos");

app.MapGet("/veiculos",([FromQuery]int? pagina, IVeiculoServico veiculoServico) => {
    if(pagina == null) {
        pagina = 1;
    }
    var veiculos = veiculoServico.Todos((int)pagina);
    return Results.Ok(veiculos);
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador, Editor"})
.WithTags("Veiculos");
#endregion

#region App
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.Run();

#endregion


