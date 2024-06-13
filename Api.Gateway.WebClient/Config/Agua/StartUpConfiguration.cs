using Api.Gateway.Proxies.Agua.CedulasEvaluacion;
using Api.Gateway.Proxies.Agua.CedulasEvaluacion.Commands;
using Api.Gateway.Proxies.Agua.CFDIs.Commands;
using Api.Gateway.Proxies.Agua.CFDIs.Queries;
using Api.Gateway.Proxies.Agua.Contratos.Commands;
using Api.Gateway.Proxies.Agua.Contratos.Queries;
using Api.Gateway.Proxies.Agua.Convenios.Commands;
using Api.Gateway.Proxies.Agua.Convenios.Queries;
using Api.Gateway.Proxies.Agua.Cuestionarios.Queries;
using Api.Gateway.Proxies.Agua.Entregables;
using Api.Gateway.Proxies.Agua.Entregables.Commands;
using Api.Gateway.Proxies.Agua.EntregablesContrato.Commands;
using Api.Gateway.Proxies.Agua.EntregablesContrato.Queries;
using Api.Gateway.Proxies.Agua.Firmantes.Commands;
using Api.Gateway.Proxies.Agua.Firmantes.Queries;
using Api.Gateway.Proxies.Agua.Incidencias.Commands;
using Api.Gateway.Proxies.Agua.Incidencias.Queries;
using Api.Gateway.Proxies.Agua.LogCedulas.Commands;
using Api.Gateway.Proxies.Agua.LogCedulas.Queries;
using Api.Gateway.Proxies.Agua.LogEntregables.Commands;
using Api.Gateway.Proxies.Agua.LogEntregables.Queries;
using Api.Gateway.Proxies.Agua.Oficios.Commands;
using Api.Gateway.Proxies.Agua.Oficios.Queries;
using Api.Gateway.Proxies.Agua.Repositorios.Commands;
using Api.Gateway.Proxies.Agua.Repositorios.Queries;
using Api.Gateway.Proxies.Agua.Respuestas.Commands;
using Api.Gateway.Proxies.Agua.Respuestas.Queries;
using Api.Gateway.Proxies.Agua.ServiciosContrato.Commands;
using Api.Gateway.Proxies.Agua.ServiciosContrato.Queries;
using Api.Gateway.WebClient.Controllers.Agua.CedulasEvaluacion.Procedures;
using Api.Gateway.WebClient.Controllers.Agua.CFDIs.Procedure;
using Api.Gateway.WebClient.Controllers.Agua.Entregables.Procedures.Commands;
using Api.Gateway.WebClient.Controllers.Agua.Entregables.Procedures.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Gateway.WebClient.Config.Agua
{
    public static class StartUpConfiguration
    {
        public static IServiceCollection AddProxiesAguaQueries(this IServiceCollection service, IConfiguration configuration)
        {
            //Servicio de Agua
            service.AddHttpClient<IQCuestionarioAguaProxy, QCuestionarioAguaProxy>();
            service.AddHttpClient<IQFirmanteAguaProxy, QFirmanteAguaProxy>();
            service.AddHttpClient<IQCedulaAguaProxy, QCedulaAguaProxy>();
            service.AddHttpClient<IQRespuestaAguaProxy, QRespuestaAguaProxy>();
            service.AddHttpClient<IQRepositorioAguaProxy, QRepositorioAguaProxy>();
            service.AddHttpClient<IQCFDIAguaProxy, QCFDIAguaProxy>();
            service.AddHttpClient<IQContratoAguaProxy, QContratoAguaProxy>();
            service.AddHttpClient<IQSContratoAguaProxy, QSContratoAguaProxy>();
            service.AddHttpClient<IQConvenioAguaProxy, QConvenioAguaProxy>();
            service.AddHttpClient<IQEntregableAguaProxy, QEntregableAguaProxy>();
            service.AddHttpClient<IQEntregableAguaProxy, QEntregableAguaProxy>();
            service.AddHttpClient<IQEContratoAguaProxy, QEContratoAguaProxy>();
            service.AddHttpClient<IQLCedulaAguaProxy, QLCedulaAguaProxy>();
            service.AddHttpClient<IQLEntregableAguaProxy, QLEntregableAguaProxy>();
            service.AddHttpClient<IQIncidenciaAguaProxy, QIncidenciaAguaProxy>();
            service.AddHttpClient<IQOficioAguaProxy, QOficioAguaProxy>();

            service.AddScoped<ICedulaAguaProcedure, CedulaAguaProcedure>();
            service.AddScoped<IQAguaEntregableProcedure, QAguaEntregableProcedure>();            
            service.AddScoped<ICEntregableAguaProcedure, CEntregableAguaProcedure>();            
            service.AddScoped<ICFDIAguaProcedure, CFDIAguaProcedure>();            
            

            return service;
        }

        public static IServiceCollection AddProxiesAguaCommands(this IServiceCollection service, IConfiguration configuration)
        {
            //Servicio de Agua
            service.AddHttpClient<ICFirmanteAguaProxy, CFirmanteAguaProxy>();
            service.AddHttpClient<ICCedulaAguaProxy, CCedulaAguaProxy>();
            service.AddHttpClient<ICRespuestaAguaProxy, CRespuestaAguaProxy>();
            service.AddHttpClient<ICRepositorioAguaProxy, CRepositorioAguaProxy>();
            service.AddHttpClient<ICCFDIAguaProxy, CCFDIAguaProxy>();
            service.AddHttpClient<ICContratoAguaProxy, CContratoAguaProxy>();
            service.AddHttpClient<ICSContratoAguaProxy, CSContratoAguaProxy>();
            service.AddHttpClient<ICConvenioAguaProxy, CConvenioAguaProxy>();
            service.AddHttpClient<ICEntregableAguaProxy, CEntregableAguaProxy>();
            service.AddHttpClient<ICEntregableAguaProxy, CEntregableAguaProxy>();
            service.AddHttpClient<ICEContratoAguaProxy, CEContratoAguaProxy>();
            service.AddHttpClient<ICLCedulaAguaProxy, CLCedulaAguaProxy>();
            service.AddHttpClient<ICLEntregableAguaProxy, CLEntregableAguaProxy>();
            service.AddHttpClient<ICLEntregableAguaProxy, CLEntregableAguaProxy>();
            service.AddHttpClient<ICIncidenciaAguaProxy, CIncidenciaAguaProxy>(); 
            service.AddHttpClient<ICOficioAguaProxy, COficioAguaProxy>();

            return service;
        }
    }
}
