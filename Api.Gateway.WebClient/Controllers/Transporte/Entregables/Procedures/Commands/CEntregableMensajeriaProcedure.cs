using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas;
using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Cedulas;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Gateway.Proxies;
using Api.Gateway.Proxies.Meses;
using Api.Gateway.Models;
using Api.Gateway.Models.Catalogos.DTOs.Entregables;
using Api.Gateway.Proxies.Transporte.CedulasEvaluacion;
using Api.Gateway.Proxies.Transporte.Entregables;
using System.IO;
using Ionic.Zip;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Api.Gateway.Models.Estatus.DTOs.EntregablesEstatus;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Create;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Update;
using Api.Gateway.Proxies.Transporte.Entregables.Commands;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Delete;
using Api.Gateway.WebClient.Controllers.Transporte.Entregables.Procedures.Queries;

namespace Api.Gateway.WebClient.Controllers.Transporte.Entregables.Procedures.Commands
{
    public interface ICEntregableTransporteProcedure
    {
        Task<List<EntregableEstatusDto>> InsertarEntregablesByEC(CedulaEvaluacionUpdateCommand request);
        Task<List<EntregableDto>> ActualizarEntregablesByEC(CedulaEvaluacionUpdateCommand request);
        Task<List<EntregableEstatusDto>> EliminarEntregablesByEC(CedulaEvaluacionUpdateCommand request);
    }

    public class CEntregableTransporteProcedure : ICEntregableTransporteProcedure
    {
        private readonly IMesProxy _meses;
        private readonly ICTEntregableProxy _ctentregables;
        private readonly ICEntregableTransporteProxy _entregablesCommand;
        private readonly IQEntregableTransporteProxy _entregablesQuery;
        private readonly IEstatusEntregableProxy _estatus;

        public CEntregableTransporteProcedure(IMesProxy meses, ICTEntregableProxy ctentregables, ICEntregableTransporteProxy entregablesCommand, 
                                              IQEntregableTransporteProxy entregablesQuery, IEstatusEntregableProxy estatus)
        {
            _meses = meses;
            _ctentregables = ctentregables;
            _entregablesCommand = entregablesCommand;
            _entregablesQuery = entregablesQuery;
            _estatus = estatus;
        }



        public async Task<List<EntregableEstatusDto>> InsertarEntregablesByEC(CedulaEvaluacionUpdateCommand request)
        {
            var entregablesActuales = (await _entregablesQuery.GetEntregablesByCedula(request.Id)).Select(e => e.EntregableId);

            List<EntregableEstatusDto> entregables = await _estatus.GetEntregablesByEstatus(request.ServicioId, request.EstatusId, request.Flujo);

            entregables = entregables.Where(e => !entregablesActuales.Contains(e.EntregableId) && !e.Eliminar).ToList();

            foreach (var e in entregables)
            {
                var entregable = (await _ctentregables.GetEntregableById(e.EntregableId)).Nombre;
                
                EntregableCreateCommand create = new EntregableCreateCommand
                {
                    CedulaEvaluacionId = request.Id,
                    EntregableId = e.EntregableId,
                    Entregable = entregable,
                };
                
                await _entregablesCommand.CreateEntregable(create);                
            }
            return entregables;
        }

        public async Task<List<EntregableDto>> ActualizarEntregablesByEC(CedulaEvaluacionUpdateCommand cedula)
        {
            var entregablesEstatus = (await _estatus.GetEntregablesByEstatus(cedula.ServicioId, cedula.EstatusId, cedula.Flujo)).Select(e => e.EntregableId).ToList();

            var entregables = await _entregablesQuery.GetEntregablesByCedula(cedula.Id);

            //entregables = entregables.Where(e => entregablesEstatus.Contains(e.EntregableId)).ToList();


            foreach (var e in entregables)
            {
                var entregable = await _entregablesQuery.GetEntregableById(e.Id);
                var estatus = await _estatus.GetEEByIdAsync(entregable.EstatusId);
                var nombreEstatus = (await _estatus.GetEEByIdAsync(entregable.EstatusId)).Nombre;
                var estatusEntregable = await _estatus.GetEEntregableByEC(cedula.EstatusId, entregable.EntregableId, cedula.Flujo);
                if (nombreEstatus.Equals("En Proceso") || nombreEstatus.Equals("En Revisión"))
                {
                    EntregableCommandUpdate update = new EntregableCommandUpdate
                    {
                        Id = e.Id,
                        UsuarioId = cedula.UsuarioId,
                        EstatusId = estatusEntregable.EEstatusId != 0 ? estatusEntregable.EEstatusId : entregable.EstatusId,
                        Observaciones = estatusEntregable.Observaciones
                    };

                    await _entregablesCommand.UpdateEntregable(update);
                }
            }

            return entregables;
        }

        public async Task<List<EntregableEstatusDto>> EliminarEntregablesByEC(CedulaEvaluacionUpdateCommand request)
        {
            var entregables = await _entregablesQuery.GetEntregablesByCedula(request.Id);

            List<EntregableEstatusDto> entregablesEstatus = await _estatus.GetEntregablesByEstatus(request.ServicioId, request.EstatusId, request.Flujo);

            //entregables = entregables.Where(e => entregablesActuales.Contains(e.EntregableId) && e.Eliminar).ToList();

            foreach (var e in entregables)
            {
                var entregable = await _entregablesQuery.GetEntregableById(e.Id);
                var nombreEntregable = await _ctentregables.GetEntregableById(e.EntregableId);
                var entregableEstatus = entregablesEstatus.SingleOrDefault(en => en.EntregableId == e.EntregableId && en.Flujo.Equals(request.Flujo));
                var estatusEntregable = await _estatus.GetEEntregableByEC(request.EstatusId, entregable.EntregableId, request.Flujo);

                if (entregableEstatus != null && entregableEstatus.Eliminar)
                {
                    EntregableDeleteCommand delete = new EntregableDeleteCommand
                    {
                        CedulaEvaluacionId = request.Id,
                        UsuarioId = request.UsuarioId,
                        EntregableId = e.EntregableId,
                        TipoEntregable = nombreEntregable.Nombre,
                        Mes = request.Mes,
                    };
                    await _entregablesCommand.DeleteEntregable(delete);
                }
                else if(entregableEstatus != null)
                {
                    EntregableCommandUpdate update = new EntregableCommandUpdate
                    {
                        Id = e.Id,
                        UsuarioId = e.UsuarioId,
                        EstatusId = estatusEntregable.EEstatusId != 0 ? estatusEntregable.EEstatusId : entregable.EstatusId,
                        Observaciones = estatusEntregable.Observaciones
                    };

                    await _entregablesCommand.UpdateEntregable(update);
                }
            }
            return entregablesEstatus;
        }
    }
}
