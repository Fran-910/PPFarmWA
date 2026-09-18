using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PPFarmWA.Shared.DTO
{
    public class RegistroDTO
    {
        [Required(ErrorMessage = "El usuario o el email sonobligatorios.")]
        public string emailOusuario  { get; set;}

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string Password { get; set;}

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set;}
    }
}
