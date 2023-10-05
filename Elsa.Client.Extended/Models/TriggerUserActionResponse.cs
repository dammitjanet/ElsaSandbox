using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Elsa.Client.Models
{
    [DataContract]
    public class TriggerUserActionResponse
    {
        [DataMember(Order = 1)] public ICollection<CollectedWorkflow> StartedWorkflows { get; set; } = default!;
    }
}