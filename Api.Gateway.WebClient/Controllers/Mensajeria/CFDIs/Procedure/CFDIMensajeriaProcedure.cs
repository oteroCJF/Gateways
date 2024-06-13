using Api.Gateway.Models.CFDIs.ServiciosGenerales.Commands;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;

namespace Api.Gateway.WebClient.Controllers.Mensajeria.CFDIs.Procedure
{
    public interface ICFDIMensajeriaProcedure
    {
        HistorialMFCreateCommand GetObservacionesHCM(HistorialMFCreateCommand historial, CFDIDto factura);
    }

    public class CFDIMensajeriaProcedure : ICFDIMensajeriaProcedure
    {
        public CFDIMensajeriaProcedure()
        {
        }

        public HistorialMFCreateCommand GetObservacionesHCM(HistorialMFCreateCommand historial, CFDIDto factura)
        {
            if (factura.EstatusId == 201)
            {
                historial.Observaciones = "El archivo se cargó correctamente.";
            }
            else if (factura.EstatusId == 205)
            {
                historial.Observaciones = "La factura ya fue previamente cargada.";
            }
            else if (factura.EstatusId == 206)
            {
                historial.Observaciones = "La factura adjuntada no corresponde al prestador del servicio.";
            }
            else
            {
                historial.Observaciones = "Se presentó un error al intentar adjuntar el archivo.";
            }

            return historial;
        }
    }
}
