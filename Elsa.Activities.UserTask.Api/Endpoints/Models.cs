using Elsa.Services.Models;
using System.Collections.Generic;

namespace Elsa.Activities.UserTask.Api.Endpoints
{
    public record ExecuteUserActionRequestModel(string Action, string WorkflowInstanceId = default, string CorrelationId = default);

    public record DispatchUserActionRequestModel(string Action, string WorkflowInstanceId = default, string CorrelationId = default);

    public record TriggerUserActionResponse
    {
        // Need this for Swagger.
        public TriggerUserActionResponse()
        {
        }

        public TriggerUserActionResponse(ICollection<CollectedWorkflow> startedWorkflows)
        {
            StartedWorkflows = startedWorkflows;
        }

        public ICollection<CollectedWorkflow> StartedWorkflows { get; init; } = default!;
    }
}