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
using Api.Gateway.Proxies.Mensajeria.CedulasEvaluacion;
using Api.Gateway.Proxies.Mensajeria.Entregables;
using System.IO;
using Ionic.Zip;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Api.Gateway.Models.Estatus.DTOs.EntregablesEstatus;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Create;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Update;
using System.Net.NetworkInformation;
using Api.Gateway.Proxies.Mensajeria.Entregables.Commands;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Delete;
using System.Text;
using System.Globalization;

namespace Api.Gateway.WebClient.Controllers.Mensajeria.Entregables.Procedures.Queries
{
    public interface IQMensajeriaEntregableProcedure
    {
        bool VerificaCedulaMensajeria(DataCollection<EntregableDto> entregables, List<CTEntregableDto> ctentregables, int cedula);
        bool VerificaActaMensajeria(DataCollection<EntregableDto> entregables, List<CTEntregableDto> ctentregables, int cedula);
        bool VerificaMemorandumMensajeria(DataCollection<EntregableDto> entregables, List<CTEntregableDto> ctentregables, int cedula);
        Task<string> DescargarEntregables(DEntregablesCommand request);
    }

    public class QMensajeriaEntregableProcedure : IQMensajeriaEntregableProcedure
    {
        private readonly IQCedulaMensajeriaProxy _cedulas;
        private readonly IMesProxy _meses;
        private readonly IInmuebleProxy _inmuebles;
        private readonly ICTEntregableProxy _ctentregables;
        private readonly IEstatusEntregableProxy _estatusEntregable;
        private readonly IQEntregableMensajeriaProxy _entregablesQuery;

        public QMensajeriaEntregableProcedure(IQCedulaMensajeriaProxy cedulas, IMesProxy meses, IInmuebleProxy inmuebles, ICTEntregableProxy ctentregables,
                                             IQEntregableMensajeriaProxy entregablesQuery, IEstatusEntregableProxy estatusEntregable)
        {
            _cedulas = cedulas;
            _meses = meses;
            _inmuebles = inmuebles;
            _ctentregables = ctentregables;
            _estatusEntregable = estatusEntregable;
            _entregablesQuery = entregablesQuery;
        }

        public bool VerificaCedulaMensajeria(DataCollection<EntregableDto> entregables, List<CTEntregableDto> ctentregables, int cedulaId)
        {
            var cedula = ctentregables.SingleOrDefault(ct => ct.Abreviacion.Equals("Cedula_Firmada")).Id;

            var validada = entregables.Items.SingleOrDefault(e => e.CedulaEvaluacionId == cedulaId && e.EntregableId == cedula && !e.FechaEliminacion.HasValue);

            return validada != null ? validada.Validado : false;
        }

        public bool VerificaActaMensajeria(DataCollection<EntregableDto> entregables, List<CTEntregableDto> ctentregables, int cedula)
        {
            var actaId = ctentregables.SingleOrDefault(ct => ct.Abreviacion.Equals("ActaERF")).Id;

            var acta = entregables.Items.SingleOrDefault(e => e.CedulaEvaluacionId == cedula && e.EntregableId == actaId && !e.FechaEliminacion.HasValue);

            return acta != null ? acta.Validado : false;
        }

        public bool VerificaMemorandumMensajeria(DataCollection<EntregableDto> entregables, List<CTEntregableDto> ctentregables, int cedula)
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

                List<EntregableDto> entregablesDescargar = new List<EntregableDto>();;

                string fecha = DateTime.Now.ToString("yyyy_MM_dd");

                if (!Directory.Exists(archivoD))
                {
                    Directory.CreateDirectory(archivoD);
                }

                if (request.EntregablesId.Contains(1))
                {
                    var entregablesActa = await GetActasERPendientes(request);
                    entregablesDescargar.AddRange(entregablesActa);
                }

                request.EntregablesId.RemoveAll(r => r == 1);


                var entregables = await GetEntregables(request);

                entregablesDescargar.AddRange(entregables);

                static string NormalizarNombre(string nombre)
                {
                    // Remover caracteres especiales que puedan causar problemas
                    var normalizedString = nombre.Normalize(NormalizationForm.FormD)
                                                .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                                                .ToArray();
                    return new string(normalizedString);
                }

                foreach (var en in entregablesDescargar)
                {
                    var cedula = await _cedulas.GetCedulaById(en.CedulaEvaluacionId);
                    var mes = await _meses.GetMesByIdAsync(cedula.MesId);
                    var inmueble = await _inmuebles.GetInmuebleById(cedula.InmuebleId);
                    var entregable = await _ctentregables.GetEntregableById(en.EntregableId);

                    if (inmueble.Id==4)
                    {
                        archivoO = request.Path;
                    }
                    archivoO = request.Path;
                    archivoD = Directory.GetCurrentDirectory() + "\\Descargas";

                    var nombreInmuebleNormalizado = NormalizarNombre(inmueble.Nombre); // Función para normalizar el nombre
                    var nombreEntregableNormalizado = NormalizarNombre(entregable.Nombre);

                    archivoO = archivoO + "\\" + cedula.Anio + "\\" + mes.Nombre + "\\" + "\\" + cedula.Folio + "\\" + entregable.Nombre + "\\" + en.Archivo;
                    archivoD = archivoD + "\\" + i + "_Mensajeria_" + fecha + "_" + nombreInmuebleNormalizado + "_" + mes.Nombre + "_" + nombreEntregableNormalizado + ".pdf";

                    var file = new FileInfo(archivoO);
                    var fileD = new FileInfo(archivoD);

                    if (File.Exists(archivoO))
                    {
                        file.CopyTo(archivoD);
                    }

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

                var estatus = (await _estatusEntregable.GetAllEstatusEntregablesAsync()).Single(e => e.Nombre.Equals("Autorizado")).Id;
                var cedulas = await _cedulas.GetCedulaEvaluacionByAnio(request.Anio);
                var entregables = (await _entregablesQuery.GetAllEntregablesAsync()).Where(e => e.EstatusId == estatus).ToList();



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

                return entregables;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
        
        public async Task<List<int>> GetCedulasConActaERFPendiente(DEntregablesCommand request)
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
                    entregables = entregables.Where(e => (e.Archivo.Equals("") || e.Archivo == null) && e.EntregableId == 19 && !e.Validado
                                                                && cedulasId.Contains(e.CedulaEvaluacionId)).ToList();
                }
                else
                {
                    cedulasId = cedulas.Items.Select(s => s.Id).ToList();
                    entregables = entregables.Where(e => (e.Archivo.Equals("") || e.Archivo == null) && e.EntregableId == 19 && !e.Validado
                                                                && cedulasId.Contains(e.CedulaEvaluacionId)).ToList();
                }

                if (request.InmueblesId.Count() != 0 && !request.InmueblesId.Contains(0))
                {
                    cedulasId = cedulas.Items.Where(c => request.InmueblesId.Contains(c.InmuebleId)).Select(s => s.Id).ToList();
                    entregables = entregables.Where(e => (e.Archivo.Equals("") || e.Archivo == null) && e.EntregableId == 19 && !e.Validado
                                                                && cedulasId.Contains(e.CedulaEvaluacionId)).ToList();
                }
                else
                {
                    cedulasId = cedulas.Items.Select(s => s.Id).ToList();
                    entregables = entregables.Where(e => (e.Archivo.Equals("") || e.Archivo == null) && e.EntregableId == 19 && !e.Validado
                                                                && cedulasId.Contains(e.CedulaEvaluacionId)).ToList();
                }

                return entregables.Select(e => e.CedulaEvaluacionId).ToList();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

        public async Task<List<EntregableDto>> GetActasERPendientes(DEntregablesCommand request)
        {
            try
            {
                List<int> cedulasId = new List<int>();

                var cedulas = await GetCedulasConActaERFPendiente(request);
                var entregables = await _entregablesQuery.GetAllEntregablesAsync();

                entregables = entregables.Where(e =>  e.EntregableId == 1 && cedulas.Contains(e.CedulaEvaluacionId)).ToList();
               

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
