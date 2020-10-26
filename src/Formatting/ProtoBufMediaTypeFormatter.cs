namespace System.Net.Http.Formatting
{
    using System;
    using Http;
    using IO;
    using Net;
    using Threading.Tasks;
    using Headers;
    using ProtoBuf.Meta;
    
    /// <summary>
    /// <see cref="MediaTypeFormatter"/> class to handle ProtoBuf.
    /// </summary>
    public class ProtoBufMediaTypeFormatter : MediaTypeFormatter
    {
        public static MediaTypeWithQualityHeaderValue DefaultMediaType => ProtoBufMediaTypeHeaderValues.ApplicationProtoBuf;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProtoBufMediaTypeFormatter"/> class.
        /// </summary>
        public ProtoBufMediaTypeFormatter() : this(RuntimeTypeModel.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProtoBufMediaTypeFormatter"/> class.
        /// </summary>
        /// <param name="formatter">The <see cref="ProtoBufMediaTypeFormatter"/> instance to copy settings from.</param>
        protected internal ProtoBufMediaTypeFormatter(ProtoBufMediaTypeFormatter formatter)
            : base(formatter)
        {
            Model = formatter.Model;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProtoBufMediaTypeFormatter"/> class.
        /// </summary>
        /// <param name="model">Options for running serialization.</param>
        public ProtoBufMediaTypeFormatter(TypeModel model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            SupportedMediaTypes.Add(ProtoBufMediaTypeHeaderValues.ApplicationProtoBuf);
            SupportedMediaTypes.Add(ProtoBufMediaTypeHeaderValues.ApplicationXProtoBuf);
        }

        /// <summary>
        /// >Provides protobuf serialization support for a number of types.
        /// </summary>
        public TypeModel Model { get; }

        /// <inheritdoc />
        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (readStream is null) throw new ArgumentNullException(nameof(readStream));

            using (var memoryStream = new MemoryStream())
            {
                await readStream.CopyToAsync(memoryStream);

                if (memoryStream.Length == 0)
                {
                    return default;
                }

                memoryStream.Position = 0;
                return Model.Deserialize(memoryStream, null, type);
            }
        }

        /// <inheritdoc />
        public override async Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (writeStream is null) throw new ArgumentNullException(nameof(writeStream));

            if (value is null)
            {
                return;
            }

            using (var memoryStream = new MemoryStream())
            {
                Model.Serialize(memoryStream, value);
                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(writeStream);
            }
        }

        /// <inheritdoc />
        public override bool CanReadType(Type type) => CanSerialize(type);

        /// <inheritdoc />
        public override bool CanWriteType(Type type) => CanSerialize(type);

        private bool CanSerialize(Type type)
        {
            return Model.CanSerialize(type);
        }
    }
}
