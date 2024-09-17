using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimal_api.Dominio.Entidades;

namespace minimal_api.Dominio.Interfaces
{
    public interface IVeiculoServico
    {
        public List<Veiculo> Todos(int pagina = 1, string? nome =  null, string? marca = null, int? ano = null);

        public Veiculo? BuscarPorId(int id);

        public void Incluir(Veiculo veiculo);

        public void Atualizar(Veiculo veiculo);

        public void Apagar(Veiculo veiculo);
        
    }
}