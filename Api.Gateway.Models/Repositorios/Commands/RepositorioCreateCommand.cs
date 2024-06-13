using System;

namespace Api.Gateway.Models.Repositorios.Commands
{
    public class RepositorioCreateCommand
    {
        public int ContratoId { get; set; }
        public string UsuarioId { get; set; }
        public int MesId { get; set; }
        public int Anio { get; set; }
        public string Estatus { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
