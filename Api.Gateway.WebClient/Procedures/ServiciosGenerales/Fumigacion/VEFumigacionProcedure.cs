using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Fumigacion.Entregables;
using Api.Gateway.Proxies.Limpieza.Entregables;
using Api.Gateway.Proxies.Mensajeria.Entregables;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Procedures.ServiciosGenerales.Fumigacion
{
    public interface IVEFumigacionProcedure
    {
        Task<bool> VerificaCedulaFumigacion(int cedula);
        Task<bool> VerificaActaFumigacion(int cedula);
        Task<bool> VerificaMemorandumFumigacion(int cedula);
    }

    public class VEFumigacionProcedure : IVEFumigacionProcedure
    {
        private readonly IFEntregableProxy _entregables;
        private readonly ICTEntregableProxy _centregables;

        public VEFumigacionProcedure(IFEntregableProxy entregables, ICTEntregableProxy centregables)
        {
            _entregables = entregables;
            _centregables = centregables;
        }

        public async Task<bool> VerificaCedulaFumigacion(int cedula)
        {
            var catalogoE = (await _centregables.GetAllCTEntregables()).Single(en => en.Abreviacion.Equals("Cedula_Firmada")).Id;
            var entregables = (await _entregables.GetEntregablesByCedula(cedula)).Where(e => e.EntregableId == catalogoE).ToList();

            var validado = entregables.Count() != 0 ? true : false;

            foreach (var en in entregables)
            {
                if (en.Validado == false || en.Validado == null)
                {
                    validado = false;
                }
            }

            return validado;
        }

        public async Task<bool> VerificaActaFumigacion(int cedula)
        {
            var catalogoE = (await _centregables.GetAllCTEntregables()).Single(en => en.Abreviacion.Equals("ActaER")).Id;
            var entregables = (await _entregables.GetEntregablesByCedula(cedula)).Where(e => e.EntregableId == catalogoE).ToList();

            var validado = entregables.Count() != 0 ? true : false;

            foreach (var en in entregables)
            {
                if (en.Validado == false || en.Validado == null)
                {
                    validado = false;
                }
            }

            return validado;
        }

        public async Task<bool> VerificaMemorandumFumigacion(int cedula)
        {
            var catalogoE = (await _centregables.GetAllCTEntregables()).Single(en => en.Abreviacion.Equals("Memorandum")).Id;
            var entregables = (await _entregables.GetEntregablesByCedula(cedula)).Where(e => e.EntregableId == catalogoE).ToList();

            var validado = entregables.Count() != 0 ? true : false;

            foreach (var en in entregables)
            {
                if (en.Validado == false || en.Validado == null)
                {
                    validado = false;
                }
            }

            return validado;
        }
    }
}
