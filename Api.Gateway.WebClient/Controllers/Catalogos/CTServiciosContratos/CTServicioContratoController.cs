using Api.Gateway.Models.Catalogos.DTOs.Servicios;
using Api.Gateway.Models.Catalogos.DTOs.ServiciosContratos;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Catalogos.CTServicios;
using Api.Gateway.Proxies.Catalogos.CTServiciosContratos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Catalogos.CTServiciosContratos
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("catalogos/serviciosContrato")]
    public class CTServicioController : ControllerBase
    {
        private readonly ICTServicioProxy _servicios;
        private readonly ICTServicioContratoProxy _scontratos;

        public CTServicioController(ICTServicioProxy servicios, ICTServicioContratoProxy scontratos)
        {
            _servicios = servicios;
            _scontratos = scontratos;
        }

        [HttpGet]
        public async Task<List<CTServicioContratoDto>> GetAllServiciosAsync()
        {
            return await _scontratos.GetAllServiciosContratosAsync();
        }

        [Route("getServiciosByServicio/{servicio}")]
        [HttpGet]
        public async Task<List<CTServicioContratoDto>> GetServicioByIdAsync(int servicio)
        {
            var servicios =  await _scontratos.GetServiciosByServicioAsync(servicio);

            return servicios;
        }
    }
}
