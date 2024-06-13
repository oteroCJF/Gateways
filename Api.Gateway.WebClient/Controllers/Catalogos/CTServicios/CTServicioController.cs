using Api.Gateway.Models.Catalogos.DTOs.Servicios;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Catalogos.CTServicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Catalogos.CTServicios
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("catalogos/servicios")]
    public class CTServicioController : ControllerBase
    {
        private readonly ICTServicioProxy _servicios;
        private readonly ICTEntregableProxy _entregables;

        public CTServicioController(ICTServicioProxy servicios, ICTEntregableProxy entregables)
        {
            _servicios = servicios;
            _entregables = entregables;
        }

        [HttpGet]
        public async Task<List<CTServicioDto>> GetAllServiciosAsync()
        {
            return await _servicios.GetAllServiciosAsync();
        }

        [Route("getServicioById/{servicio}")]
        [HttpGet]
        public async Task<CTServicioDto> GetServicioByIdAsync(int servicio)
        {
            return await _servicios.GetServicioByIdAsync(servicio);
        }
    }
}
