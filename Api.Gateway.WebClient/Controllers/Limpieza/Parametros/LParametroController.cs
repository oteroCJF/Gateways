using Api.Gateway.Models.Parametrizacion.DTOs;
using Api.Gateway.Proxies.Limpieza.Variables;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Limpieza.Parametros
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("limpieza/variables")]
    public class LParametroController : ControllerBase
    {
        private readonly ILVariableProxy _variables;

        public LParametroController(ILVariableProxy variables)
        {
            _variables = variables;
        }

        [HttpGet]
        public async Task<List<ParametroDto>> GetAllVariables()
        {
            var variables = await _variables.GetAllVariables();
            return variables;
        }

        [Route("getVariableById/{variable}")]
        [HttpGet]
        public async Task<ParametroDto> GetVariableByIdAsync(int variable)
        {
            var variables = await _variables.GetVariableById(variable);

            return variables;
        }

        [Route("getVariablesTipoIncidencia")]
        [HttpGet]
        public async Task<List<ParametroDto>> GetVariablesTipoIncidencia()
        {
            var tiposIncidencia = await _variables.GetVariablesTipoIncidencia();

            return tiposIncidencia;
        }

        [Route("getIdByVariables/{abreviacion}/{valor}")]
        [HttpGet]
        public async Task<int> GetVariableById(string abreviacion, string valor)
        {
            var id = await _variables.GetVariableIdByTipoIncidencia(abreviacion, valor);

            return id;
        }
    }
}
