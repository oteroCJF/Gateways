using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Update
{
    public class EntregableCommandUpdate
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public IFormFile Archivo { get; set; }
        public int EstatusId { get; set; }
        public string Estatus { get; set; }
        public bool Validado { get; set; }
        public bool Validar { get; set; }
        public string Supervicion { get; set; }
        public string Observaciones { get; set; }

        //Variables Adicionales
        public int Anio { get; set; }
        public string Mes { get; set; }
        public string Folio { get; set; }
        public string TipoEntregable { get; set; }
    }
}
