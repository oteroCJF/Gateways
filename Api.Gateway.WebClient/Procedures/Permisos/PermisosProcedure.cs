using Api.Gateway.Models.Permisos.DTOs;
using Api.Gateway.Proxies.Permisos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Procedures.Permisos
{
    public interface IPermisosProcedure
    {
        Task<List<PermisoSubmoduloDto>> GetPermisosBySubmodulo(int submodulo);
    }

    public class PermisosProcedure : IPermisosProcedure
    {
        private readonly IPermisoProxy _permisos;

        public PermisosProcedure(IPermisoProxy permisos)
        {
            _permisos = permisos;
        }

        public async Task<List<PermisoSubmoduloDto>> GetPermisosBySubmodulo(int submodulo)
        {
            var listPermisos = await _permisos.GetPermisosBySubmoduloAsync(submodulo);

            foreach (var pr in listPermisos)
            {
                pr.Permiso = await _permisos.GetPermisosByIdAsync(pr.PermisoId);
            }

            return listPermisos;
        }
    }
}
