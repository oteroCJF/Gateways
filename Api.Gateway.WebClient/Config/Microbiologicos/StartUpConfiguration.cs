using Api.Gateway.Proxies.Microbiologicos.Contratos.Commands;
using Api.Gateway.Proxies.Microbiologicos.Contratos.Queries;
using Api.Gateway.Proxies.Microbiologicos.Convenios.Commands;
using Api.Gateway.Proxies.Microbiologicos.Convenios.Queries;
using Api.Gateway.Proxies.Microbiologicos.EntregablesContrato.Commands;
using Api.Gateway.Proxies.Microbiologicos.EntregablesContrato.Queries;
using Api.Gateway.Proxies.Microbiologicos.ServiciosContrato.Commands;
using Api.Gateway.Proxies.Microbiologicos.ServiciosContrato.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Gateway.WebClient.Config.Microbiologicos
{
    public static class StartUpConfiguration
    {
        public static IServiceCollection AddProxiesMicrobiologicosQueries(this IServiceCollection service, IConfiguration configuration)
        {
            //Servicio de Microbiologicos
            service.AddHttpClient<IQContratoMicrobiologicosProxy, QContratoMicrobiologicosProxy>();
            service.AddHttpClient<IQSContratoMicrobiologicosProxy, QSContratoMicrobiologicosProxy>();
            service.AddHttpClient<IQConvenioMicrobiologicosProxy, QConvenioMicrobiologicosProxy>();
            service.AddHttpClient<IQEContratoMicrobiologicosProxy, QEContratoMicrobiologicosProxy>();

            return service;
        }

        public static IServiceCollection AddProxiesMicrobiologicosCommands(this IServiceCollection service, IConfiguration configuration)
        {
            //Servicio de Microbiologicos
            service.AddHttpClient<ICContratoMicrobiologicosProxy, CContratoMicrobiologicosProxy>();
            service.AddHttpClient<ICSContratoMicrobiologicosProxy, CSContratoMicrobiologicosProxy>();
            service.AddHttpClient<ICConvenioMicrobiologicosProxy, CConvenioMicrobiologicosProxy>();
            service.AddHttpClient<ICEContratoMicrobiologicosProxy, CEContratoMicrobiologicosProxy>();

            return service;
        }
    }
}
