using ProtoBuf;
using Xunit;

namespace System.Net.Http.Formatting.Models
{
    [ProtoContract]
    public class ComplexType
    {
        [ProtoMember(1)] public SimpleType Inner { get; set; }

        public static ComplexType Create()
        {
            return new ComplexType
            {
                Inner = SimpleType.Create()
            };
        }

        public void Verify()
        {
            Assert.NotNull(Inner);
            Inner.Verify();
        }
    }
}