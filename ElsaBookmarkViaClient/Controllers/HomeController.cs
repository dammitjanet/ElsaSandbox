using Elsa.Client.Models;
using Microsoft.AspNetCore.Mvc;
using Elsa.Client.Extended.Services;

namespace ElsaBookmarkViaClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IElsaExtendedClient _elsaClient;

        public HomeController(
            ILogger<HomeController> logger,
            IElsaExtendedClient elsaClient)
        {
            _logger = logger;
            _elsaClient = elsaClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            List<WorkflowInstance> instances = new();
            var providers = await _elsaClient.WorkflowRegistry.ListProvidersAsync();
            List<WorkflowBlueprintSummary> blueprints = new();
            foreach (var provider in providers)
            {
                var workflowBlueprints = await _elsaClient.WorkflowRegistry.ListByProviderAsync(provider.Name, versionOptions: VersionOptions.Published);
                blueprints.AddRange(workflowBlueprints.Items);
            }
            foreach (var blueprint in blueprints)
            {
                var pagedList = await _elsaClient.WorkflowInstances.ListDetailedAsync(workflowDefinitionId: blueprint.Id, cancellationToken: ct);
                instances.AddRange(pagedList.Items);
            }
            return View(instances);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkflowItem(string workflowName, CancellationToken ct)
        {
            var workflowBlueprint = await _elsaClient.WorkflowRegistry.GetByNameAsync(workflowName, VersionOptions.LatestOrPublished, ct);
            if (workflowBlueprint != null)
            {
                var request = new ExecuteWorkflowDefinitionRequest()
                {
                    CorrelationId = Guid.NewGuid().ToString("N")
                };

                var result = await _elsaClient.Workflows.ExecuteWorkflowAsync(workflowBlueprint.Id, request, ct);
                if (result == null || result.WorkflowInstance == null)
                {
                    throw new Exception($"Unable to create workflow instance of type '{workflowName}'");
                }
            }
            else
            {
                throw new Exception($"Unable to find workflow blueprint with name '{workflowName}'");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SendWorkflowAction(string instanceId, string correlationId, string activityId, string action, CancellationToken ct)
        {
            var instance = await _elsaClient.WorkflowInstances.GetByIdAsync(instanceId, ct);
            if (instance != null && instance.CorrelationId!.Equals(correlationId))
            {
                TriggerUserActionRequest userAction = new()
                {
                    Action = action,
                    WorkflowInstanceId = instance.Id,
                    CorrelationId = instance.CorrelationId
                };
                var resultingWorkflows = await _elsaClient.UserTasks.ExecuteUserActionAsync(userAction, ct);

                if (resultingWorkflows == null)
                {
                    throw new Exception($"Unable to find execute user task {action} on instance '{instanceId}'");
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}