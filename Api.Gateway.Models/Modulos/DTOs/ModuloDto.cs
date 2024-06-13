using Api.Gateway.Models.Permisos.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Modulos.DTOs
{
    public class ModuloDto
    {
        public int Id { get; set; }
        public System.Nullable<int> ServicioId { get; set; }
        public string Nombre { get; set; }
        public string URL { get; set; }
        public string Icono { get; set; }
        public List<SubmoduloDto> Submodulos { get; set; } = new List<SubmoduloDto>();
    }
}
