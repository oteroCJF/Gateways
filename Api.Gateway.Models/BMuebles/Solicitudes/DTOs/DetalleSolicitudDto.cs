using Api.Gateway.Models.Inmuebles.DTOs.Inmuebles;
using Api.Gateway.Models.Usuarios.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.BMuebles.Solicitudes.DTOs
{
    public class DetalleSolicitudDto
    {
        public int SolicitudId { get; set; }
        public string Concepto { get; set; }
        public int Unidades { get; set; }
        public int Estibadores { get; set; }
        public string UEntregaId { get; set; }
        public UsuarioDto Entrega { get; set; } = new UsuarioDto();
        public string URecibeId { get; set; }
        public UsuarioDto Recibe { get; set; } = new UsuarioDto();
        public string Telefono { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
