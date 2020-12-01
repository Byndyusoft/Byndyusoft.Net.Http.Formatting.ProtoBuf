using System.IO;
using System.Net.Http.ProtoBuf;
using System.Threading;
using System.Threading.Tasks;
using ProtoBuf.Meta;

namespace System.Net.Http.Formatting
{
    /// <summary>
    ///     <see cref="MediaTypeFormatter" /> class to handle ProtoBuf.
    /// </summary>
    /// <see
    ///     href="https://github.com/protobuf-net/protobuf-net/blob/main/src/protobuf-net.AspNetCore/Formatters/ProtoInputFormatter.cs" />
    public class ProtoBufMediaTypeFormatter : MediaTypeFormatter
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProtoBufMediaTypeFormatter" /> class.
        /// </summary>
        public ProtoBufMediaTypeFormatter() : this(ProtoBufDefaults.TypeModel)
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
            SupportedMediaTypes.Add(ProtoBufDefaults.MediaTypeHeaders.ApplicationProtoBuf);
            SupportedMediaTypes.Add(ProtoBufDefaults.MediaTypeHeaders.ApplicationXProtoBuf);
        }

        /// <summary>
        ///     Provides protobuf serialization support for a number of types.
        /// </summary>
        public TypeModel Model { get; }

        /// <inheritdoc />
        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content,
            IFormatterLogger formatterLogger)
        {
            return ReadFromStreamAsync(type, readStream, content, formatterLogger, CancellationToken.None);
        }

        /// <inheritdoc />
        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content,
            IFormatterLogger formatterLogger,
            CancellationToken cancellationToken)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (readStream is null) throw new ArgumentNullException(nameof(readStream));
            if (content == null) throw new ArgumentNullException(nameof(content));

            return await content.ReadFromProtoBufAsync(type, Model, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
            TransportContext transportContext)
        {
            return WriteToStreamAsync(type, value, writeStream, content, transportContext, CancellationToken.None);
        }

        /// <inheritdoc />
        public override async Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
            TransportContext transportContext, CancellationToken cancellationToken)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (writeStream is null) throw new ArgumentNullException(nameof(writeStream));
            if (content == null) throw new ArgumentNullException(nameof(content));

            var protoBufContent = content as ProtoBufContent ?? ProtoBufContent.Create(value, type, Model);
            await protoBufContent.CopyToAsync(writeStream).ConfigureAwait(false);
            content.Headers.ContentLength = protoBufContent.Headers.ContentLength;
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
    }
}