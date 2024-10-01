using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.DTOs;


namespace Test.Mocks
{
    public class AdministradorServicoMock : IAdministradorServico
    {
        private static List<Administrador> administradores = new List<Administrador>()
        {
            new Administrador() { 
                Id     = 1,
                Email  = "adm@teste.com",
                Perfil = "Administrador",
                Senha  = "123456"
            },
            new Administrador() { 
                Id     = 2,
                Email  = "editor@teste.com",
                Perfil = "Editor",
                Senha  = "123456"
            }
        };
        public Administrador BuscarPorId(int id)
        {
            return administradores.Find(a => a.Id == id);
        }

        public Administrador Incluir(Administrador administrador)
        {
            administradores.Add(administrador);
            return administrador;
        }

        public Administrador Login(LoginDTO loginDTO)
        {
            return administradores.Find(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha);
        }

        public List<Administrador> Todos(int pagina)
        {
            return administradores;
        }
    }
}