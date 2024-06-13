using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Limpieza.Entregables;
using Api.Gateway.Proxies.Limpieza.Entregables;
using Api.Gateway.Proxies.Mensajeria.Entregables;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Procedures.ServiciosGenerales.Limpieza
{
    public interface IVELimpiezaProcedure
    {
        Task<bool> VerificaCedulaLimpieza(int cedula);
        Task<bool> VerificaActaLimpieza(int cedula);
        Task<bool> VerificaMemorandumLimpieza(int cedula);
    }

    public class VELimpiezaProcedure : IVELimpiezaProcedure
    {
        private readonly ILEntregableProxy _entregables;
        private readonly ICTEntregableProxy _centregables;

        public VELimpiezaProcedure(ILEntregableProxy entregables, ICTEntregableProxy centregables)
        {
            _entregables = entregables;
            _centregables = centregables;
        }

        public async Task<bool> VerificaCedulaLimpieza(int cedula)
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

        public async Task<bool> VerificaActaLimpieza(int cedula)
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

        public async Task<bool> VerificaMemorandumLimpieza(int cedula)
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
