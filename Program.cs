using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;
using MinimalApi.Dominio.ModelViews;
using minimal_api.Dominio.Servicos;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.DTOs;
using MinimalApi.Infraestrutura.DB;
using MinimalApi.Dominio.DTOs;

#region Builder
var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSwaggerGen();
#endregion

#region App
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
#endregion

#region Administrador

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
}).WithTags("Administradores");

app.MapPost("administrador/login",([FromBody]LoginDTO loginDTO, IAdministradorServico administradorServico) => {
    if(administradorServico.Login(loginDTO)!=null) {
        return Results.Ok("Login realizado com sucesso.");
    } else return Results.Unauthorized();
}).WithTags("Administradores");

app.MapGet("/administrador/{id}",([FromRoute]int? id, IAdministradorServico administradorServico) => {
    var adm = administradorServico.BuscarPorId((int)id);
    
    if(adm == null) {
        return Results.NotFound();
    }  else {
        AdministradorModelView admView = IAdministradorServico.AdministradorConverter(adm);
        return Results.Ok(admView);
    } 

}).WithTags("Administradores");

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
    
}).WithTags("Administradores");
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
}).WithTags("Veiculos");

app.MapGet("/veiculo/{id}",([FromRoute]int? id, IVeiculoServico veiculoServico) => {
    var veiculo = veiculoServico.BuscarPorId((int)id);
    
    if(veiculo == null) {
        return Results.NotFound();
    }  else return Results.Ok(veiculo);

}).WithTags("Veiculos");

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

}).WithTags("Veiculos");

app.MapDelete("/veiculo/{id}",([FromRoute]int? id, IVeiculoServico veiculoServico) => {
    var veiculoBD = veiculoServico.BuscarPorId((int)id);
    
    if(veiculoBD == null) {
        return Results.NotFound();
    }  else {
        veiculoServico.Apagar(veiculoBD);
        return Results.NoContent();
    }
}).WithTags("Veiculos");

app.MapGet("/veiculos",([FromQuery]int? pagina, IVeiculoServico veiculoServico) => {
    if(pagina == null) {
        pagina = 1;
    }
    var veiculos = veiculoServico.Todos((int)pagina);
    return Results.Ok(veiculos);
}).WithTags("Veiculos");
#endregion

app.Run();


