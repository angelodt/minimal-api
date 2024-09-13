using MinimalApi.DTOs;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Olá pessoal!");

app.MapPost("/login",(LoginDTO loginDTO) => {
    if("adm@teste.com".Equals(loginDTO.Email) && "123456".Equals(loginDTO.Senha)) {
        return Results.Ok("Login realizado com sucesso.");
    } else return Results.Unauthorized();
});

app.Run();


