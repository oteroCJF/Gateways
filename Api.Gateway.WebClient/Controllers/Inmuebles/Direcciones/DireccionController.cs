using Api.Gateway.Models.Inmuebles.Commands.Direcciones;
using Api.Gateway.Models.Inmuebles.DTOs.Direcciones;
using Api.Gateway.Proxies.Inmuebles.Direcciones;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Inmuebles.Direcciones
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("inmuebles")]
    public class DireccionController : ControllerBase
    {
        private readonly IDireccionProxy _direccion;

        public DireccionController(IDireccionProxy direccion)
        {
            _direccion = direccion;
        }
        [HttpGet]
        [Route("getDirecciones")]
        public async Task<List<DireccionDto>> GetAllDirecciones()
        {
            return await _direccion.GetAllDirecciones();
        }

        [HttpGet]
        [Route("getDireccionById/{id}")]
        public async Task<DireccionDto> GetInmuebleById(int id)
        {
            return await _direccion.GetDireccionById(id);
        }


        [HttpPost]
        [Route("createDireccion")]
        public async Task<IActionResult> CreateInmuebleUS([FromBody] CreateDireccionCommand request)
        {
            var direccion = await _direccion.CreateDireccion(request);

            return Ok(direccion);
        }


        [Route("updateDireccion")]
        [HttpPut]
        public async Task<IActionResult> UpdateDireccion([FromBody] UpdateDireccionCommand request)
        {
            var direccion = await _direccion.UpdateDireccion(request);

            return Ok(direccion);
        }

    }
}
