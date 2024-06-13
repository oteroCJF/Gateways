using Api.Gateway.Models.Contratos.DTOs;
using Api.Gateway.Models.Meses.DTOs;
using Api.Gateway.Models.Usuarios.DTOs;
using System;

namespace Api.Gateway.Models.Repositorios.DTOs
{
    public class RepositorioDto
    {
        public int Id { get; set; }
        public int ContratoId { get; set; }
        public ContratoDto Contrato { get; set; } = new ContratoDto();
        public int MesId { get; set; }
        public MesDto Mes { get; set; } = new MesDto();
        public int Anio { get; set; }
        public string UsuarioId { get; set; }
        public UsuarioDto Usuario { get; set; } = new UsuarioDto();
        public string Estatus { get; set; }
        public int Facturas { get; set; }
        public int NotasCredito { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
