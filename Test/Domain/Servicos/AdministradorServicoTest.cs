
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Infraestrutura.DB;

namespace Test.Domain.Servicos
{
    [TestClass]
    public class AdministradorServicoTest
    {
        private int IdTeste = 1;
        private string EmailTeste = "teste@teste.com";
        private string PasswordTeste = "t3st3";
        private string PerfilTeste = "PerfilAdmTeste";

        private DbContexto CriarContextoTeste()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.GetFullPath(Path.Combine(assemblyPath?? "", "..", "..", ".."));

            var configuration = new ConfigurationBuilder()
           .SetBasePath(path ?? Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddEnvironmentVariables().Build();

           return new DbContexto(configuration);
        }

        [TestMethod]
        public void TesteSalvarAdministrador()
        {
            //Arrange
            var context = CriarContextoTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores;");

            var admInsert = new Administrador();

            //Act
            admInsert.Id = IdTeste;
            admInsert.Email = EmailTeste;
            admInsert.Senha = PasswordTeste;
            admInsert.Perfil = PerfilTeste;

            
            var admService = new AdministradorServico(context);

            admService.Incluir(admInsert);

            //Assert
            Assert.AreEqual(1, admService.Todos(1).Count);                       
        }

        [TestMethod]
        public void TesteBuscarAdministradorPorId() {
            //Arrange
            var context = CriarContextoTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores;");

            var admInsert = new Administrador();

            //Act
            admInsert.Id = IdTeste;
            admInsert.Email = EmailTeste;
            admInsert.Senha = PasswordTeste;
            admInsert.Perfil = PerfilTeste;

            
            var admService = new AdministradorServico(context);
            admService.Incluir(admInsert);
            var admBD = admService.BuscarPorId(IdTeste);

            //Assert
            Assert.AreEqual(IdTeste, admBD?.Id);

        }
        
    }
}