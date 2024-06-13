using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Permisos.DTOs
{
    public class PermisoSubmoduloDto
    {
        public int SubmoduloId { get; set; }
        public int PermisoId { get; set; }
        public PermisoDto Permiso { get; set; } = new PermisoDto();
    }
}
