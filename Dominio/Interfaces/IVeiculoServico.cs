using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimal_api.Dominio.DTOs;
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
        public static List<string> ValidarDadosVeículo(VeiculoDTO veiculoDTO)
        {
            List<string> erros = new List<string>();
            if (veiculoDTO == null)
            {
                erros = ["Dados do veículo não encontrado."];
            } else {
                if(string.IsNullOrEmpty(veiculoDTO.Nome)) {
                    erros.Add("Nome do veículo obrigatório.");
                }
                if(string.IsNullOrEmpty(veiculoDTO.Marca))
                {
                    erros.Add("Marca do veículo obrigatório.");
                }
                if(veiculoDTO.Ano < 1769)
                {
                    erros.Add("Ano do veículo inválido. Permitido apenas ANO maior que 1768");
                }
            }
            return erros;
        }
        public static List<string> ValidarDadosVeículo(Veiculo veiculo)                
        {
            List<string> erros;
            if (veiculo == null)
            {
                erros = ["Dados do veículo não encontrado."];
            }
            else
            {
                var veiculoDTO = new VeiculoDTO
                {
                    Nome = veiculo.Nome,
                    Marca = veiculo.Marca,
                    Ano = veiculo.Ano
                };
                erros = ValidarDadosVeículo(veiculoDTO);                
            }
            return erros;
        }
    }
}