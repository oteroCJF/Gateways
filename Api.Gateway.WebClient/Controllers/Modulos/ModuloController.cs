using Api.Gateway.Models.Meses.DTOs;
using Api.Gateway.Models.Modulos.DTOs;
using Api.Gateway.Proxies.Modulos;
using Api.Gateway.Proxies.Permisos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Modulos
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("modulos")]
    public class ModuloController : ControllerBase
    {
        private readonly IModuloProxy _modulos;
        private readonly ISubmoduloProxy _submodulos;
        private readonly IPermisoProxy _permisos;

        public ModuloController(IModuloProxy modulos, ISubmoduloProxy submodulos, IPermisoProxy permisos)
        {
            _modulos = modulos;
            _submodulos = submodulos;
            _permisos = permisos;
        }

        [HttpGet]

        public async Task<List<ModuloDto>> GetModulosAll()
        {
            var result = await _modulos.GetAllModulosAsync();

            foreach (var sb in result)
            {
                sb.Submodulos = await _submodulos.GetSubmoduloByModuloAsync(sb.Id);
                if (sb.Submodulos != null)
                {
                    foreach (var ps in sb.Submodulos)
                    {
                        ps.Permisos = await _permisos.GetPermisosBySubmoduloAsync(ps.Id);
                        foreach (var p in ps.Permisos)
                        {
                            p.Permiso = await _permisos.GetPermisosByIdAsync(p.PermisoId);
                        }
                    }
                }
            }

            return result;
        }

        [HttpGet("{modulo}")]
        public async Task<ModuloDto> GetModuloById(int modulo)
        {
            var result = await _modulos.GetModuloByIdAsync(modulo);

            return result;
        }
        
        [HttpGet("getSubmoduloById/{submodulo}")]
        public async Task<SubmoduloDto> GetSubmoduloById(int submodulo)
        {
            var result = await _submodulos.GetSubmoduloByIdAsync(submodulo);

            return result;
        }
    }
}
