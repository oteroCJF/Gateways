using Api.Gateway.Models.Catalogos.DTOs.Entregables;
using Api.Gateway.Models.Catalogos.DTOs.Incidencias;
using Api.Gateway.Proxies.Catalogos.CTIncidencias;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Catalogos.CTIncidencias
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("catalogos/incidencias")]
    public class CTIncidenciaController : ControllerBase
    {
        private readonly ICTIncidenciaProxy _incidencias;

        public CTIncidenciaController(ICTIncidenciaProxy incidencias)
        {
            _incidencias = incidencias;
        }

        public async Task<List<CTIncidenciaDto>> GetAllIncidencias()
        {
            var incidencias = await _incidencias.GetAllIncidenciasAsync();
            return incidencias;
        }

        [HttpGet]
        [Route("getIncidenciasByServicio/{servicio}")]
        public async Task<List<CTIncidenciaDto>> GetEntregablesServicio(int servicio)
        {
            var incidencias = await _incidencias.GetIncidenciasByServicio(servicio);
            return incidencias;
        }
        
        [HttpGet]
        [Route("getIncidenciaById/{incidencia}")]
        public async Task<CTIncidenciaDto> GetIncidenciaById(int incidencia)
        {
            return await _incidencias.GetIncidenciaById(incidencia);
        }
    }
}
