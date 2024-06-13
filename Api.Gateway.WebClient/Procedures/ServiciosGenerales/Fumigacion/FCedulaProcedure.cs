using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Fumigacion;
using Api.Gateway.Proxies.Catalogos.CTIndemnizacion;
using Api.Gateway.Proxies.Catalogos.CTParametros;
using Api.Gateway.Proxies.Catalogos.CTServiciosContratos;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Fumigacion.ServicioContrato;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Procedures.ServiciosGenerales.Fumigacion
{
    public interface IFCedulaProcedure
    {
        Task<EnviarCedulaEvaluacionUpdateCommand> EnviarCedulaEvaluacion(CedulaEvaluacionUpdateCommand request, CedulaFumigacionDto cedula);
        Task<DBloquearCedulaUpdateCommand> DBloquearCedulaEvaluacion(CedulaEvaluacionUpdateCommand request, CedulaFumigacionDto cedula);
    }

    public class FCedulaProcedure : IFCedulaProcedure
    {
        private readonly ICTServicioContratoProxy _cscontratos;
        private readonly IFServicioContratoProxy _scontratos;
        private readonly IEstatusCedulaProxy _estatusc;
        private readonly ICTParametroProxy _parametros;
        private readonly ICTIndemnizacionProxy _indemnizacion;

        public FCedulaProcedure(ICTServicioContratoProxy cscontratos, IFServicioContratoProxy scontratos, IEstatusCedulaProxy estatusc, 
                                ICTParametroProxy parametros, ICTIndemnizacionProxy indemnizacion)
        {
            _cscontratos = cscontratos;
            _scontratos = scontratos;
            _estatusc = estatusc;
            _parametros = parametros;
            _indemnizacion = indemnizacion;
        }

        public async Task<DBloquearCedulaUpdateCommand> DBloquearCedulaEvaluacion(CedulaEvaluacionUpdateCommand request, CedulaFumigacionDto cedula)
        {
            DBloquearCedulaUpdateCommand dbloquear = new DBloquearCedulaUpdateCommand();
            dbloquear.Id = request.Id;
            dbloquear.UsuarioId = request.UsuarioId;
            dbloquear.EstatusId = (request.Bloqueada ? (await _estatusc.GetAllEstatusCedulaAsync()).Single(e => e.Nombre.Equals("Bloqueada")).Id:request.EstatusId);
            dbloquear.RepositorioId = request.RepositorioId;
            dbloquear.EFacturaId = request.EFacturaId;
            dbloquear.Bloqueada = request.Bloqueada;
            dbloquear.Observaciones = "Se "+(request.Bloqueada ? "bloquea":"desbloquea")+" la cédula de evaluación ya que cuenta con guías pendientes de atender por parte del prestador de servicios.";
            dbloquear.FechaActualizacion = DateTime.Now;

            return dbloquear;
        }

        public async Task<EnviarCedulaEvaluacionUpdateCommand> EnviarCedulaEvaluacion(CedulaEvaluacionUpdateCommand request, CedulaFumigacionDto cedula)
        {
            EnviarCedulaEvaluacionUpdateCommand enviar = new EnviarCedulaEvaluacionUpdateCommand();
            enviar.Id = request.Id;
            enviar.UsuarioId = request.UsuarioId;
            enviar.EstatusId = request.EstatusId;
            enviar.RepositorioId = request.RepositorioId;
            enviar.EFacturaId = request.EFacturaId;
            enviar.Calcula = request.Calcula;
            enviar.Estatus = (await _estatusc.GetECByIdAsync(request.EstatusId)).Nombre;
            enviar.Observaciones =  request.Observaciones;
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
