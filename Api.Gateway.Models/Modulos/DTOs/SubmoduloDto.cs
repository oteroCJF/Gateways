using Api.Gateway.Models.Permisos.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Modulos.DTOs
{
    public class SubmoduloDto
    {
        public int Id { get; set; }
        public int ModuloId { get; set; }
        public string Nombre { get; set; }
        public string URL { get; set; }
        public string Icono { get; set; }
        public List<PermisoSubmoduloDto> Permisos {get;set;}
    }
}
