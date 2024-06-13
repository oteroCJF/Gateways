using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas;
using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Cedulas;
using Api.Gateway.Models.Estatus.DTOs.EstatusEntregables;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Fumigacion.Entregables;
using Api.Gateway.Proxies.Mensajeria.Entregables;
using Api.Gateway.Proxies.Mensajeria.Entregables.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Procedures.ServiciosGenerales.Estatus
{
    public interface IEstatusProcedure
    {
        Task<int> EnviarCedula(int estatus);
        Task<List<EntregableDto>> MActualizaEntregablesByEC(CedulaEvaluacionUpdateCommand request);
        Task<List<EntregableDto>> FActualizaEntregablesByEC(CedulaEvaluacionUpdateCommand request);
        Task<int> CedulaSolicitudRechazo(int estatus);
    }

    public class EstatusProcedure : IEstatusProcedure
    {
        private readonly IEstatusCedulaProxy _estatus;
        private readonly IEstatusEntregableProxy _estatuse;
        private readonly IQEntregableMensajeriaProxy _entregablesm;
        private readonly ICEntregableMensajeriaProxy _entregablesc;
        private readonly IFEntregableProxy _entregablesf;

        public EstatusProcedure(IEstatusCedulaProxy estatus, IEstatusEntregableProxy estatuse, IQEntregableMensajeriaProxy entregablesm,
                                IFEntregableProxy entregablesf)
        {
            _estatus = estatus;
            _estatuse = estatuse;
            _entregablesm = entregablesm;
            _entregablesf = entregablesf;
        }

        public async Task<int> EnviarCedula(int estatus)
        {
            var Estatus = await _estatus.GetECByIdAsync(estatus);

            if (Estatus.Nombre.Equals("Enviado a DAS") || Estatus.Nombre.Equals("Revisión CAE") || Estatus.Nombre.Equals("En Trámite")
                || Estatus.Nombre.Equals("Bloqueada"))
            {
                estatus = (await _estatuse.GetAllEstatusEntregablesAsync()).SingleOrDefault(ee => ee.Nombre.Equals("En Revisión")).Id;
            }
            else if (Estatus.Nombre.Equals("Rechazada") || Estatus.Nombre.Equals("En Proceso"))
            {
                estatus = (await _estatuse.GetAllEstatusEntregablesAsync()).SingleOrDefault(ee => ee.Nombre.Equals("En Proceso")).Id;
            }
            else if (Estatus.Nombre.Equals("Trámite Rechazado"))
            {
                estatus = (await _estatuse.GetAllEstatusEntregablesAsync()).SingleOrDefault(ee => ee.Nombre.Equals("Rechazado")).Id;
            }
            else if (Estatus.Nombre.Equals("Autorizado CAE"))
            {
                estatus = (await _estatuse.GetAllEstatusEntregablesAsync()).SingleOrDefault(ee => ee.Nombre.Equals("Autorizado")).Id;
            }

            return estatus;
        }

        public async Task<List<EntregableDto>> MActualizaEntregablesByEC(CedulaEvaluacionUpdateCommand request)
        {
            var eAutorizado = (await _estatuse.GetAllEstatusEntregablesAsync()).SingleOrDefault(ee => ee.Nombre.Equals("Autorizado")).Id;

            List<EntregableDto> entregables = null;

            if (request.Elimina)
            {
                entregables = await _entregablesm.GetEntregablesByCedula(request.Id);
            }
            else
            {
                entregables = (await _entregablesm.GetEntregablesByCedula(request.Id)).Where(e => e.EstatusId != eAutorizado).ToList();
            }
            
            EEntregableUpdateCommand update = null;

            List<EEntregableCedulaDto> eByEstatus = await _estatuse.GetEEntregablesByEC(request.EstatusId, request.Flujo);

            var fechaActualizacion = DateTime.Now;

            foreach (var en in entregables)
            {
                EEntregableCedulaDto eeCedula = await _estatuse.GetEEntregableByEC(request.EstatusId, en.EntregableId, request.Flujo);

                if (eeCedula != null && eeCedula.EntregableId != 0)
                {
                    update = new EEntregableUpdateCommand();
                    update.Id = en.Id;
                    update.CedulaEvaluacionId = en.CedulaEvaluacionId;
                    update.UsuarioId = request.UsuarioId;
                    update.EstatusId = eeCedula.EEstatusId;
                    update.Observaciones = eeCedula.Observaciones;
                    update.FechaActualizacion = fechaActualizacion;

                    await _entregablesc.AUpdateEntregable(update);
                }
            }

            foreach (var en in entregables)
            {
                EEntregableCedulaDto eeCedula = await _estatuse.GetEEntregableByEC(request.EstatusId, en.EntregableId, request.Flujo);

                if (eeCedula.EntregableId == 0 && request.Elimina)
                {
                    update = new EEntregableUpdateCommand();
                    update.Id = en.Id;
                    update.CedulaEvaluacionId = en.CedulaEvaluacionId;
                    update.UsuarioId = en.UsuarioId;
                    update.EstatusId = en.EstatusId;
                    update.Observaciones = "Se elimina el entregabla por rechazo de la cédula.";
                    update.FechaActualizacion = fechaActualizacion;
                    update.FechaEliminacion = fechaActualizacion;

                    await _entregablesc.AUpdateEntregable(update);
                }
            }

            return entregables;
        }
        
        public async Task<List<EntregableDto>> FActualizaEntregablesByEC(CedulaEvaluacionUpdateCommand request)
        {
            var eAutorizado = (await _estatuse.GetAllEstatusEntregablesAsync()).SingleOrDefault(ee => ee.Nombre.Equals("Autorizado")).Id;

            List<EntregableDto> entregables = null;

            if (request.Elimina)
            {
                entregables = await _entregablesf.GetEntregablesByCedula(request.Id);
            }
            else
            {
                entregables = (await _entregablesf.GetEntregablesByCedula(request.Id)).Where(e => e.EstatusId != eAutorizado).ToList();
            }
            
            EEntregableUpdateCommand update = null;

            List<EEntregableCedulaDto> eByEstatus = await _estatuse.GetEEntregablesByEC(request.EstatusId, request.Flujo);

            var fechaActualizacion = DateTime.Now;

            foreach (var en in entregables)
            {
                EEntregableCedulaDto eeCedula = await _estatuse.GetEEntregableByEC(request.EstatusId, en.EntregableId, request.Flujo);

                if (eeCedula != null && eeCedula.EntregableId != 0)
                {
                    update = new EEntregableUpdateCommand();
                    update.Id = en.Id;
                    update.CedulaEvaluacionId = en.CedulaEvaluacionId;
                    update.UsuarioId = request.UsuarioId;
                    update.EstatusId = eeCedula.EEstatusId;
                    update.Observaciones = eeCedula.Observaciones;
                    update.FechaActualizacion = fechaActualizacion;

                    await _entregablesf.AUpdateEntregable(update);
                }
            }

            foreach (var en in entregables)
            {
                EEntregableCedulaDto eeCedula = await _estatuse.GetEEntregableByEC(request.EstatusId, en.EntregableId, request.Flujo);

                if (eeCedula.EntregableId == 0 && request.Elimina)
                {
                    update = new EEntregableUpdateCommand();
                    update.Id = en.Id;
                    update.CedulaEvaluacionId = en.CedulaEvaluacionId;
                    update.UsuarioId = en.UsuarioId;
                    update.EstatusId = en.EstatusId;
                    update.Observaciones = "Se elimina el entregabla por rechazo de la cédula.";
                    update.FechaActualizacion = fechaActualizacion;
                    update.FechaEliminacion = fechaActualizacion;

                    await _entregablesf.AUpdateEntregable(update);
                }
            }

            return entregables;
        }

        public async Task<int> CedulaSolicitudRechazo(int estatus)
        {
            var Estatus = await _estatuse.GetEEByIdAsync(estatus);

            if (Estatus.Nombre.Equals("En Revisión") || Estatus.Nombre.Equals("Autorizado"))
            {
                estatus = (await _estatuse.GetAllEstatusEntregablesAsync()).SingleOrDefault(ee => ee.Nombre.Equals("En Proceso")).Id;
            }

            return estatus;
        }


    }
}
