using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;
using Api.Gateway.Models.Inmuebles.DTOs.Parametros;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Inmuebles.DTOs.Inmuebles
{
    public class InmuebleDto
    {
        public int Id { get; set; }
        public Nullable<int> AdministracionId { get; set; }
        public int Clave { get; set; }
        public string? ClienteEstafeta { get; set; }
        public string HorarioComedor { get; set; }
        public Nullable<bool> Estafeta { get; set; }
        public string? Nombre { get; set; }
        public string? Abreviacion { get; set; }
        public string? Descripcion { get; set; }
        public string? DescripcionAdministrador { get; set; }
        public string? Direccion { get; set; }
        public string? Estado { get; set; }
        public Nullable<int> Tipo { get; set; }
        public string? Administrador { get; set; }

        public virtual CedulaDto Cedula { get; set; } = new CedulaDto();
        public virtual List<CFDIDto> Facturas { get; set; } = new List<CFDIDto>();
    }
}
