using Api.Gateway.Models.Contratos.DTOs;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Catalogos.CTParametros;
using Api.Gateway.Proxies.Catalogos.CTServiciosContratos;
using Api.Gateway.Proxies.Convencional.Contratos.Queries;
using Api.Gateway.Proxies.Convencional.Convenios.Queries;
using Api.Gateway.Proxies.Convencional.EntregablesContrato.Queries;
using Api.Gateway.Proxies.Convencional.ServiciosContrato.Queries;
using Api.Gateway.Proxies.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Convencional.Contratos.Queries
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("convencional/contratos")]
    public class ContratoConvencionalController : ControllerBase
    {
        private readonly IUsuarioProxy _usuarios;
        
        private readonly ICTEntregableProxy _centregables;
        private readonly ICTParametroProxy _parametros;
        private readonly ICTServicioContratoProxy _serviciosc;

        private readonly IQContratoConvencionalProxy _contratos;
        private readonly IQConvenioConvencionalProxy _convenios;
        private readonly IQSContratoConvencionalProxy _scontrato;
        private readonly IQEContratoConvencionalProxy _entregables;

        public ContratoConvencionalController(IUsuarioProxy usuarios, IQContratoConvencionalProxy contratos, IQConvenioConvencionalProxy convenios, 
                                          IQSContratoConvencionalProxy scontrato, IQEContratoConvencionalProxy entregables, ICTEntregableProxy centregables, 
                                          ICTParametroProxy parametros, ICTServicioContratoProxy serviciosc)
        {
            _usuarios = usuarios;
            _contratos = contratos;
            _convenios = convenios;
            _scontrato = scontrato;
            _entregables = entregables;
            _centregables = centregables;
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
    }
}
