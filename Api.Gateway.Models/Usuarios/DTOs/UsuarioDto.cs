using Api.Gateway.Models.Inmuebles.DTOs;
using Api.Gateway.Models.Permisos.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Usuarios.DTOs
{
    public class UsuarioDto
    {
        public string Id { get; set; }
        public string NombreEmp { get; set; }
        public string PaternoEmp { get; set; }
        public string MaternoEmp { get; set; }
        public string Email { get; set; }
        public string RFC { get; set; }
        public string CURP { get; set; }
        public string Puesto { get; set; }
        public string ClavePuesto { get; set; }
        public string Usuario { get; set; }
        public int Expediente { get; set; }
        public IEnumerable<PermisoUsuarioDto> permisos { get; set; } = new List<PermisoUsuarioDto>();
    }
}
