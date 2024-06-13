using Api.Gateway.Models.Contratos.Commands;
using Api.Gateway.Models.Contratos.DTOs;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Catalogos.CTParametros;
using Api.Gateway.Proxies.Catalogos.CTServicios;
using Api.Gateway.Proxies.Catalogos.CTServiciosContratos;
using Api.Gateway.Proxies.Limpieza.Contratos;
using Api.Gateway.Proxies.Limpieza.Convenios;
using Api.Gateway.Proxies.Limpieza.EntregablesContratacion;
using Api.Gateway.Proxies.Limpieza.ServicioContrato;
using Api.Gateway.Proxies.Limpieza.Variables;
using Api.Gateway.Proxies.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Limpieza.Contratos
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("limpieza/contratos")]
    public class ContratoLimpiezaController : ControllerBase
    {
        private readonly IUsuarioProxy _usuarios;
        private readonly ILConvenioProxy _convenios;
        private readonly ILContratoProxy _contratos;
        private readonly ILServicioContratoProxy _scontrato;
        private readonly ILEContratoProxy _entregables;
        private readonly ICTEntregableProxy _centregables;
        private readonly ICTServicioProxy _servicios;
        private readonly ICTParametroProxy _parametros;
        private readonly ICTServicioContratoProxy _serviciosc;

        public ContratoLimpiezaController(IUsuarioProxy usuarios, ILConvenioProxy convenios, ILContratoProxy contratos, 
                                          ILServicioContratoProxy scontrato, ILEContratoProxy entregables, 
                                          ICTEntregableProxy centregables, ICTServicioProxy servicios, ICTParametroProxy parametros, 
                                          ICTServicioContratoProxy serviciosc)
        {
            _usuarios = usuarios;
            _convenios = convenios;
            _contratos = contratos;
            _scontrato = scontrato;
            _entregables = entregables;
            _centregables = centregables;
            _servicios = servicios;
            _parametros = parametros;
            _serviciosc = serviciosc;
        }

        [Route("getContratos")]
        public async Task<List<ContratoDto>> GetAllContratos()
        {
            List<ContratoDto> contratos = await _contratos.GetAllContratosAsync();
            foreach (var cn in contratos)
            {
                cn.Usuario = await _usuarios.GetUsuarioByIdAsync(cn.UsuarioId);
            }
            return contratos;
        }

        [Route("getContratoById/{contrato}")]
        public async Task<ContratoDto> GetContratoById(int contrato)
        {
            ContratoDto gContrato = await _contratos.GetContratoByIdAsync(contrato);

            gContrato.Usuario = await _usuarios.GetUsuarioByIdAsync(gContrato.UsuarioId);
            gContrato.Convenios = await _convenios.GetConveniosByContrato(gContrato.Id);
            gContrato.EntregablesContrato = await _entregables.GetEntregableContratacionByContrato(gContrato.Id);
            gContrato.ServiciosContrato = await _scontrato.GetServiciosByContrato(contrato);

            foreach (var sc in gContrato.ServiciosContrato)
            {
                sc.Servicio = await _serviciosc.GetServicioContratoByIdAsync(sc.ServicioId);
            }

            foreach (var v in gContrato.EntregablesContrato)
            {
                v.TipoEntregable = await _centregables.GetEntregableById(v.EntregableId);
                v.Usuario = await _usuarios.GetUsuarioByIdAsync(v.UsuarioId);
            }

            foreach (var cv in gContrato.Convenios)
            {
                cv.Rubros = await _convenios.GetRubrosByConvenio(cv.Id);
                foreach (var r in cv.Rubros)
                {
                    r.Rubro = await _parametros.GetParametroById(r.RubroId);
                }
            }

            return gContrato;
        }

        [Route("createContrato")]
        [HttpPost]
        public async Task<IActionResult> CreateContrato([FromBody] ContratoCreateCommand contrato)
        {
            int success = await _contratos.CreateContrato(contrato);
            return Ok(success);
        }

        [Route("updateContrato")]
        [HttpPut]
        public async Task<IActionResult> UpdateContrato([FromBody] ContratoUpdateCommand contrato)
        {
            int success = await _contratos.UpdateContrato(contrato);
            return Ok(success);
        }

        [Route("deleteContrato")]
        [HttpPut]
        public async Task<IActionResult> DeleteContrato([FromBody] ContratoDeleteCommand contrato)
        {
            int success = await _contratos.DeleteContrato(contrato);
            return Ok(success);
        }
    }
}
