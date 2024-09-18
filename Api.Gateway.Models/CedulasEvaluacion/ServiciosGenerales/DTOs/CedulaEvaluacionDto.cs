using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs
{
    public class CedulaEvaluacionDto
    {
        public int Id {  get; set; }
        public int Anio {  get; set; }
        public int ContratoId {  get; set; }
        public int MesId {  get; set; }
        public string Mes {  get; set; }
        public int InmuebleId {  get; set; }
        public string Inmueble {  get; set; }
        public string Folio {  get; set; }
        public int EstatusId {  get; set; }
        public string Estatus {  get; set; }
        public string Fondo {  get; set; }
        public int ServicioId { get; set; }
        public string Servicio { get; set; }
        public decimal Calificacion {  get; set; }
        public decimal Penalizacion {  get; set; }
        public Nullable<decimal> Factura {  get; set; }
        public Nullable<decimal> NC{  get; set; }
        public Nullable<decimal> TotalDeductivas {  get; set; }
        public Nullable<bool> RequiereNC {  get; set; }
        public bool Bloqueada { get; set; }
        public bool Cedula {  get; set; }
        public bool ActaFirmada {  get; set; }
        public Nullable<bool> Memorandum {  get; set; }
        public Nullable<DateTime> FechaInicial {  get; set; }
        public Nullable<DateTime> FechaFinal {  get; set; }
        public Nullable<DateTime> FechaCreacion {  get; set; }
        public Nullable<DateTime> FechaActualizacion {  get; set; }
    }
}
