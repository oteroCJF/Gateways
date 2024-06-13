using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Comedor;
using Api.Gateway.Proxies.Catalogos.CTIndemnizacion;
using Api.Gateway.Proxies.Catalogos.CTParametros;
using Api.Gateway.Proxies.Catalogos.CTServiciosContratos;
using Api.Gateway.Proxies.Comedor.ServiciosContrato.Commands;
using Api.Gateway.Proxies.Comedor.ServiciosContrato.Queries;
using Api.Gateway.Proxies.Estatus;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Controllers.Comedor.CedulasEvaluacion.Procedures
{

    public interface ICedulaComedorProcedure
    {
        Task<EnviarCedulaEvaluacionUpdateCommand> EnviarCedulaEvaluacion(CedulaEvaluacionUpdateCommand request, CedulaComedorDto cedula);
    }

    public class CedulaComedorProcedure : ICedulaComedorProcedure
    {
        private readonly ICTServicioContratoProxy _cscontratos;
        private readonly IQSContratoComedorProxy _scontratos;
        private readonly IEstatusCedulaProxy _estatusc;
        private readonly ICTParametroProxy _parametros;
        private readonly ICTIndemnizacionProxy _indemnizacion;

        public CedulaComedorProcedure(ICTServicioContratoProxy cscontratos, IQSContratoComedorProxy scontratos, IEstatusCedulaProxy estatusc, 
                                      ICTParametroProxy parametros, ICTIndemnizacionProxy indemnizacion)
        {
            _cscontratos = cscontratos;
            _scontratos = scontratos;
            _estatusc = estatusc;
            _parametros = parametros;
            _indemnizacion = indemnizacion;
        }

        public async Task<EnviarCedulaEvaluacionUpdateCommand> EnviarCedulaEvaluacion(CedulaEvaluacionUpdateCommand request, CedulaComedorDto cedula)
        {
            EnviarCedulaEvaluacionUpdateCommand enviar = new EnviarCedulaEvaluacionUpdateCommand();
            enviar.Id = request.Id;
            enviar.UsuarioId = request.UsuarioId;
            enviar.EstatusId = request.EstatusId;
            enviar.RepositorioId = request.RepositorioId;
            enviar.EFacturaId = request.EFacturaId;
            enviar.Calcula = request.Calcula;
            enviar.Estatus = (await _estatusc.GetECByIdAsync(request.EstatusId)).Nombre;
            enviar.Observaciones = request.Observaciones;
            enviar.Penalizacion = await _scontratos.GetServiciosByContrato(cedula.ContratoId);
            enviar.Indemnizaciones = await _indemnizacion.GetAllIndemnizacionesAsync();
            enviar.FechaActualizacion = DateTime.Now;
            foreach (var sc in enviar.Penalizacion)
            {
                sc.Servicio = await _cscontratos.GetServicioContratoByIdAsync(sc.ServicioId);
            }

            return enviar;
        }
    }
}
