using Api.Gateway.Models.Repositorios.Commands;
using Api.Gateway.Models.Repositorios.DTOs;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Fumigacion.CFDIs;
using Api.Gateway.Proxies.Fumigacion.Contratos;
using Api.Gateway.Proxies.Fumigacion.Facturacion;
using Api.Gateway.Proxies.Meses;
using Api.Gateway.Proxies.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Fumigacion.Facturacion
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("fumigacion/repositorios")]
    public class FacturacionController : ControllerBase
    {

        private readonly IFRepositorioProxy _repositorios;
        private readonly IFCFDIProxy _facturas;
        private readonly IFContratoProxy _contrato;
        private readonly IUsuarioProxy _usuarios;
        private readonly IInmuebleProxy _inmuebles;
        private readonly IMesProxy _mes;

        public FacturacionController(IFRepositorioProxy repositorios, IFCFDIProxy facturas, IFContratoProxy contrato, 
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

        public async Task<List<RepositorioDto>> GetAllFacturacion(int anio)
        {
            var result = await _repositorios.GetAllFacturacionesAsync(anio);

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

        [HttpGet("getRepositorioById/{id}")]
        public async Task<RepositorioDto> GetFacturacionById(int id)
        {
            var result = await _repositorios.GetFacturacionByIdAsync(id);

            result.Usuario = await _usuarios.GetUsuarioByIdAsync(result.UsuarioId);
            result.Contrato = await _contrato.GetContratoByIdAsync(result.ContratoId);
            result.Mes = await _mes.GetMesByIdAsync(result.MesId);

            return result;
        }

        [Route("createRepositorio")]
        [HttpPost]
        public async Task<int> CreateFacturacion([FromBody] RepositorioCreateCommand repositorios)
        {
            return await _repositorios.CreateFacturacion(repositorios);
        }
    }
}
