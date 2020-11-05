namespace Byndyusoft.Net.Http.Formatting.ProtoBuf.Formaters
{
    using System;
    using System.IO;
    using System.Net.Http.Formatting;
    using System.Threading.Tasks;
    using global::ProtoBuf.Meta;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Net.Http.Headers;

    public class ProtoBufInputFormatter : InputFormatter
    {
        private readonly TypeModel _model;

        public ProtoBufInputFormatter()
        {
            _model = RuntimeTypeModel.Default;

            SupportedMediaTypes.Add(new MediaTypeHeaderValue(ProtoBufMediaTypes.ApplicationProtoBuf));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(ProtoBufMediaTypes.ApplicationXProtoBuf));
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var request = context.HttpContext.Request;
            object model = null;

            await using var stream = new MemoryStream();
            await request.Body.CopyToAsync(stream).ConfigureAwait(false);
            
            if (stream.Length > 0)
            {
                stream.Position = 0;
                model = _model.Deserialize(stream, null, context.ModelType, stream.Length);
            }

            if (model == null && context.TreatEmptyInputAsDefaultValue)
            {
                return await InputFormatterResult.NoValueAsync();
            }

            return await InputFormatterResult.SuccessAsync(model);
        }

        protected override bool CanReadType(Type type)
        {
            return base.CanReadType(type) && _model.CanSerialize(type);
        }
    }
}