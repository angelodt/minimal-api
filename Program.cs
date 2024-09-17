using Microsoft.EntityFrameworkCore;
using MinimalApi.DTOs;
using MinimalApi.Infraestrutura.DB;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DbContexto>(options => {
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
});

var app = builder.Build();



app.MapGet("/", () => "Olá pessoal!");

app.MapPost("/login",(LoginDTO loginDTO) => {
    if("adm@teste.com".Equals(loginDTO.Email) && "123456".Equals(loginDTO.Senha)) {
        return Results.Ok("Login realizado com sucesso.");
    } else return Results.Unauthorized();
});

app.Run();


