using Api.Gateway.Proxies.Celular.Contratos.Commands;
using Api.Gateway.Proxies.Celular.Contratos.Queries;
using Api.Gateway.Proxies.Celular.Convenios.Commands;
using Api.Gateway.Proxies.Celular.Convenios.Queries;
using Api.Gateway.Proxies.Celular.EntregablesContrato.Commands;
using Api.Gateway.Proxies.Celular.EntregablesContrato.Queries;
using Api.Gateway.Proxies.Celular.ServiciosContrato.Commands;
using Api.Gateway.Proxies.Celular.ServiciosContrato.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Gateway.WebClient.Config.Celular
{
    public static class StartUpConfiguration
    {
        public static IServiceCollection AddProxiesCelularQueries(this IServiceCollection service, IConfiguration configuration)
        {
            //Servicio de Celular
            service.AddHttpClient<IQContratoCelularProxy, QContratoCelularProxy>();
            service.AddHttpClient<IQSContratoCelularProxy, QSContratoCelularProxy>();
            service.AddHttpClient<IQConvenioCelularProxy, QConvenioCelularProxy>();
            service.AddHttpClient<IQEContratoCelularProxy, QEContratoCelularProxy>();

            return service;
        }

        public static IServiceCollection AddProxiesCelularCommands(this IServiceCollection service, IConfiguration configuration)
        {
            //Servicio de Celular
            service.AddHttpClient<ICContratoCelularProxy, CContratoCelularProxy>();
            service.AddHttpClient<ICSContratoCelularProxy, CSContratoCelularProxy>();
            service.AddHttpClient<ICConvenioCelularProxy, CConvenioCelularProxy>();
            service.AddHttpClient<ICEContratoCelularProxy, CEContratoCelularProxy>();

            return service;
        }
    }
}
