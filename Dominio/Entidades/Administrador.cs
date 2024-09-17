using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MinimalApi.Dominio.Entidades
{
    public class Administrador
    {
        [Key]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public int Id { get; set;} = default;

        [Required]
        [StringLength(250)]
        public string Email { get; set;} = default;

        [StringLength(50)]
        public string Senha { get; set;} = default;

        [StringLength(20)]
        public string Perfil { get; set;} = default;
        
    }
}