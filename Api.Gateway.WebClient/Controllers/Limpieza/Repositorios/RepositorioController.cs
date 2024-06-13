using Api.Gateway.Models.Repositorios.Commands;
using Api.Gateway.Models.Repositorios.DTOs;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Limpieza.CedulaEvaluacion;
using Api.Gateway.Proxies.Limpieza.Contratos;
using Api.Gateway.Proxies.Limpieza.Facturas;
using Api.Gateway.Proxies.Limpieza.Repositorios;
using Api.Gateway.Proxies.Meses;
using Api.Gateway.Proxies.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Limpieza.Repositorios
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("limpieza/repositorios")]
    public class RepositorioController : ControllerBase
    {
        private readonly ILCedulaProxy _limpieza;
        private readonly ILRepositorioProxy _repositorios;
        private readonly ILCFDIProxy _facturas;
        private readonly ILContratoProxy _contrato;
        private readonly IUsuarioProxy _usuarios;
        private readonly IInmuebleProxy _inmuebles;
        private readonly IMesProxy _mes;

        public RepositorioController(ILCedulaProxy limpieza, ILRepositorioProxy repositorios, ILCFDIProxy facturas,
                                     ILContratoProxy contrato, IUsuarioProxy usuarios, IInmuebleProxy inmuebles, IMesProxy mes)
        {
            _limpieza = limpieza;
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

        [HttpGet("getRepositorioById/{id}")]
        public async Task<RepositorioDto> GetFacturacionById(int id)
        {
            var result = await _repositorios.GetRepositorioByIdAsync(id);

            result.Usuario = await _usuarios.GetUsuarioByIdAsync(result.UsuarioId);
            result.Contrato = await _contrato.GetContratoByIdAsync(result.ContratoId);
            result.Mes = await _mes.GetMesByIdAsync(result.MesId);

            return result;
        }

        [Route("createRepositorio")]
        [HttpPost]
        public async Task<int> CreateRepositorio([FromBody] RepositorioCreateCommand request)
        {
            return await _repositorios.CreateRepositorio(request);
        }
    }
}
