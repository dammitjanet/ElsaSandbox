using Elsa.Client.Extended.Services;
using Elsa.Client.Extensions;
using Elsa.Client.Options;
using Elsa.Client.Services;
using Elsa.Client.Webhooks.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using IWorkflowInstancesApi = Elsa.Client.Extended.Services.IWorkflowInstancesApi;
using IWorkflowRegistryApi = Elsa.Client.Extended.Services.IWorkflowRegistryApi;

namespace Elsa.Client.Extended.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddElsaExtendedClient(this IServiceCollection services, Action<ElsaClientOptions>? configure = default, Func<HttpClient>? httpClientFactory = default)
        {
            if (configure != null)
                services.Configure(configure);

            var refitSettings = Client.Extensions.ServiceCollectionExtensions.CreateRefitSettings();

            services
                .AddApiClient<IActivitiesApi>(refitSettings, httpClientFactory)
                .AddApiClient<IWorkflowsApi>(refitSettings, httpClientFactory)
                .AddApiClient<IWorkflowDefinitionsApi>(refitSettings, httpClientFactory)
                .AddApiClient<IWorkflowRegistryApi>(refitSettings, httpClientFactory) // extended
                .AddApiClient<IWorkflowInstancesApi>(refitSettings, httpClientFactory) // extended
                .AddApiClient<IWebhookDefinitionsApi>(refitSettings, httpClientFactory)
                .AddApiClient<IScriptingApi>(refitSettings, httpClientFactory)
                .AddApiClient<IUserTasksApi>(refitSettings, httpClientFactory);
            return services
                .AddTransient<IElsaExtendedClient, ElsaExtendedClient>();
        }
    }
}