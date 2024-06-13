﻿using Api.Gateway.Models.Convenios.Commands;
using Api.Gateway.Models.Convenios.DTOs;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Catalogos.CTParametros;
using Api.Gateway.Proxies.Catalogos.CTServicios;
using Api.Gateway.Proxies.Limpieza.Convenios;
using Api.Gateway.Proxies.Limpieza.EntregablesContratacion;
using Api.Gateway.Proxies.Limpieza.Variables;
using Api.Gateway.Proxies.Mensajeria.Convenios.Commands;
using Api.Gateway.Proxies.Mensajeria.Convenios.Queries;
using Api.Gateway.Proxies.Mensajeria.EntregablesContrato.Commands;
using Api.Gateway.Proxies.Mensajeria.EntregablesContrato.Queries;
using Api.Gateway.Proxies.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Mensajeria.Convenios.Queries
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("mensajeria/convenios")]
    public class ConvenioController : ControllerBase
    {
        private readonly IUsuarioProxy _usuarios;
        private readonly IQConvenioMensajeriaProxy _convenios;
        private readonly IQEContratoMensajeriaProxy _entregables;
        private readonly ICTEntregableProxy _centregables;
        private readonly ICTParametroProxy _parametros;

        public ConvenioController(IUsuarioProxy usuarios, IQConvenioMensajeriaProxy convenios, IQEContratoMensajeriaProxy entregables, 
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
    }
}
