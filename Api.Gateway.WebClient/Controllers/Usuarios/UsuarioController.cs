using Api.Gateway.Models.Permisos;
using Api.Gateway.Models.Usuarios.DTOs;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Modulos;
using Api.Gateway.Proxies.Permisos;
using Api.Gateway.Proxies.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Usuarios
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioProxy _usuarios;
        private readonly IPermisoProxy _permisos;
        private readonly IModuloProxy _modulos;
        private readonly ISubmoduloProxy _submodulos;
        private readonly IInmuebleProxy _inmuebles;

        public UsuarioController(IUsuarioProxy usuarios, IPermisoProxy permisos, IModuloProxy modulos, ISubmoduloProxy submodulos, IInmuebleProxy inmuebles)
        {
            _usuarios = usuarios;
            _permisos = permisos;
            _modulos = modulos;
            _submodulos = submodulos;
            _inmuebles = inmuebles;
        }

        [HttpGet]
        public async Task<List<UsuarioDto>> GetAllUsuariosAsync()
        {
            var result = await _usuarios.GetAllUsuariosAsync();

            return result;
        }

        [Route("getUsuarioById/{usuario}")]
        [HttpGet]
        public async Task<UsuarioDto> GetUsuarioById(string usuario)
        {
            var usuarios = await _usuarios.GetUsuarioByIdAsync(usuario);
            usuarios.permisos = await _permisos.GetPermisosUsuarioAsync(usuario);

            return usuarios;
        }
        
        [Route("getUsuariosByServicio/{usuario}/{servicio}")]
        [HttpGet]
        public async Task<List<UsuarioDto>> GetUsuariosByServicio(string usuario, int servicio)
        {
            var inmuebles = (await _inmuebles.GetAdministracionesByUsuarioServicio(usuario, servicio)).Select(i => i.Id).ToList();

            var usrId = (await _inmuebles.GetAllInmueblesUsuarios(servicio)).Where(i => inmuebles.Contains(i.InmuebleId) && 
                i.ServicioId == servicio).Select(i => i.UsuarioId).Distinct().ToList();

            var usuarios = (await _usuarios.GetAllUsuariosAsync()).Where(i => usrId.Contains(i.Id)).ToList();

            return usuarios;
        }
    }
}
