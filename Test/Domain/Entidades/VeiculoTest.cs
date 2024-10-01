using System.Diagnostics;
using MinimalApi.Dominio.Entidades;

namespace Test.Domain.Entidades
{
    [TestClass]
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class VeiculoTest
    {

        private int IdTeste = 1;
        private string NomeVeiculoTeste = "Veiculo Teste";
        private string MarcaVeiculoTeste = "Marca Teste";
        private int AnoVeiculoTeste = 2024;

        [TestMethod]
        public void TestGetSetProperties()
        {
            //Arrange
            var v = new Veiculo
            {
                //Act
                Id = IdTeste,
                Nome = NomeVeiculoTeste,
                Marca = MarcaVeiculoTeste,
                Ano = AnoVeiculoTeste
            };

            //Assert
            Assert.AreEqual(IdTeste, v.Id);
            Assert.AreEqual(NomeVeiculoTeste, v.Nome);
            Assert.AreEqual(MarcaVeiculoTeste, v.Marca);
            Assert.AreEqual(AnoVeiculoTeste, v.Ano);
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}