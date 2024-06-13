using Api.Gateway.Models.Modulos.DTOs;
using Api.Gateway.Models.Permisos.Commands;
using Api.Gateway.Models.Permisos.DTOs;
using Api.Gateway.Proxies.Modulos;
using Api.Gateway.Proxies.Permisos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Permisos
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("permisos")]
    public class PermisoController : ControllerBase
    {
        private readonly IPermisoProxy _permisos;
        private readonly IModuloProxy _modulos;
        private readonly ISubmoduloProxy _submodulos;
        public PermisoController(IPermisoProxy permisos, IModuloProxy modulos, ISubmoduloProxy submodulos)
        {
            _permisos = permisos;
            _modulos = modulos;
            _submodulos = submodulos;
        }

        [HttpGet]
        [Route("getPermisosByUsuario/{usuario}")]
        public async Task<IEnumerable<PermisoUsuarioDto>> GetPermisosByUsuario(string usuario)
        {
            var result = await _permisos.GetPermisosUsuarioAsync(usuario);

            return result;
        }

        [Route("getModulosByUsuario/{usuario}")]
        public async Task<List<ModuloDto>> GetModulosByUsuario(string usuario)
        {
            var pmUsuario = (await _permisos.GetPermisosUsuarioAsync(usuario)).Select(x => x.ModuloId).Distinct().ToList();
            var psmUsuario = (await _permisos.GetPermisosUsuarioAsync(usuario)).Select(x => x.SubmoduloId).Distinct().ToList();

            var modulos = (await _modulos.GetAllModulosAsync()).Where(m => pmUsuario.Contains(m.Id)).OrderBy(m => m.ServicioId).ToList();

            foreach (var sb in modulos)
            {
                sb.Submodulos = (await _submodulos.GetSubmoduloByModuloAsync(sb.Id)).Where(sm => psmUsuario.Contains(sm.Id)).ToList();
            }

            return modulos;
        }

        [Route("getPermisosByModuloUsuario/{usuario}/{modulo}")]
        public async Task<IEnumerable<PermisoUsuarioDto>> GetPermisosByModuloUsuario(string usuario, int modulo)
        {
            var collection = await _permisos.GetPermisosByModuloUsuarioAsync(usuario, modulo);

            foreach (var pr in collection)
            {
                pr.Modulo = await _modulos.GetModuloByIdAsync(pr.ModuloId);
                pr.Submodulo = await _submodulos.GetSubmoduloByIdAsync(pr.SubmoduloId);
                pr.Permiso = await _permisos.GetPermisosByIdAsync(pr.PermisoId);
            }

            return collection;
        }

        [Route("createPermisosByUsuario")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] List<PermisoCreateCommand> permisos)
        {
            await _permisos.CreatePermisos(permisos);
            return Ok();
        }

        [Route("deletePermisosByUsuario/{usuario}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string usuario)
        {
            PermisoDeleteCommand permisos = new PermisoDeleteCommand();
            permisos.UsuarioId = usuario;
            await _permisos.DeletePermisos(permisos);
            return Ok();
        }
    }
}
