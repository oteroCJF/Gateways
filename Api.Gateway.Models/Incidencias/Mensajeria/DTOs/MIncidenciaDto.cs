using Api.Gateway.Models.Catalogos.DTOs.Incidencias;
using Api.Gateway.Models.Catalogos.DTOs.Indemnizaciones;
using Api.Gateway.Models.Usuarios.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Incidencias.Mensajeria.DTOs
{
    public class MIncidenciaDto
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int IncidenciaId { get; set; }
        public int IndemnizacionId { get; set; }
        public int EstatusId { get; set; }//verificar si se generará otra tabla debido al modulo de seguimientos
        public int Pregunta { get; set; }
        public string NumeroGuia { get; set; }
        public string CodigoRastreo { get; set; }
        public decimal Sobrepeso { get; set; }
        public string TipoServicio { get; set; }
        public string Acuse { get; set; }
        public int TotalAcuses { get; set; }
        public string Acta { get; set; }
        public string Escrito { get; set; }
        public string? Comprobante { get; set; }
        public string Observaciones { get; set; }
        public bool Penalizable { get; set; }
        public decimal MontoPenalizacion { get; set; }
        public DateTime? FechaProgramada { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }

        public MConfiguracionIncidenciaDto ciMensajeria { get; set; } = new MConfiguracionIncidenciaDto();
        public UsuarioDto Usuario { get; set; } = new UsuarioDto();
        public CTIncidenciaDto Incidencia { get; set; } = new CTIncidenciaDto();
        public CTIndemnizacionDto Indemnizacion { get; set; } = new CTIndemnizacionDto();
    }
}
