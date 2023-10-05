using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Elsa.Client.Models
{
    [DataContract]
    public class TriggerUserActionRequest
    {
        [DataMember(Order = 1)] public string Action { get; set; } = default!;
        [DataMember(Order = 2)] public string? WorkflowInstanceId { get; set; }
        [DataMember(Order = 3)] public string? CorrelationId { get; set; }

        // realistically we should be providing this as an optional parameter
        //[DataMember(Order = 4)] public string? ActivityId { get; set; }
    }
}