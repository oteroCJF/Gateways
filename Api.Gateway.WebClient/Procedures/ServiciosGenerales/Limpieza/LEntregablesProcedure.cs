using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas;
using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Cedulas;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;
using Api.Gateway.Proxies.Meses;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies;
using Ionic.Zip;
using Api.Gateway.Proxies.Fumigacion.CedulasEvaluacion;
using Api.Gateway.Proxies.Limpieza.Entregables;
using Api.Gateway.Proxies.Limpieza.CedulaEvaluacion;
using System.Text;
using System.Globalization;

namespace Api.Gateway.WebClient.Procedures.ServiciosGenerales.Limpieza
{
    public interface ILEntregablesProcedure
    {
        Task<string> DescargarEntregables(DEntregablesCommand request);
    }

    public class LEntregablesProcedure : ILEntregablesProcedure
    {
        private readonly ILCedulaProxy _cedulas;
        private readonly IMesProxy _meses;
        private readonly IInmuebleProxy _inmuebles;
        private readonly ILEntregableProxy _entregables;
        private readonly ICTEntregableProxy _ctentregables;

        public LEntregablesProcedure(ILCedulaProxy cedulas, ILEntregableProxy entregables, IMesProxy meses, ICTEntregableProxy ctentregables,
                                    IInmuebleProxy inmuebles)
        {
            _cedulas = cedulas;
            _entregables = entregables;
            _inmuebles = inmuebles;
            _ctentregables = ctentregables;
            _meses = meses;
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

                static string NormalizarNombre(string nombre)
                {
                    // Remover caracteres especiales que puedan causar problemas
                    var normalizedString = nombre.Normalize(NormalizationForm.FormD)
                                                .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                                                .ToArray();
                    return new string(normalizedString); 
                }

                foreach (var en in entregables)
                {
                    archivoO = request.Path;
                    archivoD = Directory.GetCurrentDirectory() + "\\Descargas";

                    var cedula = await _cedulas.GetCedulaById(en.CedulaEvaluacionId);
                    var mes = await _meses.GetMesByIdAsync(cedula.MesId);
                    var inmueble = await _inmuebles.GetInmuebleById(cedula.InmuebleId);
                    var entregable = await _ctentregables.GetEntregableById(en.EntregableId);

                    var nombreInmuebleNormalizado = NormalizarNombre(inmueble.Nombre); // Función para normalizar el nombre
                    var nombreEntregableNormalizado = NormalizarNombre(entregable.Nombre);

                    archivoO = archivoO + "\\" + cedula.Anio + "\\" + mes.Nombre + "\\" + "\\" + cedula.Folio + "\\" + entregable.Nombre + "\\" + en.Archivo;
                    archivoD = archivoD + "\\" + (i + "_Limpieza_" + fecha + "_" + nombreInmuebleNormalizado + "_" + mes.Nombre + "_" + nombreEntregableNormalizado) + ".pdf";


                    var file = new FileInfo(archivoO);
                    var fileD = new FileInfo(archivoD);

                    file.CopyTo(archivoD);

                    i++;
                }

                archivoD = Directory.GetCurrentDirectory() + "\\Descargas";

                string archivoZip = "Entregables_" + fecha+".zip";

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
                var entregables = await _entregables.GetAllEntregablesAsync();

                //si existen datos en el mes, filtramos por mes
                if (request.Meses.Count() != 0 && !request.Meses.Contains(0))
                {
                    cedulasId = cedulas.Where(c => request.Meses.Contains(c.MesId)).Select(s => s.Id).ToList();
                    entregables = entregables.Where(e => request.EntregablesId.Contains(e.EntregableId)
                                                                && cedulasId.Contains(e.CedulaEvaluacionId)).ToList();
                }
                else
                {
                    cedulasId = cedulas.Select(s => s.Id).ToList();
                    entregables = entregables.Where(e => request.EntregablesId.Contains(e.EntregableId)
                                                                && cedulasId.Contains(e.CedulaEvaluacionId)).ToList();
                }
                
                if (request.InmueblesId.Count() != 0 && !request.InmueblesId.Contains(0))
                {
                    cedulasId = cedulas.Where(c => request.InmueblesId.Contains(c.InmuebleId)).Select(s => s.Id).ToList();
                    entregables = entregables.Where(e => request.EntregablesId.Contains(e.EntregableId)
                                                                && cedulasId.Contains(e.CedulaEvaluacionId)).ToList();
                }
                else
                {
                    cedulasId = cedulas.Select(s => s.Id).ToList();
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
            catch(Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
    }
}
