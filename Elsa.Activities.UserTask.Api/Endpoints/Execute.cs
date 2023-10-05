using Elsa.Activities.UserTask.Contracts;
using Elsa.Activities.UserTask.Models;
using Elsa.Server.Api.ActionFilters;
using Elsa.Server.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Activities.UserTask.Api.Endpoints
{
    [ApiController]
    [ApiVersion("1")]
    [Route("v{apiVersion:apiVersion}/user-task/execute")]
    [Produces("application/json")]
    public class Execute : Controller
    {
        private readonly IUserTaskService _userTaskService;
        private readonly IEndpointContentSerializerSettingsProvider _serializerSettingsProvider;

        public Execute(IUserTaskService userTaskService, IEndpointContentSerializerSettingsProvider serializerSettingsProvider)
        {
            _userTaskService = userTaskService;
            _serializerSettingsProvider = serializerSettingsProvider;
        }

        [HttpPost]
        [ElsaJsonFormatter]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TriggerUserActionResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Executes the specified usertask.",
            Description = "Executes the specified usertask from the supplied request.",
            OperationId = "UserTasks.Execute",
            Tags = new[] { "UserTasks" })
        ]
        public async Task<IActionResult> Handle(ExecuteUserActionRequestModel request, CancellationToken cancellationToken = default)
        {
            TriggerUserAction userAction = new(request.Action, request.WorkflowInstanceId, request.CorrelationId);

            var result = await _userTaskService.ExecuteUserActionsAsync(userAction, cancellationToken);

            if (result == null)
            {
                return NotFound();
            }

            if (Response.HasStarted)
                return new EmptyResult();

            return Json(new TriggerUserActionResponse(result.ToList()), _serializerSettingsProvider.GetSettings());
        }
    }
}