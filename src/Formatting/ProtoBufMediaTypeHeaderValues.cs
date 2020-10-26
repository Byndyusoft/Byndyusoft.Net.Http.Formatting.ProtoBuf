namespace System.Net.Http.Formatting
{
    using Headers;

    public static class ProtoBufMediaTypeHeaderValues
    {
        public static readonly MediaTypeWithQualityHeaderValue ApplicationProtoBuf = new MediaTypeWithQualityHeaderValue(ProtoBufMediaTypes.ApplicationProtoBuf);
        public static readonly MediaTypeWithQualityHeaderValue ApplicationXProtoBuf = new MediaTypeWithQualityHeaderValue(ProtoBufMediaTypes.ApplicationXProtoBuf);
    }
}