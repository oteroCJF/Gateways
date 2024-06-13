using Api.Gateway.Models.Catalogos.DTOs.MarcoJuridico;
using Api.Gateway.Models.Catalogos.DTOs.Parametros;
using Api.Gateway.Models.Catalogos.DTOs.Servicios;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Catalogos.CTMarcoJuridico;
using Api.Gateway.Proxies.Catalogos.CTParametros;
using Api.Gateway.Proxies.Catalogos.CTServicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Catalogos.CTParametros
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("catalogos/marcoJuridico")]
    public class MarcoJuridicoController : ControllerBase
    {
        private readonly ICTMarcoJuridicoProxy _marco;

        public MarcoJuridicoController(ICTMarcoJuridicoProxy marco)
        {
            _marco = marco;
        }

        [HttpGet]
        public async Task<List<MarcoJuridicoDto>> GetAllServiciosAsync()
        {
            return await _marco.GetAllMarcoJuridico();
        }

        [Route("getMarcoJuridicoById/{id}")]
        [HttpGet]
        public async Task<MarcoJuridicoDto> GetServicioByIdAsync(int id)
        {
            return await _marco.GetMarcoJuridicoById(id);
        }
    }
}
