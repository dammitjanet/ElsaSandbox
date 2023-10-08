using Elsa.Client.Models;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Client.Extended.Services
{
    public interface IUserTasksApi
    {
        #region extended

        #region added

        [Post("/v1/user-task/execute")]
        Task<bool?> ExecuteUserActionAsync([Body] TriggerUserActionRequest context, CancellationToken cancellationToken = default);

        [Post("/v1/user-task/dispatch")]
        Task<bool?> DisaptchUserActionAsync([Body] TriggerUserActionRequest context, CancellationToken cancellationToken = default);

        #endregion

        #endregion
    }
}