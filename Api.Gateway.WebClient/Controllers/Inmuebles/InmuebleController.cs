using Api.Gateway.Models.Inmuebles.Commands;
using Api.Gateway.Models.Inmuebles.Commands.SedesUsuarios;
using Api.Gateway.Models.Inmuebles.DTOs.Inmuebles;
using Api.Gateway.Models.Inmuebles.DTOs.InmueblesServicio;
using Api.Gateway.Models.Inmuebles.DTOs.InmueblesUS;
using Api.Gateway.Proxies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("inmuebles")]
    public class InmuebleController : ControllerBase
    {
        private readonly IInmuebleProxy _inmuebles;

        public InmuebleController(IInmuebleProxy inmuebles)
        {
            _inmuebles = inmuebles;
        }

        [HttpGet]
        [Route("getInmuebles")]
        public async Task<List<InmuebleDto>> GetAllInmueblesAsync()
        {
            var inmuebles = await _inmuebles.GetAllInmueblesAsync();
            return inmuebles;
        }

        [HttpGet]
        [Route("getAdministraciones")]
        public async Task<List<InmuebleDto>> GetAllAdministraciones()
        {
            return await _inmuebles.GetAllAdministraciones();
        }
        
        [HttpGet]
        [Route("getInmueblesServicio")]
        public async Task<List<InmuebleServicioDto>> GetAllInmueblesServicio()
        {
            return await _inmuebles.GetAllInmueblesServicio();
        }

        [HttpGet]
        [Route("getInmueblesByAdministracion/{administracion}")]
        public async Task<List<InmuebleDto>> GetInmueblesByAdministracion(int administracion)
        {
            return await _inmuebles.GetInmueblesByAdministracion(administracion);
        }

        [HttpGet]
        [Route("getInmueblesByServicio/{servicio}")]
        public async Task<List<InmuebleServicioDto>> GetInmueblesByServicio(int servicio)
        {
            return await _inmuebles.GetInmueblesByServicio(servicio);
        }

        [HttpGet]
        [Route("getInmueblesByUsuario/{usuario}")]
        public async Task<List<InmuebleUSDto>> GetInmueblesByUsuario(string usuario)
        {
            return await _inmuebles.GetInmueblesByUsuario(usuario);
        }
        
        [Route("getInmueblesByUsuarioServicio/{usuario}/{servicio}")]
        public async Task<List<InmuebleUSDto>> GetInmueblesByUsuarioServicio(string usuario, int servicio)
        {
            return await _inmuebles.GetInmueblesByUsuarioServicio(usuario, servicio);
        }

        [HttpGet]
        [Route("getInmuebleById/{inmueble}")]
        public async Task<InmuebleDto> GetInmuebleById(int inmueble)
        {
            return await _inmuebles.GetInmuebleById(inmueble);
        }

        [HttpPost]
        [Route("createInmuebleUS")]
        public async Task<IActionResult> CreateInmuebleUS([FromBody] List<CreateCommandInmuebleUS> inmuebleUS)
        {
            await _inmuebles.CreateInmuebleUS(inmuebleUS);
            return Ok();
        }

        [Route("deleteInmuebleSUS/{usuario}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteInmueblesUS(string usuario)
        {
            await _inmuebles.DeleteInmuebleUS(usuario);
            return Ok();
        }

        [HttpGet]
        [Route("getSedeByUsuario/{usuario}")]
        public async Task<InmuebleDto> GetSedeByUsuario(string usuario)
        {
            var inmuebleSede = await _inmuebles.GetSedeByUsuario(usuario);
            return inmuebleSede;
        }


        [HttpPost]
        [Route("createSede")]
        public async Task<IActionResult> CreateSedeUsuario([FromBody] CreateSedeUsuarioCommand usuario)
        {
            var sede = await _inmuebles.CreateSedeByUsuario(usuario);
            return Ok(sede);
        }


        [Route("deleteSede/{usuario}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSede(string usuario)
        {
            var sede = await _inmuebles.DeleteSedeByUsuario(usuario);

            return Ok(sede);
        }
    }
}
