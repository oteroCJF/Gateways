using Api.Gateway.Proxies;
using Api.Gateway.Proxies.BMuebles.Contratos;
using Api.Gateway.Proxies.BMuebles.Convenios;
using Api.Gateway.Proxies.BMuebles.EntregablesContrato;
using Api.Gateway.Proxies.BMuebles.Solicitudes;
using Api.Gateway.Proxies.Catalogos.CTActividadesContrato;
using Api.Gateway.Proxies.Catalogos.CTDestinos;
using Api.Gateway.Proxies.Catalogos.CTDiasInhabiles;
using Api.Gateway.Proxies.Catalogos.CTEntregables;
using Api.Gateway.Proxies.Catalogos.CTIncidencias;
using Api.Gateway.Proxies.Catalogos.CTIndemnizacion;
using Api.Gateway.Proxies.Catalogos.CTMarcoJuridico;
using Api.Gateway.Proxies.Catalogos.CTParametros;
using Api.Gateway.Proxies.Catalogos.CTServicios;
using Api.Gateway.Proxies.Catalogos.CTServiciosContratos;
using Api.Gateway.Proxies.Estatus;
using Api.Gateway.Proxies.Fumigacion.CedulaEvaluacion;
using Api.Gateway.Proxies.Fumigacion.CedulasEvaluacion;
using Api.Gateway.Proxies.Fumigacion.CFDIs;
using Api.Gateway.Proxies.Fumigacion.Contratos;
using Api.Gateway.Proxies.Fumigacion.Convenios;
using Api.Gateway.Proxies.Fumigacion.Cuestionarios;
using Api.Gateway.Proxies.Fumigacion.Entregables;
using Api.Gateway.Proxies.Fumigacion.EntregablesContratacion;
using Api.Gateway.Proxies.Fumigacion.Facturacion;
using Api.Gateway.Proxies.Fumigacion.Firmantes;
using Api.Gateway.Proxies.Fumigacion.Historiales;
using Api.Gateway.Proxies.Fumigacion.Incidencias;
using Api.Gateway.Proxies.Fumigacion.Oficios;
using Api.Gateway.Proxies.Fumigacion.ServicioContrato;
using Api.Gateway.Proxies.Inmuebles.Direcciones;
using Api.Gateway.Proxies.Limpieza.CedulaEvaluacion;
using Api.Gateway.Proxies.Limpieza.Contratos;
using Api.Gateway.Proxies.Limpieza.Convenios;
using Api.Gateway.Proxies.Limpieza.Cuestionarios;
using Api.Gateway.Proxies.Limpieza.Entregables;
using Api.Gateway.Proxies.Limpieza.EntregablesContratacion;
using Api.Gateway.Proxies.Limpieza.Facturas;
using Api.Gateway.Proxies.Limpieza.Firmantes;
using Api.Gateway.Proxies.Limpieza.Historiales;
using Api.Gateway.Proxies.Limpieza.Incidencias;
using Api.Gateway.Proxies.Limpieza.Oficios;
using Api.Gateway.Proxies.Limpieza.Repositorios;
using Api.Gateway.Proxies.Limpieza.ServicioContrato;
using Api.Gateway.Proxies.Limpieza.Variables;
using Api.Gateway.Proxies.Meses;
using Api.Gateway.Proxies.Modulos;
using Api.Gateway.Proxies.Permisos;
using Api.Gateway.Proxies.ServiciosBasicos.AEElectrica.CFDIs;
using Api.Gateway.Proxies.ServiciosBasicos.AEElectrica.Entregables;
using Api.Gateway.Proxies.ServiciosBasicos.AEElectrica.LogEntregables;
using Api.Gateway.Proxies.ServiciosBasicos.AEElectrica.LogSolicitudes;
using Api.Gateway.Proxies.ServiciosBasicos.AEElectrica.SolicitudesPagos;
using Api.Gateway.Proxies.Usuarios;
using Api.Gateway.WebClient.Controllers.Mensajeria.Entregables.Procedures;
using Api.Gateway.WebClient.Procedures.Permisos;
using Api.Gateway.WebClient.Procedures.ServiciosBasicos.AEElectrica;
using Api.Gateway.WebClient.Procedures.ServiciosGenerales.CFDI;
using Api.Gateway.WebClient.Procedures.ServiciosGenerales.Estatus;
using Api.Gateway.WebClient.Procedures.ServiciosGenerales.Fumigacion;
using Api.Gateway.WebClient.Procedures.ServiciosGenerales.Limpieza;
using Api.Gateways.Proxies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Gateway.WebClient.Config
{
    public static class StartUpConfiguration
    {
        public static IServiceCollection AddAppsettingBinding(this IServiceCollection service, IConfiguration configuration)
        {
            service.Configure<ApiUrls>(opts => configuration.GetSection("ApiUrls").Bind(opts));
            return service;
        }

        public static IServiceCollection AddProxiesCatalogos(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddHttpClient<ICTEntregableProxy, CTEntregableProxy>();
            service.AddHttpClient<ICTServicioProxy, CTServicioProxy>();
            service.AddHttpClient<ICTIncidenciaProxy, CTIncidenciaProxy>();
            service.AddHttpClient<ICTServicioContratoProxy, CTServicioContratoProxy>();            
            service.AddHttpClient<ICTParametroProxy, CTParametroProxy>();
            service.AddHttpClient<ICTIComedorProxy, CTIComedorProxy>();            
            service.AddHttpClient<ICTILimpiezaProxy, CTILimpiezaProxy>();            
            service.AddHttpClient<ICTIndemnizacionProxy, CTIndemnizacionProxy>();
            service.AddHttpClient<ICTIFumigacionProxy, CTIFumigacionProxy>();
            service.AddHttpClient<ICTIAguaProxy, CTIAguaProxy>();
            service.AddHttpClient<ICTActividadContratoProxy, CTActividadContratoProxy>();
            service.AddHttpClient<ICTDestinoProxy, CTDestinoProxy>();
            service.AddHttpClient<ICTDiasInhabilesProxy, CTDiasInhabilesProxy>();
            service.AddHttpClient<ICTMarcoJuridicoProxy, CTMarcoJuridicoProxy>();

            return service;
        }
        
        public static IServiceCollection AddProxiesEstatus(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddHttpClient<IEstatusCedulaProxy, EstatusCedulaProxy>();
            service.AddHttpClient<IEstatusFacturaProxy, EstatusFacturaProxy>();
            service.AddHttpClient<IEstatusEntregableProxy, EstatusEntregableProxy>();
            service.AddHttpClient<IEstatusSPProxy, EstatusSPProxy>();
            service.AddHttpClient<IEstatusOficioProxy, OficioProxy>();

            return service;
        }
        
        public static IServiceCollection AddProxiesGenerales(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddHttpClient<IPermisoProxy, PermisoProxy>();
            service.AddScoped<IPermisosProcedure, PermisosProcedure>();
            service.AddHttpClient<IModuloProxy, ModuloProxy>();
            service.AddHttpClient<ISubmoduloProxy, SubmoduloProxy>();
            service.AddHttpClient<IInmuebleProxy, InmuebleProxy>();
            service.AddHttpClient<IDireccionProxy, DireccionProxy>();
            service.AddHttpClient<IMesProxy, MesProxy>();
            service.AddHttpClient<IUsuarioProxy, UsuarioProxy>();
            service.AddScoped<IEstatusProcedure, EstatusProcedure>();
            service.AddScoped<ICFDIProcedure, MCFDIProcedure>();

            return service;
        }

        public static IServiceCollection AddProxiesAEElectrica(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddScoped<IAEElectricaProcedure, AEElectricaProcedure>();
            service.AddHttpClient<IAEESolicitudPagoProxy, AEESolicitudPagoProxy>();
            service.AddHttpClient<IAEECFDIProxy, AEECFDIProxy>();
            service.AddHttpClient<IAEEEntregableProxy, AEEEntregableProxy>();            
            service.AddHttpClient<IAEELogEntregableProxy, AEELogEntregableProxy>();            
            service.AddHttpClient<IAEELogSolicitudProxy, AEELogSolicitudProxy>();            

            return service;
        }

        public static IServiceCollection AddProxiesLimpieza(this IServiceCollection service, IConfiguration configuration)
        {
            //Servicio de Limpieza
            service.AddHttpClient<ILCuestionarioProxy, LCuestionarioProxy>();
            service.AddHttpClient<ILVariableProxy, LParametroProxy>();
            service.AddHttpClient<ILFirmanteProxy, LFirmanteProxy>();
            service.AddHttpClient<ILCedulaProxy, LCedulaProxy>();
            service.AddHttpClient<ILRepositorioProxy, LRepositorioProxy>();
            service.AddHttpClient<ILCFDIProxy, LCFDIProxy>();
            service.AddHttpClient<ILContratoProxy, LContratoProxy>();
            service.AddHttpClient<ILServicioContratoProxy, LSContratoProxy>();
            service.AddHttpClient<ILConvenioProxy, LConvenioProxy>();
            service.AddHttpClient<ILRespuestaProxy, LRespuestaProxy>();
            service.AddHttpClient<ILIncidenciaProxy, LIncidenciaProxy>();
            service.AddHttpClient<ILEntregableProxy, LEntregableProxy>();
            service.AddHttpClient<ILEContratoProxy, LEContratoProxy>();
            service.AddHttpClient<ILLogCedulaProxy, LLogCedulaProxy>();
            service.AddHttpClient<ILLogEntregableProxy, LLEntregableProxy>();
            service.AddHttpClient<ILOficioProxy, LOficioProxy>();

            service.AddScoped<ILEntregablesProcedure, LEntregablesProcedure>();
            service.AddScoped<IVELimpiezaProcedure, VELimpiezaProcedure>();
            service.AddScoped<IVLimpiezaProcedure, VLimpiezaProcedure>();

            return service;
        }
        
        public static IServiceCollection AddProxiesFumigacion(this IServiceCollection service, IConfiguration configuration)
        {
            //Servicio de Mensajeria
            service.AddHttpClient<IFCuestionarioProxy, FCuestionarioProxy>();
            service.AddHttpClient<IFFirmanteProxy, FFirmanteProxy>();
            service.AddHttpClient<IFCedulaProxy, FCedulaProxy>();
            service.AddHttpClient<IFRespuestaProxy, FRespuestaProxy>();
            service.AddHttpClient<IFRepositorioProxy, FRepositorioProxy>();
            service.AddHttpClient<IFCFDIProxy, FCFDIProxy>();
            service.AddHttpClient<IFContratoProxy, FContratoProxy>();
            service.AddHttpClient<IFServicioContratoProxy, FServicioContratoProxy>();
            service.AddHttpClient<IFConvenioProxy, FConvenioProxy>();
            service.AddHttpClient<IFEntregableProxy, FEntregableProxy>();
            service.AddHttpClient<IFEContratoProxy, FEContratoProxy>();
            service.AddHttpClient<IFIncidenciaProxy, FIncidenciaProxy>();
            service.AddHttpClient<IFLCedulaProxy, FLCedulaProxy>();
            service.AddHttpClient<IFLEntregableProxy, FLEntregableProxy>();
            service.AddHttpClient<IFOficioProxy, FOficioProxy>();

            service.AddScoped<IFCedulaProcedure, FCedulaProcedure>();
            service.AddScoped<IFEntregablesProcedure, FEntregablesProcedure>();
            service.AddScoped<IFCFDIProcedure, FCFDIProcedure>();
            service.AddScoped<IVEFumigacionProcedure, VEFumigacionProcedure>();
            service.AddScoped<IVFumigacionProcedure, VFumigacionProcedure>();

            return service;
        }
        
        public static IServiceCollection AddProxiesBMuebles(this IServiceCollection service, IConfiguration configuration)
        {
            //Servicio de Bienes Muebles
            service.AddHttpClient<IBMSolicitudProxy, BMSolicitudProxy>();
            service.AddHttpClient<IBMContratoProxy, BMContratoProxy>();
            service.AddHttpClient<IBMConvenioProxy, BMConvenioProxy>();
            service.AddHttpClient<IBMEContratoProxy, BMEContratoProxy>();

            return service;
        }
    }
}
