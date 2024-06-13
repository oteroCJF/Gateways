using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas;
using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Cedulas;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Mensajeria.Entregables;
using Api.Gateway.WebClient.Controllers.Mensajeria.Entregables.Procedures.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Mensajeria.Entregables.Queries
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("mensajeria/entregablesCedula")]
    public class EntregableQueryController : ControllerBase
    {
        private readonly IQEntregableMensajeriaProxy _entregables;
        private readonly IEstatusEntregableProxy _estatus;
        private readonly ICTEntregableProxy _centregable;
        private readonly IQMensajeriaEntregableProcedure _pentregables;

        public EntregableQueryController(IQEntregableMensajeriaProxy entregables, IEstatusEntregableProxy estatus, ICTEntregableProxy centregable,
                                    IQMensajeriaEntregableProcedure pentregables)
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
