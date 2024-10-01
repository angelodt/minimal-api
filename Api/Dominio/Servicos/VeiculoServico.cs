using minimal_api.Dominio.Interfaces;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Infraestrutura.DB;

namespace minimal_api.Dominio.Servicos
{
    public class VeiculoServico : IVeiculoServico
    {
        private readonly DbContexto _contexto;
        public VeiculoServico(DbContexto contexto) 
        {
            _contexto = contexto;
        }        
        
        public void Apagar(Veiculo veiculo)
        {
            _contexto.Veiculos.Remove(veiculo);
            _contexto.SaveChanges();
        }

        public void Atualizar(Veiculo veiculo)
        {
            if(IVeiculoServico.ValidarDadosVeículo(veiculo).Count() == 0 ) {
            _contexto.Veiculos.Update(veiculo);
            _contexto.SaveChanges();
            } else throw new Exception("Não foi possível atualizar o veículo: Novos dados incompletos.");
        }

        public Veiculo? BuscarPorId(int id)
        {
            return _contexto.Veiculos.Find(id);
        }

        public void Incluir(Veiculo veiculo)
        {
            if(IVeiculoServico.ValidarDadosVeículo(veiculo).Count() == 0 ) {
                _contexto.Veiculos.Add(veiculo);
                _contexto.SaveChanges();
            } else throw new Exception("Não foi possível salvar o veículo: Dados incompletos.");
        }

        public List<Veiculo> Todos(int pagina = 1, string nome = null, string marca = null, int? ano = null)
        {
            var query =  _contexto.Veiculos.AsQueryable();
            if(!string.IsNullOrEmpty(nome))
            {
                query.Where(v => v.Nome.ToLower().Contains(nome.ToLower()));
            } else if(!string.IsNullOrEmpty(marca))
            {
                query.Where(v => v.Marca.ToLower().Contains(marca.ToLower()));
            } else if(ano > 1768)
            {
                query.Where(v => v.Ano.Equals(ano));
            }
            int ItensPorPagina = 10;
            return [.. query.Skip((pagina-1)*ItensPorPagina).Take(ItensPorPagina)];
        }
    }
}