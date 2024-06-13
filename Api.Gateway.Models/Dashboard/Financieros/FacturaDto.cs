namespace Api.Gateway.Models.Dashboard.Financieros
{
    public class FacturaDto
    {
        public int Anio { get; set; }
        public int RepositorioId { get; set; }
        public int TotalInmuebles { get; set; }
        public int Pendientes { get; set; }
        public int Cargadas { get; set; }
        public decimal PorcentajeAvance { get; set; }
        public string Mes { get; set; }
        public string Fondo { get; set; }
    }
}
