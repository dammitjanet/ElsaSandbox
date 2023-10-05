using Elsa.Client.Models;
using ElsaDashboard.Shared.Rpc;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Client.Extended.Services
{
    public interface IWorkflowInstancesApi
    {
        [Get("/v1/workflow-instances/{id}")]
        Task<WorkflowInstance?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

        [Get("/v1/workflow-instances")]
        Task<PagedList<WorkflowInstanceSummary>> ListAsync(
            int? page = default,
            int? pageSize = default,
            [AliasAs("workflow")] string? workflowDefinitionId = default,
            [AliasAs("status")] WorkflowStatus? workflowStatus = default,
            WorkflowInstanceOrderBy? orderBy = default,
            string? searchTerm = default,
            CancellationToken cancellationToken = default);

        [Delete("/v1/workflow-instances/{id}")]
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);

        [Delete("/v1/workflow-instances/bulk")]
        Task BulkDeleteAsync([Body] BulkDeleteWorkflowInstancesRequest instancesRequest, CancellationToken cancellationToken = default);

        [Post("/v1/workflow-instances/{id}/retry")]
        Task RetryAsync(string id, RetryWorkflowRequest request, CancellationToken cancellationToken = default);

        [Post("/v1/workflow-instances/bulk/retry")]
        Task BulkRetryAsync([Body] BulkRetryWorkflowInstancesRequest instancesRequest, CancellationToken cancellationToken = default);

        #region extended

        #region endpoint exists but has no Elsa.Client implementation

        #endregion

        #region added

        [Get("/v1/workflow-instances/correlation/{correlationId}")]
        Task<WorkflowInstance?> GetByCorrelationIdAsync(string correlationId, CancellationToken cancellationToken = default);

        [Get("/v1/workflow-instances/detail")]
        Task<PagedList<WorkflowInstance>> ListDetailedAsync(
            int? page = default,
            int? pageSize = default,
            [AliasAs("workflow")] string? workflowDefinitionId = default,
            [AliasAs("status")] WorkflowStatus? workflowStatus = default,
            WorkflowInstanceOrderBy? orderBy = default,
            string? searchTerm = default,
            CancellationToken cancellationToken = default);

        #endregion

        #endregion
    }
}