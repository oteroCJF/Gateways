using Api.Gateway.Models.Catalogos.DTOs.Parametros;
using Api.Gateway.Models.Catalogos.DTOs.Servicios;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
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
    [Route("catalogos/parametros")]
    public class CTParametroController : ControllerBase
    {
        private readonly ICTParametroProxy _parametros;

        public CTParametroController(ICTParametroProxy parametros)
        {
            _parametros = parametros;
        }

        [HttpGet]
        public async Task<List<CTParametroDto>> GetAllServiciosAsync()
        {
            return await _parametros.GetAllParametrosAsync();
        }

        [Route("getParametroById/{parametro}")]
        [HttpGet]
        public async Task<CTParametroDto> GetServicioByIdAsync(int parametro)
        {
            return await _parametros.GetParametroById(parametro);
        }
        
        [Route("getParametroByTipo/{tipo}")]
        [HttpGet]
        public async Task<List<CTParametroDto>> GetParametroByTipo(string tipo)
        {
            return await _parametros.GetParametroByTipo(tipo);
        }
        
        [Route("getParametroByTabla/{tabla}")]
        [HttpGet]
        public async Task<List<CTParametroDto>> GetParametroByTabla(string tabla)
        {
            return await _parametros.GetParametroByTabla(tabla);
        }
    }
}
