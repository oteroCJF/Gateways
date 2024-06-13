using Api.Gateway.Models.Catalogos.DTOs.Parametros;
using Api.Gateway.Models.Parametrizacion.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Convenios.DTOs
{
    public class RubroConvenioDto
    {
        public int ConvenioId { get; set; }
        public int RubroId { get; set; }

        public CTParametroDto Rubro { get; set; } = new CTParametroDto();
    }
}
