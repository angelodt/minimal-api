using MinimalApi.Dominio.Entidades;

namespace Test.Domain.Entidades
{
    [TestClass]
    public class AdministradorTest
    {

        private int IdTeste = 1;
        private string EmailTeste = "teste@teste.com";
        private string PasswordTeste = "t3st3";
        private string PerfilTeste = "PerfilAdmTeste";

        
        [TestMethod]
        public void TestGetSetProperties()
        {
            //Arrange
            var adm = new Administrador();

            //Act
            adm.Id = IdTeste;
            adm.Email = EmailTeste;
            adm.Senha = PasswordTeste;
            adm.Perfil = PerfilTeste;

            //Assert
            Assert.AreEqual(IdTeste, adm.Id);
            Assert.AreEqual(EmailTeste, adm.Email);
            Assert.AreEqual(PasswordTeste, adm.Senha);
            Assert.AreEqual(PerfilTeste, adm.Perfil);            
        }
    }
}