using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Contratos;
using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Contratos;
using Api.Gateway.Proxies.Convencional.EntregablesContrato.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Convencional.EntregablesContrato.Queries
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("convencional/entregablesContrato")]
    public class EContratoCommandController : ControllerBase
    {
        private readonly IQEContratoConvencionalProxy _entregables;

        public EContratoCommandController(IQEContratoConvencionalProxy entregables)
        {
            _entregables = entregables;
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
