using Api.Gateway.Proxies.Transporte.Cuestionarios.Queries;
using Api.Gateway.Proxies.Transporte.Respuestas.Queries;
using Api.Gateway.Proxies.Transporte.Contratos.Commands;
using Api.Gateway.Proxies.Transporte.Contratos.Queries;
using Api.Gateway.Proxies.Transporte.Convenios.Commands;
using Api.Gateway.Proxies.Transporte.Convenios.Queries;
using Api.Gateway.Proxies.Transporte.EntregablesContrato.Commands;
using Api.Gateway.Proxies.Transporte.EntregablesContrato.Queries;
using Api.Gateway.Proxies.Transporte.ServiciosContrato.Commands;
using Api.Gateway.Proxies.Transporte.ServiciosContrato.Queries;
using Api.Gateway.WebClient.Controllers.Transporte.CedulasEvaluacion.Procedures;
using Api.Gateway.WebClient.Controllers.Transporte.CFDIs.Procedure;
using Api.Gateway.WebClient.Controllers.Transporte.Entregables.Procedures.Commands;
using Api.Gateway.WebClient.Controllers.Transporte.Entregables.Procedures.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Api.Gateway.Proxies.Transporte.Firmantes.Queries;
using Api.Gateway.Proxies.Transporte.CedulasEvaluacion;
using Api.Gateway.Proxies.Transporte.Repositorios.Queries;
using Api.Gateway.Proxies.Transporte.CFDIs.Queries;
using Api.Gateway.Proxies.Transporte.Entregables;
using Api.Gateway.Proxies.Transporte.LogCedulas.Queries;
using Api.Gateway.Proxies.Transporte.LogEntregables.Queries;
using Api.Gateway.Proxies.Transporte.Incidencias.Queries;
using Api.Gateway.Proxies.Transporte.CFDIs.Commands;
using Api.Gateway.Proxies.Transporte.Entregables.Commands;
using Api.Gateway.Proxies.Transporte.Repositorios.Commands;
using Api.Gateway.Proxies.Transporte.Respuestas.Commands;
using Api.Gateway.Proxies.Transporte.CedulasEvaluacion.Commands;
using Api.Gateway.Proxies.Transporte.Firmantes.Commands;
using Api.Gateway.Proxies.Transporte.LogCedulas.Commands;
using Api.Gateway.Proxies.Transporte.LogEntregables.Commands;
using Api.Gateway.Proxies.Transporte.Incidencias.Commands;

namespace Api.Gateway.WebClient.Config.Transporte
{
    public static class StartUpConfiguration
    {
        public static IServiceCollection AddProxiesTransporteQueries(this IServiceCollection service, IConfiguration configuration)
        {
            //Servicio de Transporte
            service.AddHttpClient<IQContratoTransporteProxy, QContratoTransporteProxy>();
            service.AddHttpClient<IQSContratoTransporteProxy, QSContratoTransporteProxy>();
            service.AddHttpClient<IQConvenioTransporteProxy, QConvenioTransporteProxy>();
            service.AddHttpClient<IQEContratoTransporteProxy, QEContratoTransporteProxy>();

            service.AddHttpClient<IQCuestionarioTransporteProxy, QCuestionarioTransporteProxy>();
            service.AddHttpClient<IQFirmanteTransporteProxy, QFirmanteTransporteProxy>();
            service.AddHttpClient<IQCedulaTransporteProxy, QCedulaTransporteProxy>();
            service.AddHttpClient<IQRespuestaTransporteProxy, QRespuestaTransporteProxy>();
            service.AddHttpClient<IQRepositorioTransporteProxy, QRepositorioTransporteProxy>();
            service.AddHttpClient<IQCFDITransporteProxy, QCFDITransporteProxy>();
            service.AddHttpClient<IQEntregableTransporteProxy, QEntregableTransporteProxy>();
            service.AddHttpClient<IQLCedulaTransporteProxy, QLCedulaTransporteProxy>();
            service.AddHttpClient<IQLEntregableTransporteProxy, QLEntregableTransporteProxy>();
            service.AddHttpClient<IQIncidenciaTransporteProxy, QIncidenciaTransporteProxy>();

            service.AddScoped<ICedulaTransporteProcedure, CedulaTransporteProcedure>();
            service.AddScoped<IQTransporteEntregableProcedure, QTransporteEntregableProcedure>();
            service.AddScoped<ICEntregableTransporteProcedure, CEntregableTransporteProcedure>();
            service.AddScoped<ICFDITransporteProcedure, CFDITransporteProcedure>();

            return service;
        }

        public static IServiceCollection AddProxiesTransporteCommands(this IServiceCollection service, IConfiguration configuration)
        {
            //Servicio de Transporte
            service.AddHttpClient<ICContratoTransporteProxy, CContratoTransporteProxy>();
            service.AddHttpClient<ICSContratoTransporteProxy, CSContratoTransporteProxy>();
            service.AddHttpClient<ICConvenioTransporteProxy, CConvenioTransporteProxy>();
            service.AddHttpClient<ICEContratoTransporteProxy, CEContratoTransporteProxy>();

            service.AddHttpClient<ICFirmanteTransporteProxy, CFirmanteTransporteProxy>();
            service.AddHttpClient<ICCedulaTransporteProxy, CCedulaTransporteProxy>();
            service.AddHttpClient<ICRespuestaTransporteProxy, CRespuestaTransporteProxy>();
            service.AddHttpClient<ICRepositorioTransporteProxy, CRepositorioTransporteProxy>();
            service.AddHttpClient<ICCFDITransporteProxy, CCFDITransporteProxy>();
            service.AddHttpClient<ICEntregableTransporteProxy, CEntregableTransporteProxy>();
            service.AddHttpClient<ICLCedulaTransporteProxy, CLCedulaTransporteProxy>();
            service.AddHttpClient<ICLEntregableTransporteProxy, CLEntregableTransporteProxy>();
            service.AddHttpClient<ICIncidenciaTransporteProxy, CIncidenciaTransporteProxy>();

            return service;
        }
    }
}
