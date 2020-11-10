using ProtoBuf;

namespace System.Net.Http.Tests.Models
{
    [ProtoContract]
    public class ComplexType
    {
        [ProtoMember(1)] public SimpleType Inner { get; set; }
    }
}