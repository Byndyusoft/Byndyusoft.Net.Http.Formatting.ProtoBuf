using System.IO;
using System.Net.Http.Formatting.Protobuf;
using System.Threading.Tasks;
using ProtoBuf.Meta;

namespace System.Net.Http.Formatting
{
    /// <summary>
    ///     <see cref="MediaTypeFormatter" /> class to handle ProtoBuf.
    /// </summary>
    public class ProtoBufMediaTypeFormatter : MediaTypeFormatter
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProtoBufMediaTypeFormatter" /> class.
        /// </summary>
        public ProtoBufMediaTypeFormatter() : this(ProtoBufConstants.DefaultTypeModel)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProtoBufMediaTypeFormatter" /> class.
        /// </summary>
        /// <param name="formatter">The <see cref="ProtoBufMediaTypeFormatter" /> instance to copy settings from.</param>
        protected internal ProtoBufMediaTypeFormatter(ProtoBufMediaTypeFormatter formatter)
            : base(formatter)
        {
            Model = formatter.Model;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProtoBufMediaTypeFormatter" /> class.
        /// </summary>
        /// <param name="model">Options for running serialization.</param>
        public ProtoBufMediaTypeFormatter(TypeModel model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            SupportedMediaTypes.Add(ProtoBufConstants.MediaTypeHeaders.ApplicationProtoBuf);
            SupportedMediaTypes.Add(ProtoBufConstants.MediaTypeHeaders.ApplicationXProtoBuf);
        }

        /// <summary>
        ///     >Provides protobuf serialization support for a number of types.
        /// </summary>
        public TypeModel Model { get; }

        /// <inheritdoc />
        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content,
            IFormatterLogger formatterLogger)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (readStream is null) throw new ArgumentNullException(nameof(readStream));

            if (content.Headers.ContentLength != null)
            {
                if (content.Headers.ContentLength == 0)
                    return null;

                return await ReadDirectStreamAsync(type, readStream);
            }

            return await ReadBufferedStreamAsync(type, readStream);
        }

        /// <inheritdoc />
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
            TransportContext transportContext)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (writeStream is null) throw new ArgumentNullException(nameof(writeStream));

            if (value is null)
            {
                content.Headers.ContentLength = 0;
                return Task.CompletedTask;
            }

            using (var measureState = Model.Measure(value))
            {
                measureState.Serialize(writeStream);
                content.Headers.ContentLength = measureState.Length;
            }

            return Task.CompletedTask;
        }


        /// <inheritdoc />
        public override bool CanReadType(Type type)
        {
            return CanSerialize(type);
        }

        /// <inheritdoc />
        public override bool CanWriteType(Type type)
        {
            return CanSerialize(type);
        }

        private bool CanSerialize(Type type)
        {
            return Model.CanSerialize(type);
        }

        private Task<object> ReadDirectStreamAsync(Type type, Stream stream)
        {
            var model = Model.Deserialize(stream, null, type);
            return Task.FromResult(model);
        }

        private async Task<object> ReadBufferedStreamAsync(Type type, Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream).ConfigureAwait(false);

                if (memoryStream.Length == 0) return default;

                memoryStream.Position = 0;
                return Model.Deserialize(memoryStream, null, type);
            }
        }
    }
}