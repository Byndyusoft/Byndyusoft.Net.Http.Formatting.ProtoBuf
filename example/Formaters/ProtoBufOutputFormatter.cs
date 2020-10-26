namespace Byndyusoft.Net.Http.Formatting.ProtoBuf.Formaters
{
    using System;
    using System.IO;
    using System.Net.Http.Formatting;
    using System.Threading.Tasks;
    using global::ProtoBuf.Meta;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Net.Http.Headers;

    public class ProtoBufOutputFormatter : OutputFormatter
    {
        private readonly TypeModel _model;

        public ProtoBufOutputFormatter()
        {
            _model = RuntimeTypeModel.Default;

            SupportedMediaTypes.Add(new MediaTypeHeaderValue(ProtoBufMediaTypes.ApplicationProtoBuf));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(ProtoBufMediaTypes.ApplicationXProtoBuf));
        }

        protected override bool CanWriteType(Type type)
        {
            return base.CanWriteType(type) && _model.CanSerialize(type);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (context.Object != null)
            {
                await using var stream = new MemoryStream();
                _model.Serialize(stream, context.Object);
                stream.Position = 0;
                await stream.CopyToAsync(context.HttpContext.Response.Body).ConfigureAwait(false);
            }
        }
    }
}