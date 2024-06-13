using Api.Gateway.Proxies.Mensajeria.CedulasEvaluacion;
using Api.Gateway.Proxies.Mensajeria.CedulasEvaluacion.Commands;
using Api.Gateway.Proxies.Mensajeria.CFDIs.Commands;
using Api.Gateway.Proxies.Mensajeria.CFDIs.Queries;
using Api.Gateway.Proxies.Mensajeria.Contratos.Commands;
using Api.Gateway.Proxies.Mensajeria.Contratos.Queries;
using Api.Gateway.Proxies.Mensajeria.Convenios.Commands;
using Api.Gateway.Proxies.Mensajeria.Convenios.Queries;
using Api.Gateway.Proxies.Mensajeria.Cuestionarios.Queries;
using Api.Gateway.Proxies.Mensajeria.Entregables;
using Api.Gateway.Proxies.Mensajeria.Entregables.Commands;
using Api.Gateway.Proxies.Mensajeria.EntregablesContrato.Commands;
using Api.Gateway.Proxies.Mensajeria.EntregablesContrato.Queries;
using Api.Gateway.Proxies.Mensajeria.Firmantes.Commands;
using Api.Gateway.Proxies.Mensajeria.Firmantes.Queries;
using Api.Gateway.Proxies.Mensajeria.Incidencias.Commands;
using Api.Gateway.Proxies.Mensajeria.Incidencias.Queries;
using Api.Gateway.Proxies.Mensajeria.LogCedulas.Commands;
using Api.Gateway.Proxies.Mensajeria.LogCedulas.Queries;
using Api.Gateway.Proxies.Mensajeria.LogEntregables.Commands;
using Api.Gateway.Proxies.Mensajeria.LogEntregables.Queries;
using Api.Gateway.Proxies.Mensajeria.Oficios.Commands;
using Api.Gateway.Proxies.Mensajeria.Oficios.Queries;
using Api.Gateway.Proxies.Mensajeria.Repositorios.Commands;
using Api.Gateway.Proxies.Mensajeria.Repositorios.Queries;
using Api.Gateway.Proxies.Mensajeria.Respuestas.Commands;
using Api.Gateway.Proxies.Mensajeria.Respuestas.Queries;
using Api.Gateway.Proxies.Mensajeria.ServiciosContrato.Commands;
using Api.Gateway.Proxies.Mensajeria.ServiciosContrato.Queries;
using Api.Gateway.Proxies.Mensajeria.SoportePago.Commands;
using Api.Gateway.Proxies.Mensajeria.SoportePago.Queries;
using Api.Gateway.WebClient.Controllers.Mensajeria.CedulasEvaluacion.Procedures;
using Api.Gateway.WebClient.Controllers.Mensajeria.CFDIs.Procedure;
using Api.Gateway.WebClient.Controllers.Mensajeria.Entregables.Procedures.Commands;
using Api.Gateway.WebClient.Controllers.Mensajeria.Entregables.Procedures.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Gateway.WebClient.Config.Mensajeria
{
    public static class StartUpConfiguration
    {
        public static IServiceCollection AddProxiesMensajeriaQueries(this IServiceCollection service, IConfiguration configuration)
        {
            //Servicio de Mensajeria
            service.AddHttpClient<IQCuestionarioMensajeriaProxy, QCuestionarioMensajeriaProxy>();
            service.AddHttpClient<IQFirmanteMensajeriaProxy, QFirmanteMensajeriaProxy>();
            service.AddHttpClient<IQCedulaMensajeriaProxy, QCedulaMensajeriaProxy>();
            service.AddHttpClient<IQRespuestaMensajeriaProxy, QRespuestaMensajeriaProxy>();
            service.AddHttpClient<IQRepositorioMensajeriaProxy, QRepositorioMensajeriaProxy>();
            service.AddHttpClient<IQCFDIMensajeriaProxy, QCFDIMensajeriaProxy>();
            service.AddHttpClient<IQContratoMensajeriaProxy, QContratoMensajeriaProxy>();
            service.AddHttpClient<IQSContratoMensajeriaProxy, QSContratoMensajeriaProxy>();
            service.AddHttpClient<IQConvenioMensajeriaProxy, QConvenioMensajeriaProxy>();
            service.AddHttpClient<IQEntregableMensajeriaProxy, QEntregableMensajeriaProxy>();
            service.AddHttpClient<IQEntregableMensajeriaProxy, QEntregableMensajeriaProxy>();
            service.AddHttpClient<IQEContratoMensajeriaProxy, QEContratoMensajeriaProxy>();
            service.AddHttpClient<IQLCedulaMensajeriaProxy, QLCedulaMensajeriaProxy>();
            service.AddHttpClient<IQLEntregableMensajeriaProxy, QLEntregableMensajeriaProxy>();
            service.AddHttpClient<IQIncidenciaMensajeriaProxy, QIncidenciaMensajeriaProxy>();
            service.AddHttpClient<IQSoportePagoMensajeriaProxy, QSoportePagoMensajeriaProxy>();
            service.AddHttpClient<IQOficioMensajeriaProxy, QOficioMensajeriaProxy>();

            service.AddScoped<ICedulaMensajeriaProcedure, CedulaMensajeriaProcedure>();
            service.AddScoped<IQMensajeriaEntregableProcedure, QMensajeriaEntregableProcedure>();            
            service.AddScoped<ICEntregableMensajeriaProcedure, CEntregableMensajeriaProcedure>();            
            service.AddScoped<ICFDIMensajeriaProcedure, CFDIMensajeriaProcedure>();            
            

            return service;
        }

        public static IServiceCollection AddProxiesMensajeriaCommands(this IServiceCollection service, IConfiguration configuration)
        {
            //Servicio de Mensajeria
            service.AddHttpClient<ICFirmanteMensajeriaProxy, CFirmanteMensajeriaProxy>();
            service.AddHttpClient<ICCedulaMensajeriaProxy, CCedulaMensajeriaProxy>();
            service.AddHttpClient<ICRespuestaMensajeriaProxy, CRespuestaMensajeriaProxy>();
            service.AddHttpClient<ICRepositorioMensajeriaProxy, CRepositorioMensajeriaProxy>();
            service.AddHttpClient<ICCFDIMensajeriaProxy, CCFDIMensajeriaProxy>();
            service.AddHttpClient<ICContratoMensajeriaProxy, CContratoMensajeriaProxy>();
            service.AddHttpClient<ICSContratoMensajeriaProxy, CSContratoMensajeriaProxy>();
            service.AddHttpClient<ICConvenioMensajeriaProxy, CConvenioMensajeriaProxy>();
            service.AddHttpClient<ICEntregableMensajeriaProxy, CEntregableMensajeriaProxy>();
            service.AddHttpClient<ICEntregableMensajeriaProxy, CEntregableMensajeriaProxy>();
            service.AddHttpClient<ICEContratoMensajeriaProxy, CEContratoMensajeriaProxy>();
            service.AddHttpClient<ICLCedulaMensajeriaProxy, CLCedulaMensajeriaProxy>();
            service.AddHttpClient<ICLEntregableMensajeriaProxy, CLEntregableMensajeriaProxy>();
            service.AddHttpClient<ICLEntregableMensajeriaProxy, CLEntregableMensajeriaProxy>();
            service.AddHttpClient<ICIncidenciaMensajeriaProxy, CIncidenciaMensajeriaProxy>(); 
            service.AddHttpClient<ICSoportePagoMensajeriaProxy, CSoportePagoMensajeriaProxy>();
            service.AddHttpClient<ICOficioMensajeriaProxy, COficioMensajeriaProxy>();

            return service;
        }
    }
}
