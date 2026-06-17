using System.ComponentModel.DataAnnotations;

namespace SistemaStock.Models
{
    public  class Rol
    {
        [Key]
        public int RolId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Nombre { get; set; }
    }
}
