using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Agua;
using Api.Gateway.Proxies.Catalogos.CTIndemnizacion;
using Api.Gateway.Proxies.Catalogos.CTParametros;
using Api.Gateway.Proxies.Catalogos.CTServiciosContratos;
using Api.Gateway.Proxies.Agua.ServiciosContrato.Queries;
using Api.Gateway.Proxies.Estatus;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Cedulas;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas;
using System.Net.NetworkInformation;
using Api.Gateway.Proxies.Agua.Entregables;
using Api.Gateway.Proxies.Agua.Entregables.Commands;

namespace Api.Gateway.WebClient.Controllers.Agua.CedulasEvaluacion.Procedures
{

    public interface ICedulaAguaProcedure
    {
        Task<EnviarCedulaEvaluacionUpdateCommand> EnviarCedulaEvaluacion(CedulaEvaluacionUpdateCommand request, CedulaAguaDto cedula);
        Task<DBloquearCedulaUpdateCommand> DBloquearCedulaEvaluacion(CedulaEvaluacionUpdateCommand request, CedulaAguaDto cedula);
    }

    public class CedulaAguaProcedure : ICedulaAguaProcedure
    {
        private readonly ICTServicioContratoProxy _cscontratos;
        private readonly IQSContratoAguaProxy _scontratos;
        private readonly IEstatusCedulaProxy _estatusc;
        private readonly ICTIndemnizacionProxy _indemnizacion;

        public CedulaAguaProcedure(ICTServicioContratoProxy cscontratos, IQSContratoAguaProxy scontratos, 
                                         IEstatusCedulaProxy estatusc, ICTIndemnizacionProxy indemnizacion)
        {
            _cscontratos = cscontratos;
            _scontratos = scontratos;
            _estatusc = estatusc;
            _indemnizacion = indemnizacion;
        }

        public async Task<DBloquearCedulaUpdateCommand> DBloquearCedulaEvaluacion(CedulaEvaluacionUpdateCommand request, CedulaAguaDto cedula)
        {
            DBloquearCedulaUpdateCommand dbloquear = new DBloquearCedulaUpdateCommand();
            dbloquear.Id = request.Id;
            dbloquear.UsuarioId = request.UsuarioId;
            dbloquear.EstatusId = (request.Bloqueada ? (await _estatusc.GetAllEstatusCedulaAsync()).Single(e => e.Nombre.Equals("Bloqueada")).Id : request.EstatusId);
            dbloquear.RepositorioId = request.RepositorioId;
            dbloquear.EFacturaId = request.EFacturaId;
            dbloquear.Bloqueada = request.Bloqueada;
            dbloquear.Observaciones = "Se " + (request.Bloqueada ? "bloquea" : "desbloquea") + " la cédula de evaluación ya que cuenta con guías pendientes de atender por parte del prestador de servicios.";
            dbloquear.FechaActualizacion = DateTime.Now;

            return dbloquear;
        }

        public async Task<EnviarCedulaEvaluacionUpdateCommand> EnviarCedulaEvaluacion(CedulaEvaluacionUpdateCommand request, CedulaAguaDto cedula)
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
