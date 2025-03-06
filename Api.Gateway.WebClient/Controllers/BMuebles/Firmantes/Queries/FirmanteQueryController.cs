using Api.Gateway.Models.Firmantes.Commands;
using Api.Gateway.Models.Firmantes.DTOs;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.BMuebles.Firmantes.Commands;
using Api.Gateway.Proxies.BMuebles.Firmantes.Queries;
using Api.Gateway.Proxies.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.BMuebles.Firmantes.Queries
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("bmuebles/firmantes")]
    public class FirmanteQueryController : ControllerBase
    {
        private readonly IQFirmanteBMueblesProxy _firmantes;
        private readonly IUsuarioProxy _usuarios;
        private readonly IInmuebleProxy _inmuebles;

        public FirmanteQueryController(IQFirmanteBMueblesProxy firmantes, IUsuarioProxy usuarios, IInmuebleProxy inmuebles)
        {
            _firmantes = firmantes;
            _usuarios = usuarios;
            _inmuebles = inmuebles;
        }

        [HttpGet]
        public async Task<List<FirmanteDto>> GetAllFirmantesAsync()
        {
            var firmantes = await _firmantes.GetAllFirmantesAsync();

            foreach (var fr in firmantes)
            {
                if (fr.InmuebleId != 0) { fr.Inmueble = await _inmuebles.GetInmuebleById(fr.InmuebleId); }
                if (!fr.UsuarioId.Equals("")) { fr.Usuario = await _usuarios.GetUsuarioByIdAsync(fr.UsuarioId); }
            }
            return firmantes;
        }

        [HttpGet]
        [Route("getFirmanteById/{firmante}")]
        public async Task<FirmanteDto> GetFirmanteById(int firmante)
        {
            var firmantes = await _firmantes.GetFirmanteById(firmante);
            firmantes.Inmueble = await _inmuebles.GetInmuebleById(firmantes.InmuebleId);
            firmantes.Usuario = await _usuarios.GetUsuarioByIdAsync(firmantes.UsuarioId);

            return firmantes;
        }

        [HttpGet]
        [Route("getFirmantesByInmueble/{inmueble}")]
        public async Task<List<FirmanteDto>> GetFirmantesByInmueble(int inmueble)
        {
            var firmantes = await _firmantes.GetFirmantesByInmueble(inmueble);
            foreach (var fr in firmantes)
            {
                fr.Inmueble = await _inmuebles.GetInmuebleById(fr.InmuebleId);
                fr.Usuario = await _usuarios.GetUsuarioByIdAsync(fr.UsuarioId);
            }
            return firmantes;
        }
    }

}
