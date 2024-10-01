using System.Text;
using System.Text.Json;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.DTOs;
using Test.Helpers;

namespace Test.Requests
{
    [TestClass]
    public class AdministradorRequestTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            Helpers.Setup.ClassInit(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Setup.ClassCleanup();
        }

            //Arrange
            //Act
            //Assert
        [TestMethod]
        public async Task AdministradorLoginTest()
        {
            //Arrange
            var loginDTO = new LoginDTO {
                Email = "adm@teste.com",
                Senha = "123456"
            };
            var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "application/json");
            //Act
            var response = await Setup.client.PostAsync("/login", content);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            //Assert
            Assert.IsNotNull(admLogado);
            Assert.IsNotNull(admLogado.Token);
            Assert.AreEqual("adm@teste.com", admLogado.Email);
            Assert.AreEqual("Administrador", admLogado.Perfil);
        }
    }
}