using Elsa.Client.Services;
using Elsa.Client.Webhooks.Services;

namespace Elsa.Client.Extended.Services
{
    public class ElsaExtendedClient : IElsaExtendedClient
    {
        public ElsaExtendedClient(
            IActivitiesApi activities,
            IWorkflowDefinitionsApi workflowDefinitions,
            IWorkflowRegistryApi workflowRegistry,
            IWorkflowInstancesApi workflowInstances,
            IWorkflowsApi workflows,
            IWebhookDefinitionsApi webhookDefinitions,
            IScriptingApi scripting,
            IUserTasksApi userTasks
            )
        {
            Activities = activities;
            WorkflowDefinitions = workflowDefinitions;
            WorkflowRegistry = workflowRegistry;
            WorkflowInstances = workflowInstances;
            WebhookDefinitions = webhookDefinitions;
            Scripting = scripting;
            Workflows = workflows;
            UserTasks = userTasks;
        }

        public IActivitiesApi Activities { get; }
        public IWorkflowDefinitionsApi WorkflowDefinitions { get; }
        public IWorkflowRegistryApi WorkflowRegistry { get; }
        public IWorkflowInstancesApi WorkflowInstances { get; }
        public IWorkflowsApi Workflows { get; }
        public IWebhookDefinitionsApi WebhookDefinitions { get; }
        public IScriptingApi Scripting { get; }
        public IUserTasksApi UserTasks { get; }
    }
}