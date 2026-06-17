using System.ComponentModel.DataAnnotations;

namespace SistemaStock.Models
{
    public class RegistroViewModel
    {
        /// Nombre de usuario

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [Display(Name = "Nombre de Usuario")]
        public string? NombreUsuario { get; set; }

        /// Contraseña

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        /// Confirmar contraseña

        [Required(ErrorMessage = "Debe confirmar la contraseña")]
        [Display(Name = "Confirmar Contraseña")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string? ConfirmarPassword { get; set; }

        /// Nombre

        [Required(ErrorMessage = "Debe ingresar un nombre")]
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

        /// RolId por defecto (2 = Usuario)

        public int RolId { get; set; } = 2;
    }
}
