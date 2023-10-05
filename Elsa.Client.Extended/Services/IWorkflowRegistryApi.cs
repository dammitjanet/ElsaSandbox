using Elsa.Client.Models;
using Refit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Client.Extended.Services
{
    public interface IWorkflowRegistryApi
    {
        [Get("/v1/workflow-registry/{id}/{versionOptions}")]
        Task<WorkflowBlueprint?> GetByIdAsync(string id, VersionOptions versionOptions, CancellationToken cancellationToken = default);

        [Get("/v1/workflow-registry")]
        Task<IEnumerable<WorkflowBlueprintSummary>> ListAsync(int? page = default, int? pageSize = default, VersionOptions? versionOptions = default, CancellationToken cancellationToken = default);

        #region extended

        #region endpoint exists but has no Elsa.Client implementation

        [Get("/v1/workflow-registry/by-definition-version-ids")]
        Task<List<WorkflowBlueprintSummary>> ListByDefinitionVersionIdsAsync(string ids, CancellationToken cancellationToken = default);

        [Get("/v1/workflow-providers")]
        Task<List<WorkflowProviderDescriptor>> ListProvidersAsync(CancellationToken cancellationToken = default);

        [Get("/v1/workflow-registry/by-provider/{providerName}")]
        Task<PagedList<WorkflowBlueprintSummary>> ListByProviderAsync(string providerName, int? page = default, int? pageSize = default, VersionOptions? versionOptions = default, CancellationToken cancellationToken = default);

        #endregion

        #region added

        [Get("/v1/workflow-registry/by-name/{name}/{versionOptions}")]
        Task<WorkflowBlueprint?> GetByNameAsync(string name, VersionOptions? versionOptions = default, CancellationToken cancellationToken = default);

        [Get("/v1/workflow-registry/by-tag/{tag}/{versionOptions}")]
        Task<WorkflowBlueprint?> GetByTagAsync(string tag, VersionOptions versionOptions, CancellationToken cancellationToken = default);

        #endregion

        #endregion
    }
}