using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Permisos.DTOs
{
    public class PermisoDto
    {
        public int Id { get; set; }
        public string Abreviacion { get; set; }
        public string Nombre { get; set; }
    }
}
