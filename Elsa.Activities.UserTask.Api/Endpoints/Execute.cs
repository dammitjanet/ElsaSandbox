using Elsa.Activities.UserTask.Contracts;
using Elsa.Activities.UserTask.Models;
using Elsa.Server.Api.ActionFilters;
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

        public Execute(IUserTaskService userTaskService)
        {
            _userTaskService = userTaskService;
        }

        [HttpPost]
        [ElsaJsonFormatter]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
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

            return Json(result.Any());
        }
    }
}