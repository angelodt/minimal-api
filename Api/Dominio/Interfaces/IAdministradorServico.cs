using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Enuns;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.DTOs;

namespace MinimalApi.Dominio.Interfaces
{
    public interface IAdministradorServico
    {
        public Administrador Login(LoginDTO loginDTO);

        public Administrador? BuscarPorId(int id);

        public static Administrador AdministradorConverter(AdministradorDTO administradorDTO)
        {
                return new Administrador {
                    Email = administradorDTO.Email,
                    Senha = administradorDTO.Senha,
                    Perfil= administradorDTO.Perfil.ToString()
                };
        }

        public static AdministradorModelView AdministradorConverter(Administrador administrador)
        {
                return new AdministradorModelView {
                    Id = administrador.Id,
                    Email = administrador.Email,
                    Perfil= administrador.Perfil
                };
        }

        public static List<string> ValidarAdminstrador(AdministradorDTO administradorDTO) {
            List<string> erros = new List<string>();
            if (administradorDTO == null)
            {
                erros = ["Dados do Administrador não encontrado."];
            } else {

                erros = ValidarAdminstrador(AdministradorConverter(administradorDTO));
            }
            return erros;
        }
        public static List<string> ValidarAdminstrador(Administrador administrador){
            List<string> erros= new List<string>();
            if (administrador == null)
            {
                erros = ["Dados do Administrador não encontrado."];
            } else {
            
            if(string.IsNullOrEmpty(administrador.Email)) {
                    erros.Add("Email do Administrador é obrigatório. Não pode ser vazio.");
                }
                if(string.IsNullOrEmpty(administrador.Senha))
                {
                    erros.Add("Senha do Administrador é obrigatório. Não pode ser vazia.");
                }
                if(string.IsNullOrEmpty(administrador.Perfil.ToString()))
                {
                    erros.Add("Perfil do Administrador é obrigatório. Não pode ser vazio.");
                } else if(string.IsNullOrEmpty(EnumExtensions.ParseEnum<PerfilEnum>(administrador.Perfil).ToString()))
                {
                    erros.Add("Perfil do Administrador inválido.");
                }
            }
            return erros;
        }

        public Administrador Incluir(Administrador administrador);

        public List<Administrador> Todos(int pagina);
    
    }
}