using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.Commands;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;
using Api.Gateway.Proxies.Fumigacion.CedulasEvaluacion;
using Api.Gateway.Proxies.Fumigacion.CFDIs;
using Api.Gateway.Proxies.Fumigacion.Facturacion;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Procedures.ServiciosGenerales.Fumigacion
{
    public interface IFCFDIProcedure
    {
        HistorialMFCreateCommand GetObservacionesHCM(HistorialMFCreateCommand historial, CFDIDto factura);
        Task<CedulaEvaluacionDto> GetCedulaEvaluacion(int RepositorioId, int inmuebleId);
    }

    public class FCFDIProcedure : IFCFDIProcedure
    {
        private readonly IFCFDIProxy _facturas;
        private readonly IFRepositorioProxy _facturacion;
        private readonly IFCedulaProxy _cedulas;

        public FCFDIProcedure(IFCFDIProxy facturas, IFRepositorioProxy facturacion, IFCedulaProxy cedulas)
        {
            _facturas = facturas;
            _facturacion = facturacion;
            _cedulas = cedulas;
        }

        public HistorialMFCreateCommand GetObservacionesHCM(HistorialMFCreateCommand historial, CFDIDto factura)
        {
            if (factura.EstatusId == 201)
            {
                historial.Observaciones = "El archivo se cargó correctamente.";
            }
            else if (factura.EstatusId == 205)
            {
                historial.Observaciones = "La factura ya fue previamente cargada.";
            }
            else if (factura.EstatusId == 206)
            {
                historial.Observaciones = "La factura adjuntada no corresponde al prestador del servicio.";
            }
            else
            {
                historial.Observaciones = "Se presentó un error al intentar adjuntar el archivo.";
            }

            return historial;
        }

        public async Task<CedulaEvaluacionDto> GetCedulaEvaluacion(int RepositorioId, int inmuebleId)
        {
            var facturacion = await _facturacion.GetFacturacionByIdAsync(RepositorioId);

            var cedulas = await _cedulas.GetCedulaEvaluacionByInmuebleAnioMes(inmuebleId, facturacion.Anio, facturacion.MesId);

            CedulaEvaluacionDto cedula = new CedulaEvaluacionDto { 
                Id = cedulas.Id,
                MesId = cedulas.MesId,
                Anio = cedulas.Anio,
                Folio = cedulas.Folio,
                InmuebleId = cedulas.InmuebleId,
            };

            return cedula;
        }
    }
}
