using Api.Gateway.Models.Repositorios.DTOs;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Mensajeria.CFDIs.Queries;
using Api.Gateway.Proxies.Mensajeria.Contratos.Queries;
using Api.Gateway.Proxies.Mensajeria.Repositorios.Queries;
using Api.Gateway.Proxies.Meses;
using Api.Gateway.Proxies.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Mensajeria.Repositorios.Queries
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("mensajeria/repositorios")]
    public class RepositorioController : ControllerBase
    {

        private readonly IQRepositorioMensajeriaProxy _repositorios;
        private readonly IQCFDIMensajeriaProxy _facturas;
        private readonly IQContratoMensajeriaProxy _contrato;
        private readonly IUsuarioProxy _usuarios;
        private readonly IInmuebleProxy _inmuebles;
        private readonly IMesProxy _mes;

        public RepositorioController(IQRepositorioMensajeriaProxy repositorios, IQCFDIMensajeriaProxy facturas, IQContratoMensajeriaProxy contrato, 
                                     IUsuarioProxy usuarios, IInmuebleProxy inmuebles, IMesProxy mes)
        {
            _repositorios = repositorios;
            _facturas = facturas;
            _contrato = contrato;
            _usuarios = usuarios;
            _inmuebles = inmuebles;
            _mes = mes;
        }

        [HttpGet("{anio}")]

        public async Task<List<RepositorioDto>> GetAllRepositorio(int anio)
        {
            var result = await _repositorios.GetAllRepositoriosAsync(anio);

            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    item.Contrato = await _contrato.GetContratoByIdAsync(item.ContratoId);
                    item.Mes = await _mes.GetMesByIdAsync(item.MesId);
                    item.Facturas = await _facturas.GetFacturasCargadasAsync(item.Id);
                    item.NotasCredito = await _facturas.GetNotasCreditoCargadasAsync(item.Id);
                }
            }

            return result;
        }

        [HttpGet("getRepositorioByAMC/{anio}/{mes}/{contrato}")]
        public async Task<RepositorioDto> GetRepositorioByAMC(int anio, int mes, int contrato)
        {
            var result = await _repositorios.GetRepositorioByAMC(anio, mes, contrato);

            result.Usuario = await _usuarios.GetUsuarioByIdAsync(result.UsuarioId);
            result.Contrato = await _contrato.GetContratoByIdAsync(result.ContratoId);
            result.Mes = await _mes.GetMesByIdAsync(result.MesId);

            return result;
        }
        
        [HttpGet("getRepositorioById/{repositorio}")]
        public async Task<RepositorioDto> GetRepositorioById(int repositorio)
        {
            var result = await _repositorios.GetRepositorioByIdAsync(repositorio);

            result.Usuario = await _usuarios.GetUsuarioByIdAsync(result.UsuarioId);
            result.Contrato = await _contrato.GetContratoByIdAsync(result.ContratoId);
            result.Mes = await _mes.GetMesByIdAsync(result.MesId);

            return result;
        }

    }
}
