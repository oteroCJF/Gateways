using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.ServiciosBasicos.AEElectrica.SolicitudesPagos;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Procedures.ServiciosBasicos.AEElectrica
{
    public interface IAEElectricaProcedure
    {
        Task<int> GetEstatusEntregable(int solicitud);
        Task<int> EnviarSolicitudP(int estatus);
    }

    public class AEElectricaProcedure : IAEElectricaProcedure
    {
        private readonly IAEESolicitudPagoProxy _solicitudes;
        private readonly IEstatusSPProxy _estatus;
        private readonly IEstatusEntregableProxy _estatuse;

        public AEElectricaProcedure(IAEESolicitudPagoProxy solicitudes, IEstatusSPProxy estatus, IEstatusEntregableProxy estatuse)
        {
            _solicitudes = solicitudes;
            _estatus = estatus;
            _estatuse = estatuse;
        }

        public async Task<int> GetEstatusEntregable(int solicitud)
        {
            var sPago = await _solicitudes.GetSolicitudPagoById(solicitud);
            var estatus = 0;
            sPago.Estatus = await _estatus.GetESPagoByIdAsync(sPago.EstatusId);

            if (sPago.Estatus.Nombre.Equals("En Proceso") || sPago.Estatus.Nombre.Equals("Rechazada"))
            {
                estatus = (await _estatuse.GetAllEstatusEntregablesAsync()).SingleOrDefault(ee => ee.Nombre.Equals("En Proceso")).Id;
            }

            return estatus;
        }

        public async Task<int> EnviarSolicitudP(int estatus)
        {
            var Estatus = await _estatus.GetESPagoByIdAsync(estatus);

            if (Estatus.Nombre.Equals("Enviada"))
            {
                estatus = (await _estatuse.GetAllEstatusEntregablesAsync()).SingleOrDefault(ee => ee.Nombre.Equals("En Revisión")).Id;
            }

            return estatus;
        }
    }
}
