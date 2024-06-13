using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Contratos;
using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Contratos;
using Api.Gateway.Proxies.Fumigacion.EntregablesContratacion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Fumigacion.EntregablesContratacion
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("fumigacion/entregablesContrato")]
    public class EntregableContratoController : ControllerBase
    {
        private readonly IFEContratoProxy _entregables;
        private readonly IHostingEnvironment _environment;

        public EntregableContratoController(IFEContratoProxy entregables, IHostingEnvironment environment)
        {
            _entregables = entregables;
            _environment = environment;
        }

        [Route("getEntregablesByContrato/{contrato}")]
        [HttpGet]
        public async Task<List<EContratoDto>> GetEntregablesByContrato(int contrato)
        {
            var entregables = await _entregables.GetEntregableContratacionByContrato(contrato);

            return entregables;
        }

        [Route("getEntregablesByContratoConvenio/{contrato}/{convenio}")]
        [HttpGet]
        public async Task<List<EContratoDto>> GetEntregablesByContratoConvenio(int contrato, int convenio)
        {
            var entregables = await _entregables.GetEntregableContratacionByContratoConvenio(contrato, convenio);

            return entregables;
        }

        [Route("getEntregableById/{entregable}")]
        [HttpGet]
        public async Task<EContratoDto> GetEntregableById(int entregable)
        {
            var entregables = await _entregables.GetEntregableById(entregable);

            return entregables;
        }

        [Consumes("multipart/form-data")]
        [Route("updateEntregableContratacion")]
        [HttpPut]
        public async Task<IActionResult> UpdateEntregable([FromForm] EntregableContratoUpdateCommand entregable)
        {
            entregable.Convenio = entregable.Convenio == null ? "" : entregable.Convenio;
            int status = await _entregables.UpdateEntregable(entregable);
            return Ok(status);
        }

        [Route("visualizarEntregableCont/{contrato}/{tipoEntregable}/{archivo}")]
        [HttpGet]
        public async Task<string> VisualizarEntregableCont(string contrato, string tipoEntregable, string archivo)
        {
            string ruta = await _entregables.VisualizarEntregablesCont(contrato, tipoEntregable, archivo);
            return ruta;

        }
        
        [Route("visualizarEntregableConv/{contrato}/{convenio}/{tipoEntregable}/{archivo}")]
        [HttpGet]
        public async Task<string> VisualizarEntregableConv(string contrato, string convenio, string tipoEntregable, string archivo)
        {
            string ruta = await _entregables.VisualizarEntregablesConv(contrato, convenio, tipoEntregable, archivo);
            return ruta;
        }
    }
}
