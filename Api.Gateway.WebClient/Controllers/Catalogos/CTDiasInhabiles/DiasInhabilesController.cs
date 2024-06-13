using Api.Gateway.Models.Catalogos.DTOs.Destinos;
using Api.Gateway.Models.Catalogos.DTOs.DiasInhabiles;
using Api.Gateway.Proxies.Catalogos.CTDiasInhabiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Catalogos.CTDiasInhabiles
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("catalogos/diasInhabiles")]
    public class DiasInhabilesController : ControllerBase
    {
        private readonly ICTDiasInhabilesProxy _dias;

        public DiasInhabilesController(ICTDiasInhabilesProxy dias)
        {
            _dias = dias;
        }

        [HttpGet]
        public async Task<List<DiaInhabilDto>> GetAllDiasInhabilesAsync()
        {
            return await _dias.GetAllDiasInhabiles();
        }

        [Route("getDiasInhabilesByAnio/{anio}")]
        [HttpGet]
        public async Task<DiaInhabilDto> GetPartidaByIdAsync(int anio)
        {
            return await _dias.GetDiasInhabilesByAnio(anio);
        }
        
        [Route("esdiaInhabil/{anio}/{fecha}")]
        [HttpGet]
        public async Task<bool> EsdiaInhabil(int anio, string fecha)
        {
            var esHabil = await _dias.EsDiaInhabil(anio, fecha);
            return esHabil;
        }
    }
}
