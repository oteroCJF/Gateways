using Api.Gateway.Models.Catalogos.DTOs.Entregables;
using Api.Gateway.Models.Catalogos.DTOs.Incidencias;
using Api.Gateway.Proxies.Catalogos.CTIncidencias;
using Api.Gateway.Proxies.Catalogos.CTIndemnizacion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Catalogos.CTIncidencias
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("catalogos/ilimpieza")]
    public class CTILimpiezaController : ControllerBase
    {
        private readonly ICTILimpiezaProxy _incidencias;

        public CTILimpiezaController(ICTILimpiezaProxy incidencias)
        {
            _incidencias = incidencias;
        }

        public async Task<List<CTILimpiezaDto>> GetAllIncidencias()
        {
            var incidencias = await _incidencias.GetAllIncidenciasAsync();
            return incidencias;
        }

        [HttpGet]
        [Route("getIncidenciasByTipo/{incidencia}")]
        public async Task<List<CTILimpiezaDto>> GetEntregablesServicio(int incidencia)
        {
            var incidencias = await _incidencias.GetIncidenciasByTipo(incidencia);
            return incidencias;
        }
        
        [HttpGet]
        [Route("getNombresByTipo/{tipo}")]
        public async Task<List<CTILimpiezaDto>> GetNombresByTipo(string tipo)
        {
            var incidencias = await _incidencias.GetNombresByTipo(tipo);
            return incidencias;
        }
        
        [HttpGet]
        [Route("getIncidenciaById/{incidencia}")]
        public async Task<CTILimpiezaDto> GetIncidenciaById(int incidencia)
        {
            return await _incidencias.GetIncidenciaById(incidencia);
        }
    }
}
