using Api.Gateway.Models.Convenios.Commands;
using Api.Gateway.Models.Convenios.DTOs;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Limpieza.Convenios;
using Api.Gateway.Proxies.Limpieza.EntregablesContratacion;
using Api.Gateway.Proxies.Limpieza.Variables;
using Api.Gateway.Proxies.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Limpieza.Convenios
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("limpieza/convenios")]
    public class ConvenioController : ControllerBase
    {
        private readonly IUsuarioProxy _usuarios;
        private readonly ILConvenioProxy _convenios;
        private readonly ILEContratoProxy _entregables;
        private readonly ICTEntregableProxy _centregables;

        public ConvenioController(IUsuarioProxy usuarios, ILConvenioProxy convenios, ILEContratoProxy entregables,
                                  ICTEntregableProxy centregables)
        {
            _usuarios = usuarios;
            _convenios = convenios;
            _entregables = entregables;
            _centregables = centregables;
        }

        [HttpGet]
        [Route("getConveniosByContrato/{contrato}")]
        public async Task<List<ConvenioDto>> getConveniosByContrato(int contrato)
        {
            List<ConvenioDto> convenios = await _convenios.GetConveniosByContrato(contrato);
            foreach (var conv in convenios)
            {
                conv.Usuario = await _usuarios.GetUsuarioByIdAsync(conv.UsuarioId);
                conv.EntregablesConvenio = await _entregables.GetEntregableContratacionByContratoConvenio(contrato, conv.Id);
                conv.Rubros = await _convenios.GetRubrosByConvenio(conv.Id);
                foreach (var v in conv.EntregablesConvenio)
                {
                    v.TipoEntregable = await _centregables.GetEntregableById(v.EntregableId);
                    v.Usuario = await _usuarios.GetUsuarioByIdAsync(v.UsuarioId);
                }
            }

            return convenios;
        }

        [HttpGet]
        [Route("getConvenioById/{convenio}")]
        public async Task<ConvenioDto> getConveniosById(int convenio)
        {
            ConvenioDto convenios = await _convenios.GetConvenioByIdAsync(convenio);

            return convenios;
        }

        [Route("createConvenio")]
        [HttpPost]
        public async Task<IActionResult> CreateConvenio([FromBody] ConvenioCreateCommand contrato)
        {
            int success = await _convenios.CreateConvenio(contrato);
            return Ok(success);
        }

        [Route("updateConvenio")]
        [HttpPut]
        public async Task<IActionResult> UpdateConvenio([FromBody] ConvenioUpdateCommand contrato)
        {
            int success = await _convenios.UpdateConvenio(contrato);
            return Ok(success);
        }

        [Route("deleteConvenio")]
        [HttpPut]
        public async Task<IActionResult> DeleteConvenio([FromBody] ConvenioDeleteCommand contrato)
        {
            int success = await _convenios.DeleteConvenio(contrato);
            return Ok(success);
        }
    }
}
