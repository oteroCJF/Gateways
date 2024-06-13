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
using System.Net.NetworkInformation;
using Api.Gateway.Proxies.Transporte.Entregables.Commands;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Delete;

namespace Api.Gateway.WebClient.Controllers.Transporte.Entregables.Procedures.Queries
{
    public interface IQTransporteEntregableProcedure
    {
        bool VerificaCedulaTransporte(DataCollection<EntregableDto> entregables, List<CTEntregableDto> ctentregables, int cedula);
        bool VerificaActaTransporte(DataCollection<EntregableDto> entregables, List<CTEntregableDto> ctentregables, int cedula);
        bool VerificaMemorandumTransporte(DataCollection<EntregableDto> entregables, List<CTEntregableDto> ctentregables, int cedula);
        Task<string> DescargarEntregables(DEntregablesCommand request);
    }

    public class QTransporteEntregableProcedure : IQTransporteEntregableProcedure
    {
        private readonly IQCedulaTransporteProxy _cedulas;
        private readonly IMesProxy _meses;
        private readonly IInmuebleProxy _inmuebles;
        private readonly ICTEntregableProxy _ctentregables;
        private readonly IQEntregableTransporteProxy _entregablesQuery;

        public QTransporteEntregableProcedure(IQCedulaTransporteProxy cedulas, IMesProxy meses, IInmuebleProxy inmuebles, ICTEntregableProxy ctentregables,
                                             IQEntregableTransporteProxy entregablesQuery)
        {
            _cedulas = cedulas;
            _meses = meses;
            _inmuebles = inmuebles;
            _ctentregables = ctentregables;
            _entregablesQuery = entregablesQuery;
        }

        public bool VerificaCedulaTransporte(DataCollection<EntregableDto> entregables, List<CTEntregableDto> ctentregables, int cedulaId)
        {
            var cedula = ctentregables.SingleOrDefault(ct => ct.Abreviacion.Equals("Cedula_Firmada")).Id;

            var validada = entregables.Items.SingleOrDefault(e => e.CedulaEvaluacionId == cedulaId && e.EntregableId == cedula && !e.FechaEliminacion.HasValue);

            return validada != null ? validada.Validado : false;
        }

        public bool VerificaActaTransporte(DataCollection<EntregableDto> entregables, List<CTEntregableDto> ctentregables, int cedula)
        {
            var actaId = ctentregables.SingleOrDefault(ct => ct.Abreviacion.Equals("ActaERF")).Id;

            var acta = entregables.Items.SingleOrDefault(e => e.CedulaEvaluacionId == cedula && e.EntregableId == actaId && !e.FechaEliminacion.HasValue);

            return acta != null ? acta.Validado : false;
        }

        public bool VerificaMemorandumTransporte(DataCollection<EntregableDto> entregables, List<CTEntregableDto> ctentregables, int cedula)
        {
            var memorandumId = ctentregables.SingleOrDefault(ct => ct.Abreviacion.Equals("Memorandum")).Id;

            var memorandum = entregables.Items.SingleOrDefault(e => e.CedulaEvaluacionId == cedula && e.EntregableId == memorandumId && !e.FechaEliminacion.HasValue);

            return memorandum != null ? memorandum.Validado : false;
        }

        public async Task<string> DescargarEntregables(DEntregablesCommand request)
        {
            try
            {
                int i = 1;
                string archivoO = request.Path;
                string archivoD = Directory.GetCurrentDirectory() + "\\Descargas";

                string fecha = DateTime.Now.ToString("yyyy_MM_dd");

                if (!Directory.Exists(archivoD))
                {
                    Directory.CreateDirectory(archivoD);
                }

                var entregables = await GetEntregables(request);

                foreach (var en in entregables)
                {
                    archivoO = request.Path;
                    archivoD = Directory.GetCurrentDirectory() + "\\Descargas";

                    var cedula = await _cedulas.GetCedulaById(en.CedulaEvaluacionId);
                    var mes = await _meses.GetMesByIdAsync(cedula.MesId);
                    var inmueble = await _inmuebles.GetInmuebleById(cedula.InmuebleId);
                    var entregable = await _ctentregables.GetEntregableById(en.EntregableId);

                    archivoO = archivoO + "\\" + cedula.Anio + "\\" + mes.Nombre + "\\" + "\\" + cedula.Folio + "\\" + entregable.Nombre + "\\" + en.Archivo;
                    archivoD = archivoD + "\\" + i + "_Transporte_" + fecha + "_" + inmueble.Nombre + "_" + mes.Nombre + "_" + entregable.Nombre + ".pdf";

                    var file = new FileInfo(archivoO);
                    var fileD = new FileInfo(archivoD);

                    file.CopyTo(archivoD);

                    i++;
                }

                archivoD = Directory.GetCurrentDirectory() + "\\Descargas";

                string archivoZip = "Entregables_" + fecha + ".zip";

                using (ZipFile zipFile = new ZipFile())
                {
                    zipFile.AddDirectory(archivoD);
                    zipFile.Save(archivoZip);
                }

                Directory.Delete(archivoD, true);

                return Directory.GetCurrentDirectory() + "\\" + archivoZip;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return "";
        }

        public async Task<List<EntregableDto>> GetEntregables(DEntregablesCommand request)
        {
            try
            {
                List<int> cedulasId = new List<int>();

                var cedulas = await _cedulas.GetCedulaEvaluacionByAnio(request.Anio);
                var entregables = await _entregablesQuery.GetAllEntregablesAsync();

                //si existen datos en el mes, filtramos por mes
                if (request.Meses.Count() != 0 && !request.Meses.Contains(0))
                {
                    cedulasId = cedulas.Items.Where(c => request.Meses.Contains(c.MesId)).Select(s => s.Id).ToList();
                    entregables = entregables.Where(e => request.EntregablesId.Contains(e.EntregableId)
                                                                && cedulasId.Contains(e.CedulaEvaluacionId)).ToList();
                }
                else
                {
                    cedulasId = cedulas.Items.Select(s => s.Id).ToList();
                    entregables = entregables.Where(e => request.EntregablesId.Contains(e.EntregableId)
                                                                && cedulasId.Contains(e.CedulaEvaluacionId)).ToList();
                }

                if (request.InmueblesId.Count() != 0 && !request.InmueblesId.Contains(0))
                {
                    cedulasId = cedulas.Items.Where(c => request.InmueblesId.Contains(c.InmuebleId)).Select(s => s.Id).ToList();
                    entregables = entregables.Where(e => request.EntregablesId.Contains(e.EntregableId)
                                                                && cedulasId.Contains(e.CedulaEvaluacionId)).ToList();
                }
                else
                {
                    cedulasId = cedulas.Items.Select(s => s.Id).ToList();
                    entregables = entregables.Where(e => request.EntregablesId.Contains(e.EntregableId)
                                                                && cedulasId.Contains(e.CedulaEvaluacionId)).ToList();
                }

                //si existen datos en los estatus, filtramos por estatus
                if (request.Estatus.Count != 0 && !request.Estatus.Contains(0))
                {
                    entregables = entregables.Where(e => request.Estatus.Contains(e.EstatusId)).ToList();
                }
                else
                {
                    entregables = entregables.ToList();
                }

                return entregables;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
    }
}
