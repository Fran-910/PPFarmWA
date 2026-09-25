using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PPFarmWA.Shared.DTO
{
    public class RegistroDTO
    {
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]

        public string password;

        [Required(ErrorMessage = "El usuario o el email son obligatorios.")]
        public string userName  { get; set;}
        public string email { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set;}
    }
}
