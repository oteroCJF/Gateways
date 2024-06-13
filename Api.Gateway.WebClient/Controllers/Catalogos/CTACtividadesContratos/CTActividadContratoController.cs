using Api.Gateway.Models.Catalogos.DTOs.ActividadesContrato;
using Api.Gateway.Proxies.Catalogos.CTActividadesContrato;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Catalogos.CTACtividadesContratos
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("catalogos/actividades")]
    public class CTActividadContratoController : ControllerBase
    {
        private readonly ICTActividadContratoProxy _actividades;

        public CTActividadContratoController(ICTActividadContratoProxy actividades)
        {
            _actividades = actividades;
        }

        [HttpGet]
        public async Task<List<CTActividadContratoDto>> GetAllCTActividades()
        {
            return await _actividades.GetAllCTActividades();
        }

        [Route("getActividadById/{id}")]
        [HttpGet]
        public async Task<CTActividadContratoDto> GetActividadByIdAsync(int id)
        {
            return await _actividades.GetActividadByIdAsync(id);
        }
    }
}
