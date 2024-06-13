namespace Api.Gateway.Models.Catalogos.DTOs.Incidencias
{
    public class CTIAguaDto
    {
        public int Id { get; set; }
        public int IncidenciaId { get; set; }
        public string Abreviacion { get; set; }
        public string Nombre { get; set; }

        public CTIncidenciaDto Incidencia { get; set; } = new CTIncidenciaDto();
    }
}
