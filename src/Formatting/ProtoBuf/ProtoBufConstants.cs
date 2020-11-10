using System.Net.Http.Headers;
using ProtoBuf.Meta;

namespace System.Net.Http.Formatting.Protobuf
{
    public class ProtoBufConstants
    {
        public static readonly MediaTypeWithQualityHeaderValue DefaultMediaTypeHeader =
            MediaTypeHeaders.ApplicationProtoBuf;

        public static readonly TypeModel DefaultTypeModel = RuntimeTypeModel.Default;

        public static class MediaTypes
        {
            public const string ApplicationProtoBuf = "application/protobuf";
            public const string ApplicationXProtoBuf = "application/x-protobuf";
        }

        public static class MediaTypeHeaders
        {
            public static readonly MediaTypeWithQualityHeaderValue ApplicationProtoBuf =
                new MediaTypeWithQualityHeaderValue(MediaTypes.ApplicationProtoBuf);

            public static readonly MediaTypeWithQualityHeaderValue ApplicationXProtoBuf =
                new MediaTypeWithQualityHeaderValue(MediaTypes.ApplicationXProtoBuf);
        }
    }
}