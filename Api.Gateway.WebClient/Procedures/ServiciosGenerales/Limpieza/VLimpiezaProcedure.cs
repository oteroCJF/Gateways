using Api.Gateway.Proxies.Limpieza.CedulaEvaluacion;
using Api.Gateway.Proxies.Limpieza.Cuestionarios;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Gateway.WebClient.Procedures.ServiciosGenerales.Limpieza
{
    public interface IVLimpiezaProcedure
    {
        Task<bool> VerificaDeductivas(int id);
    }

    public class VLimpiezaProcedure : IVLimpiezaProcedure
    {
        private readonly ILCuestionarioProxy _cuestionario;
        private readonly ILCedulaProxy _cedula;
        private readonly ILRespuestaProxy _respuestas;

        public VLimpiezaProcedure(ILCuestionarioProxy cuestionario, ILCedulaProxy cedula, ILRespuestaProxy respuestas)
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
