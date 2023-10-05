using Elsa.Models;
using Elsa.Serialization;
using Elsa.Server.Api.Models;
using Elsa.Server.Api.Services;
using Elsa.Server.Api.Swagger.Examples;
using Elsa.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Server.Api.Endpoints.WorkflowRegistry
{
    [ApiController]
    [ApiVersion("1")]
    [Route("v{apiVersion:apiVersion}/workflow-registry/by-tag/{tag}/{versionOptions}")]
    [Produces("application/json")]
    public class GetByTag : Controller
    {
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IWorkflowBlueprintMapper _workflowBlueprintMapper;
        private readonly IContentSerializer _contentSerializer;

        public GetByTag(IWorkflowRegistry workflowRegistry, IWorkflowBlueprintMapper workflowBlueprintMapper, IContentSerializer contentSerializer)
        {
            _workflowRegistry = workflowRegistry;
            _workflowBlueprintMapper = workflowBlueprintMapper;
            _contentSerializer = contentSerializer;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<WorkflowBlueprintModel>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(WorkflowBlueprintModelExample))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Returns a single workflow blueprint.",
            Description = "Returns a single workflow blueprint by Tag. When no version options are specified, the latest version is returned.",
            OperationId = "WorkflowBlueprints.GetByTag",
            Tags = new[] { "WorkflowBlueprints" })
        ]
        public async Task<ActionResult<WorkflowBlueprintModel>> Handle(string tag, VersionOptions? versionOptions, CancellationToken cancellationToken = default)
        {
            var workflowBlueprint = await _workflowRegistry.FindByTagAsync(tag, versionOptions ?? VersionOptions.Latest, default, cancellationToken);

            if (workflowBlueprint == null)
                return NotFound();

            var model = await _workflowBlueprintMapper.MapAsync(workflowBlueprint, cancellationToken);

            // Filter out activities from composite activities.
            model.Activities = model.Activities.Where(x => x.ParentId == null || x.ParentId == workflowBlueprint.Id).ToList();

            return Json(model, _contentSerializer.GetSettings());
        }
    }
}