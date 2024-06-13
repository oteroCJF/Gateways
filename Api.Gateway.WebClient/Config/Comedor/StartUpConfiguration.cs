using Api.Gateway.Proxies.Comedor.Contratos.Commands;
using Api.Gateway.Proxies.Comedor.Contratos.Queries;
using Api.Gateway.Proxies.Comedor.Convenios.Commands;
using Api.Gateway.Proxies.Comedor.Convenios.Queries;
using Api.Gateway.Proxies.Comedor.EntregablesContrato.Commands;
using Api.Gateway.Proxies.Comedor.EntregablesContrato.Queries;
using Api.Gateway.Proxies.Comedor.ServiciosContrato.Commands;
using Api.Gateway.Proxies.Comedor.ServiciosContrato.Queries;
using Api.Gateway.Proxies.Comedor.Cuestionarios.Queries;
using Api.Gateway.Proxies.Comedor.Respuestas.Queries;
using Api.Gateway.WebClient.Controllers.Comedor.CedulasEvaluacion.Procedures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Api.Gateway.Proxies.Comedor.Firmantes.Queries;
using Api.Gateway.Proxies.Comedor.CedulasEvaluacion.Queries;
using Api.Gateway.Proxies.Comedor.Repositorios.Queries;
using Api.Gateway.Proxies.Comedor.CFDIs.Queries;
using Api.Gateway.Proxies.Comedor.Entregables.Queries;
using Api.Gateway.Proxies.Comedor.LogCedula.Queries;
using Api.Gateway.Proxies.Comedor.LogEntregables.Queries;
using Api.Gateway.WebClient.Controllers.Comedor.Entregables.Procedures.Queries;
using Api.Gateway.WebClient.Controllers.Comedor.Entregables.Procedures.Commands;
using Api.Gateway.Proxies.Comedor.Firmantes.Commands;
using Api.Gateway.Proxies.Comedor.CedulasEvaluacion.Commands;
using Api.Gateway.Proxies.Comedor.Respuestas.Commands;
using Api.Gateway.Proxies.Comedor.Repositorios.Commands;
using Api.Gateway.Proxies.Comedor.CFDIs.Commands;
using Api.Gateway.Proxies.Comedor.Entregables.Commands;
using Api.Gateway.Proxies.Comedor.LogCedula.Commands;
using Api.Gateway.Proxies.Comedor.LogEntregables.Commands;
using Api.Gateway.Proxies.Comedor.Incidencias.Queries;
using Api.Gateway.Proxies.Comedor.Incidencias.Commands;
using Api.Gateway.WebClient.Controllers.Comedor.CFDIs.Procedures;

namespace Api.Gateway.WebClient.Config.Comedor
{
    public static class StartUpConfiguration
    {
        public static IServiceCollection AddProxiesComedorQueries(this IServiceCollection service, IConfiguration configuration)
        {
            //Servicio de Comedor
            service.AddHttpClient<IQCuestionarioComedorProxy, QCuestionarioComedorProxy>();
            service.AddHttpClient<IQFirmanteComedorProxy, QFirmanteComedorProxy>();
            service.AddHttpClient<IQCedulaComedorProxy, QCedulaComedorProxy>();
            service.AddHttpClient<IQRespuestaComedorProxy, QRespuestaComedorProxy>();
            service.AddHttpClient<IQRepositorioComedorProxy, QRepositorioComedorProxy>();
            service.AddHttpClient<IQCFDIComedorProxy, QCFDIComedorProxy>();
            service.AddHttpClient<IQContratoComedorProxy, QContratoComedorProxy>();
            service.AddHttpClient<IQSContratoComedorProxy, QSContratoComedorProxy>();
            service.AddHttpClient<IQConvenioComedorProxy, QConvenioComedorProxy>();
            service.AddHttpClient<IQEntregableComedorProxy, QEntregableComedorProxy>();
            service.AddHttpClient<IQEContratoComedorProxy, QEContratoComedorProxy>();
            service.AddHttpClient<IQLCedulaComedorProxy, QLCedulaComedorProxy>();
            service.AddHttpClient<IQLEntregableComedorProxy, QLEntregableComedorProxy>();
            service.AddHttpClient<IQIncidenciaComedorProxy, QIncidenciaComedorProxy>();

            service.AddScoped<ICedulaComedorProcedure, CedulaComedorProcedure>();
            service.AddScoped<IQComedorEntregableProcedure, QComedorEntregableProcedure>();
            service.AddScoped<ICEntregableComedorProcedure, CEntregableComedorProcedure>();
            service.AddScoped<ICCFDIProcedure, CCFDIProcedure>();
            service.AddScoped<ICuestionarioComedorProcedure, CuestionarioComedorProcedure>();


            return service;
        }

        public static IServiceCollection AddProxiesComedorCommands(this IServiceCollection service, IConfiguration configuration)
        {
            //Servicio de Comedor
            service.AddHttpClient<ICFirmanteComedorProxy, CFirmanteComedorProxy>();
            service.AddHttpClient<ICCedulaComedorProxy, CCedulaComedorProxy>();
            service.AddHttpClient<ICRespuestaComedorProxy, CRespuestaComedorProxy>();
            service.AddHttpClient<ICRepositorioComedorProxy, CRepositorioComedorProxy>();
            service.AddHttpClient<ICCFDIComedorProxy, CCFDIComedorProxy>();
            service.AddHttpClient<ICContratoComedorProxy, CContratoComedorProxy>();
            service.AddHttpClient<ICSContratoComedorProxy, CSContratoComedorProxy>();
            service.AddHttpClient<ICConvenioComedorProxy, CConvenioComedorProxy>();
            service.AddHttpClient<ICEntregableComedorProxy, CEntregableComedorProxy>();
            service.AddHttpClient<ICEContratoComedorProxy, CEContratoComedorProxy>();
            service.AddHttpClient<ICLCedulaComedorProxy, CLCedulaComedorProxy>();
            service.AddHttpClient<ICLEntregableComedorProxy, CLEntregableComedorProxy>();
            service.AddHttpClient<ICIncidenciaComedorProxy, CIncidenciaComedorProxy>();
            //service.AddHttpClient<ICOficioComedorProxy, COficioComedorProxy>();

            return service;
        }
    }
}
