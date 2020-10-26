namespace System.Net.Http.Models
{
    using ProtoBuf;

    [ProtoContract]
    public class ComplexType
    {
        [ProtoMember(1)]
        public SimpleType Inner { get; set; }
    }
}