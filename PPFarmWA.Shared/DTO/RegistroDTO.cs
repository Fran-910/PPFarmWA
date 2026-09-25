using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PPFarmWA.Shared.DTO
{
    public class RegistroDTO
    {
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public string userName { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        public string email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string password { get; set; }
    }
}
