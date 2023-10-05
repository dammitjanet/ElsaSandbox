using System.Runtime.Serialization;

namespace Elsa.Client.Models
{
    [DataContract]
    public class WorkflowProviderDescriptor
    {
        [DataMember(Order = 0)] public string Name { get; set; } = default!;
        [DataMember(Order = 1)] public string DisplayName { get; set; } = default!;
    }
}