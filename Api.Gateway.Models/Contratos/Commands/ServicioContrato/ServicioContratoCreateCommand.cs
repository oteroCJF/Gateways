namespace Api.Gateway.Models.Contratos.Commands.ServicioContrato
{
    public class ServicioContratoCreateCommand
    {
        public int ContratoId { get; set; }
        public int ServicioId { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal IVA { get; set; }
        public decimal PorcentajeImpuesto { get; set; }
    }
}
