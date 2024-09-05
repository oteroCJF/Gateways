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
using Api.Gateway.Proxies.Agua.CedulasEvaluacion;
using Api.Gateway.Proxies.Agua.Entregables;
using System.IO;
using Ionic.Zip;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion;
using Api.Gateway.Models.Estatus.DTOs.EntregablesEstatus;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Create;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Update;
using System.Net.NetworkInformation;
using Api.Gateway.Proxies.Agua.Entregables.Commands;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Delete;

namespace Api.Gateway.WebClient.Controllers.Agua.Entregables.Procedures.Queries
{
    public interface IQAguaEntregableProcedure
    {
        bool VerificaCedulaAgua(DataCollection<EntregableDto> entregables, List<CTEntregableDto> ctentregables, int cedula);
        bool VerificaActaAgua(DataCollection<EntregableDto> entregables, List<CTEntregableDto> ctentregables, int cedula);
        bool VerificaMemorandumAgua(DataCollection<EntregableDto> entregables, List<CTEntregableDto> ctentregables, int cedula);
        Task<string> DescargarEntregables(DEntregablesCommand request);
    }

    public class QAguaEntregableProcedure : IQAguaEntregableProcedure
    {
        private readonly IQCedulaAguaProxy _cedulas;
        private readonly IMesProxy _meses;
        private readonly IInmuebleProxy _inmuebles;
        private readonly ICTEntregableProxy _ctentregables;
        private readonly IQEntregableAguaProxy _entregablesQuery;

        public QAguaEntregableProcedure(IQCedulaAguaProxy cedulas, IMesProxy meses, IInmuebleProxy inmuebles, ICTEntregableProxy ctentregables,
                                             IQEntregableAguaProxy entregablesQuery)
        {
            _cedulas = cedulas;
            _meses = meses;
            _inmuebles = inmuebles;
            _ctentregables = ctentregables;
            _entregablesQuery = entregablesQuery;
        }

        public bool VerificaCedulaAgua(DataCollection<EntregableDto> entregables, List<CTEntregableDto> ctentregables, int cedulaId)
        {
            var cedula = ctentregables.SingleOrDefault(ct => ct.Abreviacion.Equals("Cedula_Firmada")).Id;

            var validada = entregables.Items.SingleOrDefault(e => e.CedulaEvaluacionId == cedulaId && e.EntregableId == cedula && !e.FechaEliminacion.HasValue);

            return validada != null ? validada.Validado : false;
        }

        //Metodo que valida si el Acta Entrega del AGUA está o no validada por el usuario
        public bool VerificaActaAgua(DataCollection<EntregableDto> entregables, List<CTEntregableDto> ctentregables, int cedula)
        {
            // Recupera el Id que corresponde al entregable "ActaER" de la BD CatalogoServicios en la tabla CTEntregables:
            var actaId = ctentregables.SingleOrDefault(ct => ct.Abreviacion.Equals("ActaER")).Id;

            //Va y busca en la tabla entregables el id correspondiente de la cedula y el entregable. 
            var acta = entregables.Items.SingleOrDefault(e => e.CedulaEvaluacionId == cedula && e.EntregableId == actaId && !e.FechaEliminacion.HasValue);

            //Regresa un true si encuentra el Acta y si el atributo validado es true, false en caso contrario y regresa a CedulaQueryController
            return acta != null ? acta.Validado : false;
        }

        //Metodo que valida si el Memorandum del agua está o no validado por el usuario
        public bool VerificaMemorandumAgua(DataCollection<EntregableDto> entregables, List<CTEntregableDto> ctentregables, int cedula)
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
                    archivoD = archivoD + "\\" + i + "_Agua_" + fecha + "_" + inmueble.Nombre + "_" + mes.Nombre + "_" + entregable.Nombre + ".pdf";

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
