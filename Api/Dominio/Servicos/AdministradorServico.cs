using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.DTOs;
using MinimalApi.Infraestrutura.DB;

namespace minimal_api.Dominio.Servicos
{
    public class AdministradorServico : IAdministradorServico
    {
        private readonly DbContexto _contexto;
        public AdministradorServico(DbContexto contexto) 
        {
            _contexto = contexto;
        }

        public Administrador BuscarPorId(int id)
        {
            return _contexto.Administradores.FirstOrDefault(x => x.Id == id);
        }

        public Administrador Incluir(Administrador administrador)
        {
            _contexto.Administradores.Add(administrador);
            _contexto.SaveChanges();
            return administrador;
        }

        public Administrador Login(LoginDTO loginDTO)
        {
            var adm = _contexto.Administradores.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
            return adm;
        }

        public List<Administrador> Todos(int pagina =1)
        {
             var query =  _contexto.Administradores.AsQueryable();
            
            int ItensPorPagina = 10;
            return [.. query.Skip((pagina-1)*ItensPorPagina).Take(ItensPorPagina)];
        }
    }
}