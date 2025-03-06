//using Api.Gateway.Proxies.BMuebles.Contratos.Commands;
//using Api.Gateway.Proxies.BMuebles.Contratos.Queries;
//using Api.Gateway.Proxies.BMuebles.Convenios.Commands;
//using Api.Gateway.Proxies.BMuebles.Convenios.Queries;
//using Api.Gateway.Proxies.BMuebles.EntregablesContrato.Commands;
//using Api.Gateway.Proxies.BMuebles.EntregablesContrato.Queries;
//using Api.Gateway.Proxies.BMuebles.ServiciosContrato.Commands;
//using Api.Gateway.Proxies.BMuebles.ServiciosContrato.Queries;
//using Api.Gateway.Proxies.BMuebles.Cuestionarios.Queries;
//using Api.Gateway.Proxies.BMuebles.Respuestas.Queries;
//using Api.Gateway.WebClient.Controllers.BMuebles.CedulasEvaluacion.Procedures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Api.Gateway.Proxies.BMuebles.Firmantes.Queries;
//using Api.Gateway.Proxies.BMuebles.CedulasEvaluacion.Queries;
//using Api.Gateway.Proxies.BMuebles.Repositorios.Queries;
//using Api.Gateway.Proxies.BMuebles.CFDIs.Queries;
//using Api.Gateway.Proxies.BMuebles.Entregables.Queries;
//using Api.Gateway.Proxies.BMuebles.LogCedula.Queries;
//using Api.Gateway.Proxies.BMuebles.LogEntregables.Queries;
//using Api.Gateway.WebClient.Controllers.BMuebles.Entregables.Procedures.Queries;
//using Api.Gateway.WebClient.Controllers.BMuebles.Entregables.Procedures.Commands;
using Api.Gateway.Proxies.BMuebles.Firmantes.Commands;
//using Api.Gateway.Proxies.BMuebles.CedulasEvaluacion.Commands;
//using Api.Gateway.Proxies.BMuebles.Respuestas.Commands;
//using Api.Gateway.Proxies.BMuebles.Repositorios.Commands;
//using Api.Gateway.Proxies.BMuebles.CFDIs.Commands;
//using Api.Gateway.Proxies.BMuebles.Entregables.Commands;
//using Api.Gateway.Proxies.BMuebles.LogCedula.Commands;
//using Api.Gateway.Proxies.BMuebles.LogEntregables.Commands;
//using Api.Gateway.Proxies.BMuebles.Incidencias.Queries;
//using Api.Gateway.Proxies.BMuebles.Incidencias.Commands;
//using Api.Gateway.WebClient.Controllers.BMuebles.CFDIs.Procedures;

namespace Api.Gateway.WebClient.Config.BMuebles
{
    public static class StartUpConfiguration
    {
        public static IServiceCollection AddProxiesBMueblesQueries(this IServiceCollection service, IConfiguration configuration)
        {
            //Servicio de Comedor
            service.AddHttpClient<IQFirmanteBMueblesProxy, QFirmanteBMueblesProxy>();
            //service.AddHttpClient<IQCedulaBMueblesProxy, QCedulaBMueblesProxy>();
            //service.AddHttpClient<IQRespuestaBMueblesProxy, QRespuestaBMueblesProxy>();
            //service.AddHttpClient<IQRepositorioBMueblesProxy, QRepositorioBMueblesProxy>();
            //service.AddHttpClient<IQCFDIBMueblesProxy, QCFDIBMueblesProxy>();
            //service.AddHttpClient<IQContratoBMueblesProxy, QContratoBMueblesProxy>();
            //service.AddHttpClient<IQSContratoBMueblesProxy, QSContratoBMueblesProxy>();
            //service.AddHttpClient<IQConvenioBMueblesProxy, QConvenioBMueblesProxy>();
            //service.AddHttpClient<IQEntregableBMueblesProxy, QEntregableBMueblesProxy>();
            //service.AddHttpClient<IQEContratoBMueblesProxy, QEContratoBMueblesProxy>();
            //service.AddHttpClient<IQLCedulaBMueblesProxy, QLCedulaBMueblesProxy>();
            //service.AddHttpClient<IQLEntregableBMueblesProxy, QLEntregableBMueblesProxy>();
            //service.AddHttpClient<IQIncidenciaBMueblesProxy, QIncidenciaBMueblesProxy>();

            //service.AddScoped<ICedulaBMueblesProcedure, CedulaBMueblesProcedure>();
            //service.AddScoped<IQBMueblesEntregableProcedure, QBMueblesEntregableProcedure>();
            //service.AddScoped<ICEntregableBMueblesProcedure, CEntregableBMueblesProcedure>();
            //service.AddScoped<ICCFDIProcedure, CCFDIProcedure>();
            //service.AddScoped<ICuestionarioBMueblesProcedure, CuestionarioBMueblesProcedure>();
            //service.AddHttpClient<IQCuestionarioBMueblesProxy, QCuestionarioBMueblesProxy>();


            return service;
        }

        public static IServiceCollection AddProxiesBMueblesCommands(this IServiceCollection service, IConfiguration configuration)
        {
            //Servicio de Comedor
            service.AddHttpClient<ICFirmanteBMueblesProxy, CFirmanteBMueblesProxy>();
            //service.AddHttpClient<ICCedulaBMueblesProxy, CCedulaBMueblesProxy>();
            //service.AddHttpClient<ICRespuestaBMueblesProxy, CRespuestaBMueblesProxy>();
            //service.AddHttpClient<ICRepositorioBMueblesProxy, CRepositorioBMueblesProxy>();
            //service.AddHttpClient<ICCFDIBMueblesProxy, CCFDIBMueblesProxy>();
            //service.AddHttpClient<ICContratoBMueblesProxy, CContratoBMueblesProxy>();
            //service.AddHttpClient<ICSContratoBMueblesProxy, CSContratoBMueblesProxy>();
            //service.AddHttpClient<ICConvenioBMueblesProxy, CConvenioBMueblesProxy>();
            //service.AddHttpClient<ICEntregableBMueblesProxy, CEntregableBMueblesProxy>();
            //service.AddHttpClient<ICEContratoBMueblesProxy, CEContratoBMueblesProxy>();
            //service.AddHttpClient<ICLCedulaBMueblesProxy, CLCedulaBMueblesProxy>();
            //service.AddHttpClient<ICLEntregableBMueblesProxy, CLEntregableBMueblesProxy>();
            //service.AddHttpClient<ICIncidenciaBMueblesProxy, CIncidenciaBMueblesProxy>();
            ////service.AddHttpClient<ICOficioComedorProxy, COficioComedorProxy>();

            return service;
        }
    }
}
