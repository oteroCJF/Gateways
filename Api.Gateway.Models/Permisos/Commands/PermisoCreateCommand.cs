using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Permisos.Commands
{
    public class PermisoCreateCommand
    {
        public string UsuarioId { get; set; }
        public int ModuloId { get; set; }
        public int SubmoduloId { get; set; }
        public int PermisoId { get; set; }
    }
}
