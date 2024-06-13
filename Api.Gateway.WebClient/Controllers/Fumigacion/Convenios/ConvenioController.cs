using Api.Gateway.Models.Convenios.Commands;
using Api.Gateway.Models.Convenios.DTOs;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Catalogos.CTParametros;
using Api.Gateway.Proxies.Fumigacion.Convenios;
using Api.Gateway.Proxies.Fumigacion.EntregablesContratacion;
using Api.Gateway.Proxies.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Fumigacion.Convenios
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("fumigacion/convenios")]
    public class ConvenioController :ControllerBase
    {
        private readonly IUsuarioProxy _usuarios;
        private readonly IFConvenioProxy _convenios;
        private readonly IFEContratoProxy _entregables;
        private readonly ICTEntregableProxy _centregables;
        private readonly ICTParametroProxy _parametros;

        public ConvenioController(IUsuarioProxy usuarios, IFConvenioProxy convenios, IFEContratoProxy entregables, 
                                  ICTEntregableProxy centregables, ICTParametroProxy parametros)
        {
            _usuarios = usuarios;
            _convenios = convenios;
            _entregables = entregables;
            _centregables = centregables;
            _parametros = parametros;
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

                foreach (var p in conv.Rubros)
                {
                    p.Rubro = await _parametros.GetParametroById(p.RubroId);
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
