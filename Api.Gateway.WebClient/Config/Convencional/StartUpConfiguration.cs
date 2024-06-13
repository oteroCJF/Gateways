using Api.Gateway.Proxies.Convencional.Contratos.Commands;
using Api.Gateway.Proxies.Convencional.Contratos.Queries;
using Api.Gateway.Proxies.Convencional.Convenios.Commands;
using Api.Gateway.Proxies.Convencional.Convenios.Queries;
using Api.Gateway.Proxies.Convencional.EntregablesContrato.Commands;
using Api.Gateway.Proxies.Convencional.EntregablesContrato.Queries;
using Api.Gateway.Proxies.Convencional.ServiciosContrato.Commands;
using Api.Gateway.Proxies.Convencional.ServiciosContrato.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Gateway.WebClient.Config.Convencional
{
    public static class StartUpConfiguration
    {
        public static IServiceCollection AddProxiesConvencionalQueries(this IServiceCollection service, IConfiguration configuration)
        {
            //Servicio de Convencional
            service.AddHttpClient<IQContratoConvencionalProxy, QContratoConvencionalProxy>();
            service.AddHttpClient<IQSContratoConvencionalProxy, QSContratoConvencionalProxy>();
            service.AddHttpClient<IQConvenioConvencionalProxy, QConvenioConvencionalProxy>();
            service.AddHttpClient<IQEContratoConvencionalProxy, QEContratoConvencionalProxy>();

            return service;
        }

        public static IServiceCollection AddProxiesConvencionalCommands(this IServiceCollection service, IConfiguration configuration)
        {
            //Servicio de Convencional
            service.AddHttpClient<ICContratoConvencionalProxy, CContratoConvencionalProxy>();
            service.AddHttpClient<ICSContratoConvencionalProxy, CSContratoConvencionalProxy>();
            service.AddHttpClient<ICConvenioConvencionalProxy, CConvenioConvencionalProxy>();
            service.AddHttpClient<ICEContratoConvencionalProxy, CEContratoConvencionalProxy>();

            return service;
        }
    }
}
