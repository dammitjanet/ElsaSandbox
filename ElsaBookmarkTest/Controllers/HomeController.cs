﻿using Elsa.Activities.UserTask.Contracts;
using Elsa.Activities.UserTask.Models;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Providers.Workflows;
using Elsa.Services;
using Elsa.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElsaBookmarkTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IUserTaskService _userTaskService;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IWorkflowLaunchpad _workflowLaunchpad;
        private readonly IEnumerable<IWorkflowProvider> _workflowProviders;

        public HomeController(
            ILogger<HomeController> logger,
            IWorkflowRegistry workflowRegistry,
            IUserTaskService userTaskService,
            IWorkflowInstanceStore workflowInstanceStore,
            IWorkflowLaunchpad workflowLaunchpad,
            IEnumerable<IWorkflowProvider> workflowProviders)
        {
            _logger = logger;
            _workflowRegistry = workflowRegistry;
            _userTaskService = userTaskService;
            _workflowInstanceStore = workflowInstanceStore;
            _workflowLaunchpad = workflowLaunchpad;
            _workflowProviders = workflowProviders;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            List<WorkflowInstance> instances = new();
            List<IWorkflowBlueprint> blueprints = new();
            foreach (var provider in _workflowProviders.ToList())
            {
                var workflowBlueprints = await provider.ListAsync(VersionOptions.Published).ToListAsync();
                blueprints.AddRange(workflowBlueprints);
            }

            foreach (var blueprint in blueprints)
            {
                instances.AddRange(await _workflowInstanceStore.FindManyAsync(new WorkflowDefinitionIdSpecification(blueprint.Id), cancellationToken: ct));
            }
            return View(instances);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkflowItem(string workflowName, CancellationToken ct)
        {
            var workflowBlueprint = await _workflowRegistry.FindByNameAsync(workflowName, VersionOptions.LatestOrPublished, default, ct);
            if (workflowBlueprint != null)
            {
                string correlationId = Guid.NewGuid().ToString("N");

                var startableWorkflow = await _workflowLaunchpad.FindStartableWorkflowAsync(workflowBlueprint, correlationId: correlationId, cancellationToken: ct);
                if (startableWorkflow != null)
                {
                    var input = new WorkflowInput();
                    var result = await _workflowLaunchpad.ExecuteStartableWorkflowAsync(startableWorkflow, input, ct);

                    if (result == null || result.WorkflowInstance == null)
                    {
                        throw new Exception($"Unable to create workflow instance of type '{workflowName}'");
                    }
                }
                else
                {
                    throw new Exception($"Unable to find startable workflow with name '{workflowName}'");
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
            var instance = await _workflowInstanceStore.FindAsync(new WorkflowInstanceIdSpecification(instanceId), ct);
            if (instance != null && instance.CorrelationId.Equals(correlationId))
            {
                TriggerUserAction userAction = new(action, instance.Id, instance.CorrelationId);
                var userTasks = await _userTaskService.ExecuteUserActionsAsync(userAction, ct);

                if (userTasks == null)
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