using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Update;
using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Cedulas;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Limpieza.Entregables;
using Api.Gateway.Proxies.Limpieza.Variables;
using Api.Gateway.WebClient.Procedures.ServiciosGenerales.Fumigacion;
using Api.Gateway.WebClient.Procedures.ServiciosGenerales.Limpieza;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Limpieza.Entregables
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("limpieza/entregablesCedula")]
    public class EntregableController : ControllerBase
    {
        private readonly ILEntregableProxy _entregables;
        private readonly IEstatusEntregableProxy _estatus;
        private readonly ICTEntregableProxy _centregable;
        private readonly ILEntregablesProcedure _pentregables;

        public EntregableController(ILEntregableProxy entregables, IEstatusEntregableProxy estatus, ICTEntregableProxy centregable,
                                    ILEntregablesProcedure pentregables)
        {
            _entregables = entregables;
            _estatus = estatus;
            _centregable = centregable;
            _pentregables = pentregables;
        }


        [Route("getEntregablesByCedula/{cedula}")]
        public async Task<List<EntregableDto>> GetEntregablesByCedula(int cedula)
        {
            var entregables = await _entregables.GetEntregablesByCedula(cedula);

            foreach (var en in entregables)
            {
                en.tipoEntregable = await _centregable.GetEntregableById(en.EntregableId);
            }

            return entregables;
        }

        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [HttpPut("actualizarEntregable")]
        public async Task<IActionResult> ActualizaEntregable([FromForm] EntregableCommandUpdate request)
        {
            var entregable = await _entregables.GetEntregableById(request.Id);
            if ((await _estatus.GetEEByIdAsync(entregable.EstatusId)).Nombre.Equals("Rechazado") ||
                (await _estatus.GetEEByIdAsync(entregable.EstatusId)).Nombre.Equals("Sin Iniciar"))
            {
                request.EstatusId = (await _estatus.GetAllEstatusEntregablesAsync()).SingleOrDefault(e => e.Nombre.Equals("En Proceso")).Id;
            }
            await _entregables.UpdateEntregable(request);
            return Ok();

        }

        [HttpPut]
        [Route("AREntregable")]
        public async Task<IActionResult> AREntregable([FromForm] EEntregableUpdateCommand entregable)
        {
            entregable.EstatusId = (await _estatus.GetAllEstatusEntregablesAsync()).SingleOrDefault(ee => ee.Nombre.Equals(entregable.Estatus)).Id;
            await _entregables.AUpdateEntregable(entregable);
            return Ok();

        }

        [HttpGet("visualizarEntregable/{anio}/{mes}/{folio}/{archivo}/{tipo}")]
        public async Task<string> FacturaPDF(int anio, string mes, string folio, string archivo, string tipo)
        {
            var file = await _entregables.VisualizarEntregable(anio, mes, folio, archivo, tipo);

            return file.ToString();
        }

        [Route("descargarEntregables")]
        [HttpPost]
        public async Task<string> DescargarEntregables([FromBody] DEntregablesCommand request)
        {
            request.Path = await _entregables.GetPathEntregables();
            var entregables = await _pentregables.DescargarEntregables(request);

            return entregables;
        }
    }
}
