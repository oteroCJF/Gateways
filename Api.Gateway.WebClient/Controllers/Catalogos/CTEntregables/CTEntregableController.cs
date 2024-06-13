using Api.Gateway.Models.Catalogos.DTOs.Entregables;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Catalogos.CTServicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Catalogos.CTEntregables
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("catalogos/entregables")]
    public class CTEntregableController : ControllerBase
    {
        private readonly ICTEntregableProxy _entregables;
        private readonly ICTServicioProxy _servicios;

        public CTEntregableController(ICTEntregableProxy entregables, ICTServicioProxy servicios)
        {
            _entregables = entregables;
            _servicios = servicios;
        }

        public async Task<List<CTEntregableDto>> GetAllEntregables()
        {
            var entregables = await _entregables.GetAllCTEntregables();
            return entregables;
        }

        [HttpGet]
        [Route("getEntregablesByServicio/{servicio}")]
        public async Task<List<CTEntregableDto>> GetEntregablesServicio(int servicio)
        {
            var eServicio = (await _entregables.GetEntregablesByServicio(servicio)).Select(es => es.EntregableId).ToList();
            var entregables = (await _entregables.GetAllCTEntregables()).Where(e => eServicio.Contains(e.Id)).ToList();
            
            return entregables;
        }

        [HttpGet]
        [Route("getEntregableById/{entregable}")]
        public async Task<CTEntregableDto> GetEntregableById(int entregable)
        {
            var entregables = await _entregables.GetEntregableById(entregable);

            return entregables;
        }
    }
}
