using Api.Gateway.Models.Catalogos.DTOs.Entregables;
using Api.Gateway.Models.Catalogos.DTOs.Incidencias;
using Api.Gateway.Proxies.Catalogos.CTIncidencias;
using Api.Gateway.Proxies.Catalogos.CTIndemnizacion;
using Api.Gateway.Proxies.Catalogos.CTParametros;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Catalogos.CTIncidencias
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("catalogos/icomedor")]
    public class CTIComedorController : ControllerBase
    {
        private readonly ICTIComedorProxy _incidencias;
        private readonly ICTIncidenciaProxy _ctIncidencia;
        private readonly ICTParametroProxy _ctParametros;

        public CTIComedorController(ICTIComedorProxy incidencias, ICTIncidenciaProxy ctIncidencia, ICTParametroProxy ctParametros)
        {
            _incidencias = incidencias;
            _ctIncidencia = ctIncidencia;
            _ctParametros = ctParametros;
        }

        public async Task<List<CTIComedorDto>> GetAllIncidencias()
        {
            var incidencias = await _incidencias.GetAllIncidenciasAsync();
            return incidencias;
        }

        [HttpGet]
        [Route("getIncidenciasByTipo/{incidencia}")]
        public async Task<List<CTIComedorDto>> GetEntregablesServicio(int incidencia)
        {
            var incidencias = await _incidencias.GetIncidenciasByTipo(incidencia);
            return incidencias;
        }
        
        [HttpGet]
        [Route("getNombresByTipo/{tipo}")]
        public async Task<List<CTIComedorDto>> GetNombresByTipo(int tipo)
        {
            var incidencias = await _incidencias.GetNombresByTipo(tipo);
            return incidencias;
        }
        
        [HttpGet]
        [Route("getIncidenciaById/{incidencia}")]
        public async Task<CTIComedorDto> GetIncidenciaById(int incidencia)
        {
            var result = await _incidencias.GetIncidenciaById(incidencia);
            result.Tipo = await _ctParametros.GetParametroById(result.TipoId);
            result.Incidencia = await _ctIncidencia.GetIncidenciaById(result.IncidenciaId);

            return result;
        }
    }
}
