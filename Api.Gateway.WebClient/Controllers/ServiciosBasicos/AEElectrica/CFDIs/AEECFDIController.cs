using Api.Gateway.Models.CFDIs.ServiciosBasicos.Commands;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.ServiciosBasicos.AEElectrica.CFDIs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.ServiciosBasicos.AEElectrica.CFDIs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("aeelectrica/cfdi")]
    public class AEECFDIController :ControllerBase
    {
        private readonly IAEECFDIProxy _cfdi;
        private readonly ICTEntregableProxy _ctentregable;

        public AEECFDIController(IAEECFDIProxy cfdi, ICTEntregableProxy ctentregable)
        {
            _cfdi = cfdi;
            _ctentregable= ctentregable;

        }

        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [HttpPost("createCFDI")]
        public async Task<IActionResult> Create([FromForm] CFDISBCreateCommand factura)
        {
            factura.EntregableId = (await _ctentregable.GetAllCTEntregables()).SingleOrDefault(e => e.Nombre.Equals("Factura")).Id;
            int status = await _cfdi.CreateFactura(factura);
            return Ok(status);
        }

        [HttpPut]
        [Route("deleteCFDI")]
        public async Task<IActionResult> Delete([FromBody] CFDISBDeleteCommand factura)
        {
            int status = await _cfdi.DeleteFactura(factura);
            return Ok(status);
        }
    }
}
