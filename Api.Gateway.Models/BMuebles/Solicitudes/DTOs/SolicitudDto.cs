using Api.Gateway.Models.Catalogos.DTOs.ActividadesContrato;
using Api.Gateway.Models.Contratos.DTOs;
using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Inmuebles.DTOs.Inmuebles;
using Api.Gateway.Models.Meses.DTOs;
using Api.Gateway.Models.Usuarios.DTOs;
using System;

namespace Api.Gateway.Models.BMuebles.Solicitudes.DTOs
{
    public class SolicitudDto
    {
        public int Id { get; set; }
        public int TipoId { get; set; }
        public string UsuarioId { get; set; }
        public UsuarioDto Elaboro { get; set; } = new UsuarioDto();
        public int ContratoId { get; set; }
        public ContratoDto Contrato { get; set; } = new ContratoDto();
        public int InmuebleId { get; set; }
        public InmuebleDto Inmueble { get; set; } = new InmuebleDto();
        public int PartidaId { get; set; }
        public CTActividadContratoDto Partida { get; set; } = new CTActividadContratoDto();
        public int EstatusId { get; set; }
        public EstatusDto Estatus { get; set; } = new EstatusDto();
        public int Anio { get; set; }
        public int MesId { get; set; }
        public MesDto Mes { get; set; } = new MesDto();
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaServicio { get; set; }
        public string HoraServicio { get; set; }
        public int OrigenId { get; set; }
        public InmuebleDto Origen { get; set; } = new InmuebleDto();
        public int DestinoId { get; set; }
        public InmuebleDto Destino { get; set; } = new InmuebleDto();
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
