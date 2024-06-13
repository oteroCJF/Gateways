using Api.Gateway.Proxies.Catalogos.CTIndemnizacion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Catalogos.CTIndemnizaciones
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("catalogos/indemnizacion")]
    public class IndemnizacionController : ControllerBase
    {
        private readonly ICTIndemnizacionProxy _indemnizacion;

        public IndemnizacionController(ICTIndemnizacionProxy indemnizacion)
        {
            _indemnizacion = indemnizacion;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIndemnizaciones()
        {
            var indemnizaciones = await _indemnizacion.GetAllIndemnizacionesAsync();

            return Ok(indemnizaciones);
        }

        [HttpGet]
        [Route("getIndemnizacionByIncidencia/{incidencia}")]
        public async Task<IActionResult> GetIndemnizacionByIncidencia(int incidencia)
        {
            var indemnizaciones = await _indemnizacion.GetIndemnizacionByIncidencia(incidencia);

            return Ok(indemnizaciones);
        }

        [HttpGet]
        [Route("getIndemnizacionById/{id}")]
        public async Task<IActionResult> GetIndemnizacionById(int id)
        {
            var indemnizaciones = await _indemnizacion.GetIndemnizacionById(id);

            return Ok(indemnizaciones);
        }
    }
}
