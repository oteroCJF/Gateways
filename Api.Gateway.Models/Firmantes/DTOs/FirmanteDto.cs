using Api.Gateway.Models.Inmuebles.DTOs.Inmuebles;
using Api.Gateway.Models.Usuarios.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Firmantes.DTOs
{
    public class FirmanteDto
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int InmuebleId { get; set; }
        public string Tipo { get; set; }
        public string Escolaridad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }

        public UsuarioDto Usuario { get; set; } = new UsuarioDto();
        public InmuebleDto Inmueble { get; set; } = new InmuebleDto();
    }
}
