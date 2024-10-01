using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Required]
        [StringLength(50)]
        public string Senha { get; set;} = default;

        [Required]
        [StringLength(20)]
        public string Perfil { get; set;} = default;
        
    }
}