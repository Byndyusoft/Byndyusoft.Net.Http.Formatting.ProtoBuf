namespace System.Net.Http.Models
{
    using IO;
    using ProtoBuf;

    [ProtoContract]
    public class SimpleType
    {
        [ProtoMember(1)]
        public int Property { get; set; }
        [ProtoMember(2)]
        public string Field;
        [ProtoMember(3)]
        public SeekOrigin Enum { get; set; }
        [ProtoMember(4)]
        public int? Nullable { get; set; }
        [ProtoMember(5)]
        public int[] Array { get; set; }
    }
}