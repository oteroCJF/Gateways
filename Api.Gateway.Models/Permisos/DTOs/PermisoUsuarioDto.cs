using Api.Gateway.Models.Modulos.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Permisos.DTOs
{
    public class PermisoUsuarioDto
    {
        public string UsuarioId { get; set; }
        public int ModuloId { get; set; }
        public int SubmoduloId { get; set; }
        public int PermisoId { get; set; }

        public ModuloDto Modulo { get; set; } = new ModuloDto();
        public SubmoduloDto Submodulo { get; set; } = new SubmoduloDto();
        public PermisoDto Permiso { get; set; } = new PermisoDto();
    }
}
