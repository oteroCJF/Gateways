using Api.Gateway.Models.CFDIs.ServiciosGenerales.Commands;
using Api.Gateway.Proxies.Agua.CFDIs.Commands;
using Api.Gateway.WebClient.Controllers.Agua.CFDIs.Procedure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Agua.CFDIs.Commands
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("agua/cfdi")]
    public class CFDICommandController : ControllerBase
    {
        private readonly ICCFDIAguaProxy _cfdi;
        private readonly ICFDIAguaProcedure _cfdiProcedure;

        public CFDICommandController(ICCFDIAguaProxy cfdi, ICFDIAguaProcedure cfdiProcedure)
        {
            _cfdi = cfdi;
            _cfdiProcedure = cfdiProcedure;
        }

        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [HttpPost("createFactura")]
        public async Task<IActionResult> Create([FromForm] CFDICreateCommand command)
        {
            var factura = await _cfdi.CreateFactura(command);
            if (command.XML != null && command.PDF == null)
            {
                HistorialMFCreateCommand historial = new HistorialMFCreateCommand();
                historial.Anio = command.Anio;
                historial.Mes = command.Mes;
                historial.RepositorioId = command.RepositorioId;
                historial.InmuebleId = command.InmuebleId;
                historial.UsuarioId = command.UsuarioId;
                historial.TipoArchivo = factura.Tipo;
                historial.Facturacion = command.TipoFacturacion;
                historial.ArchivoXML = factura.ArchivoXML;
                historial.ArchivoPDF = "";

                historial = _cfdiProcedure.GetObservacionesHCM(historial, factura);

                await _cfdi.CreateHistorialMF(historial);
            }

            return Ok(factura);
        }

        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [HttpPut("updateFactura")]
        public async Task<IActionResult> UpdateFactura([FromForm] CFDIUpdateCommand command)
        {
            var factura = await _cfdi.UpdateFactura(command);
            HistorialMFCreateCommand historial = new HistorialMFCreateCommand();
            historial.Anio = command.Anio;
            historial.Mes = command.Mes;
            historial.RepositorioId = factura.RepositorioId;
            historial.InmuebleId = factura.InmuebleId;
            historial.UsuarioId = factura.UsuarioId;
            historial.TipoArchivo = factura.Tipo;
            historial.Facturacion = factura.Facturacion;
            historial.ArchivoXML = "";
            historial.ArchivoPDF = factura.ArchivoPDF;

            historial = _cfdiProcedure.GetObservacionesHCM(historial, factura);

            await _cfdi.CreateHistorialMF(historial);

            return Ok(factura);
        }

        [Route("createHistorialMF")]
        [HttpPost]
        public async Task<IActionResult> CreateHistorialMF([FromBody] HistorialMFCreateCommand historial)
        {
            await _cfdi.CreateHistorialMF(historial);
            return Ok();
        }
    }
}
