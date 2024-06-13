using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Oficios.Commands
{
    public class OficioCreateCommand
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int ContratoId { get; set; }
        public int ServicioId { get; set; }
        public int Anio { get; set; }
        public string NumeroOficio { get; set; }
        public DateTime FechaTramitado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public IFormFile Oficio { get; set; }
    }
}
