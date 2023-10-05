using System.Runtime.Serialization;

namespace Elsa.Client.Models
{
    [DataContract]
    public class CollectedWorkflow
    {
        [DataMember(Order = 1)] public string WorkflowInstanceId { get; set; } = default!;
        [DataMember(Order = 2)] public WorkflowInstance? WorkflowInstance { get; set; }
        [DataMember(Order = 3)] public string? ActivityId { get; set; }
    }
}