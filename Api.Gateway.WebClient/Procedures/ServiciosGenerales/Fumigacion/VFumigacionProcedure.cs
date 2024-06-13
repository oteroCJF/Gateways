using Api.Gateway.Proxies.Fumigacion.CedulaEvaluacion;
using Api.Gateway.Proxies.Fumigacion.CedulasEvaluacion;
using Api.Gateway.Proxies.Fumigacion.Cuestionarios;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Procedures.ServiciosGenerales.Fumigacion
{
    public interface IVFumigacionProcedure
    {
        Task<bool> VerificaDeductivas(int id);
    }

    public class VFumigacionProcedure : IVFumigacionProcedure
    {
        private readonly IFCuestionarioProxy _cuestionario;
        private readonly IFCedulaProxy _cedula;
        private readonly IFRespuestaProxy _respuestas;

        public VFumigacionProcedure(IFCuestionarioProxy cuestionario, IFCedulaProxy cedula, IFRespuestaProxy respuestas)
        {
            _cuestionario = cuestionario;
            _cedula = cedula;
            _respuestas = respuestas;
        }

        public async Task<bool> VerificaDeductivas(int id)
        {
            var cedula = await _cedula.GetCedulaById(id);

            var pDeductivas = (await _cuestionario.GetCuestionarioMensualId(cedula.Anio, cedula.MesId, cedula.ContratoId))
                              .Where(cm => cm.Tipo.Equals("Deductiva")).Select(cm => cm.Consecutivo).ToList();

            var nc = (await _respuestas.GetRespuestasEvaluacionByCedulaAnioMes(id))
                             .Where(r => pDeductivas.Contains(r.Pregunta)).Sum(r => r.MontoPenalizacion) > 0;


            return nc;
        }

    }
}
