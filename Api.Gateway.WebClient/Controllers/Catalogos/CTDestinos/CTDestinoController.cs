using Api.Gateway.Models.Catalogos.DTOs.Destinos;
using Api.Gateway.Proxies.Catalogos.CTDestinos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Catalogos.CTDestinos
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("catalogos/destinos")]
    public class CTDestinoController : ControllerBase
    {
        private readonly ICTDestinoProxy _destinos;

        public CTDestinoController(ICTDestinoProxy destinos)
        {
            _destinos = destinos;
        }

        [HttpGet]
        public async Task<List<CTDestinoDto>> GetAllDestinosAsync()
        {
            return await _destinos.GetAllCTDestinos();
        }

        [Route("getDestinoById/{id}")]
        [HttpGet]
        public async Task<CTDestinoDto> GetPartidaByIdAsync(int id)
        {
            return await _destinos.GetDestinoByIdAsync(id);
        }
    }
}
