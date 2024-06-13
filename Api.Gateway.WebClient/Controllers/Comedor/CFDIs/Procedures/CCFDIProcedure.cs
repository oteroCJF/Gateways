using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.Commands;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;
using Api.Gateway.Proxies.Comedor.CedulasEvaluacion.Commands;
using Api.Gateway.Proxies.Comedor.CedulasEvaluacion.Queries;
using Api.Gateway.Proxies.Comedor.CFDIs.Commands;
using Api.Gateway.Proxies.Comedor.CFDIs.Queries;
using Api.Gateway.Proxies.Comedor.Repositorios.Commands;
using Api.Gateway.Proxies.Comedor.Repositorios.Queries;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Comedor.CFDIs.Procedures
{
    public interface ICCFDIProcedure
    {
        HistorialMFCreateCommand GetObservacionesHCM(HistorialMFCreateCommand historial, CFDIDto factura);
        Task<CedulaEvaluacionDto> GetCedulaEvaluacion(int RepositorioId, int inmuebleId);
    }

    public class CCFDIProcedure : ICCFDIProcedure
    {
        private readonly IQCFDIComedorProxy _facturas;
        private readonly IQRepositorioComedorProxy _repositorio;
        private readonly IQCedulaComedorProxy _cedulas;

        public CCFDIProcedure(IQCFDIComedorProxy facturas, IQRepositorioComedorProxy repositorio, IQCedulaComedorProxy cedulas)
        {
            _facturas = facturas;
            _repositorio = repositorio;
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
            var repositorio = await _repositorio.GetRepositorioByIdAsync(RepositorioId);

            var cedulas = (await _cedulas.GetCedulaEvaluacionByAnio(repositorio.Anio)).Items.Single(c => c.InmuebleId == inmuebleId && c.MesId == repositorio.MesId);

            CedulaEvaluacionDto cedula = new CedulaEvaluacionDto
            {
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
