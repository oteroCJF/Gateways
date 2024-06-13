using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Incidencias.Mensajeria.DTOs
{
    public class MSoportePagoDto
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public string NumeroGuia { get; set; }
        public string Cliente { get; set; }
        public string Pedido { get; set; }
        public string Recibio { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public DateTime FechaRecoleccion { get; set; }
        public DateTime FechaEntrega { get; set; }
        public decimal PesoReal { get; set; }
        public decimal SobrepesoReal { get; set; }
        public decimal MontoServicio { get; set; }
        public decimal MontoSobrepeso { get; set; }
        public decimal MontoAcuse { get; set; }
        public int Acuse { get; set; }
        public DateTime FechaFactura { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Nullable<DateTime> FechaActualizacion { get; set; }
        public Nullable<DateTime> FechaEliminacion { get; set; }
    }
}
