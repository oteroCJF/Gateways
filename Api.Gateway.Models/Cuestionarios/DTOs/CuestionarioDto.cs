using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Cuestionarios.DTOs
{
    public class CuestionarioDto
    {
        public int Id { get; set; }
        public int IncidenciaId { get; set; }
        public int NoPregunta { get; set; }
        public string Abreviacion { get; set; }
        public string Concepto { get; set; }
        public string Pregunta { get; set; }
        public string Ayuda { get; set; }
        public string Botones { get; set; }
        public string Icono { get; set; }
        public bool NoAplica { get; set; }
        public bool NoRealizo { get; set; }
        public bool NoEntrego { get; set; }
        public bool Incidencias { get; set; }
        public bool CargaMasiva { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        //Esquema de Penalización
        public int? Ponderacion { get; set; }
        public string Tipo { get; set; }
        public string Formula { get; set; }
        public decimal Porcentaje { get; set; }
        public bool? ACLRS { get; set; }
        public int Consecutivo{ get; set; }
        public int CategoriaId { get; set; }


    }
}
