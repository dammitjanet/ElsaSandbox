using Elsa.Client.Services;
using Elsa.Client.Webhooks.Services;

namespace Elsa.Client.Extended.Services
{
    public interface IElsaExtendedClient
    {
        IActivitiesApi Activities { get; }

        IWorkflowDefinitionsApi WorkflowDefinitions { get; }

        IWorkflowRegistryApi WorkflowRegistry { get; }

        IWorkflowInstancesApi WorkflowInstances { get; }

        IWorkflowsApi Workflows { get; }

        IWebhookDefinitionsApi WebhookDefinitions { get; }

        IScriptingApi Scripting { get; }

        IUserTasksApi UserTasks { get; }
    }
}