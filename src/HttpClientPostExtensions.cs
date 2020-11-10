using System.ComponentModel;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using ProtoBuf.Meta;

namespace System.Net.Http
{
    /// <summary>
    ///     Extension methods that aid in making formatted POST requests using <see cref="HttpClient" />.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class HttpClientPostExtensions
    {
        /// <summary>
        ///     Sends a POST request as an asynchronous operation to the specified Uri with the given <paramref name="value" />
        ///     serialized as ProtoBuf.
        /// </summary>
        /// <remarks>
        ///     This method uses a default instance of <see cref="ProtoBufMediaTypeFormatter" />.
        /// </remarks>
        /// <typeparam name="T">The type of <paramref name="client" />.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsProtoBufAsync<T>(
            this HttpClient client,
            string requestUri,
            T value)
        {
            return client.PostAsProtoBufAsync(requestUri, value, CancellationToken.None);
        }

        /// <summary>
        ///     Sends a POST request as an asynchronous operation to the specified Uri with the given <paramref name="value" />
        ///     serialized as ProtoBuf.
        /// </summary>
        /// <remarks>
        ///     This method uses a default instance of <see cref="ProtoBufMediaTypeFormatter" />.
        /// </remarks>
        /// <typeparam name="T">The type of <paramref name="client" />.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <param name="model">Provides protobuf serialization support for a number of types.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsProtoBufAsync<T>(
            this HttpClient client,
            string requestUri,
            T value,
            TypeModel model)
        {
            return client.PostAsProtoBufAsync(requestUri, value, model, CancellationToken.None);
        }

        /// <summary>
        ///     Sends a POST request as an asynchronous operation to the specified Uri with the given <paramref name="value" />
        ///     serialized as ProtoBuf.
        /// </summary>
        /// <remarks>
        ///     This method uses a default instance of <see cref="ProtoBufMediaTypeFormatter" />.
        /// </remarks>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsProtoBufAsync<T>(
            this HttpClient client,
            string requestUri,
            T value,
            CancellationToken cancellationToken)
        {
            return client.PostAsProtoBufAsync(requestUri, value, RuntimeTypeModel.Default, cancellationToken);
        }

        /// <summary>
        ///     Sends a POST request as an asynchronous operation to the specified Uri with the given <paramref name="value" />
        ///     serialized as ProtoBuf.
        /// </summary>
        /// <remarks>
        ///     This method uses a default instance of <see cref="ProtoBufMediaTypeFormatter" />.
        /// </remarks>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <param name="model">Provides protobuf serialization support for a number of types.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsProtoBufAsync<T>(
            this HttpClient client,
            string requestUri,
            T value,
            TypeModel model,
            CancellationToken cancellationToken)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            return client.PostAsync(requestUri, value, new ProtoBufMediaTypeFormatter(model), cancellationToken);
        }

        /// <summary>
        ///     Sends a POST request as an asynchronous operation to the specified Uri with the given <paramref name="value" />
        ///     serialized as ProtoBuf.
        /// </summary>
        /// <remarks>
        ///     This method uses a default instance of <see cref="ProtoBufMediaTypeFormatter" />.
        /// </remarks>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsProtoBufAsync<T>(
            this HttpClient client,
            Uri requestUri,
            T value)
        {
            return client.PostAsProtoBufAsync(requestUri, value, CancellationToken.None);
        }

        /// <summary>
        ///     Sends a POST request as an asynchronous operation to the specified Uri with the given <paramref name="value" />
        ///     serialized as ProtoBuf.
        /// </summary>
        /// <remarks>
        ///     This method uses a default instance of <see cref="ProtoBufMediaTypeFormatter" />.
        /// </remarks>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <param name="model">Provides protobuf serialization support for a number of types.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsProtoBufAsync<T>(
            this HttpClient client,
            Uri requestUri,
            T value,
            TypeModel model)
        {
            return client.PostAsProtoBufAsync(requestUri, value, model, CancellationToken.None);
        }

        /// <summary>
        ///     Sends a POST request as an asynchronous operation to the specified Uri with the given <paramref name="value" />
        ///     serialized as ProtoBuf.
        /// </summary>
        /// <remarks>
        ///     This method uses a default instance of <see cref="ProtoBufMediaTypeFormatter" />.
        /// </remarks>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsProtoBufAsync<T>(
            this HttpClient client,
            Uri requestUri,
            T value,
            CancellationToken cancellationToken)
        {
            return client.PostAsProtoBufAsync(requestUri, value, RuntimeTypeModel.Default, cancellationToken);
        }

        /// <summary>
        ///     Sends a POST request as an asynchronous operation to the specified Uri with the given <paramref name="value" />
        ///     serialized as ProtoBuf.
        /// </summary>
        /// <remarks>
        ///     This method uses a default instance of <see cref="ProtoBufMediaTypeFormatter" />.
        /// </remarks>
        /// <typeparam name="T">The type of <paramref name="value" />.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <param name="model">Provides protobuf serialization support for a number of types.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsProtoBufAsync<T>(
            this HttpClient client,
            Uri requestUri,
            T value,
            TypeModel model,
            CancellationToken cancellationToken)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            return client.PostAsync(requestUri, value, new ProtoBufMediaTypeFormatter(model), cancellationToken);
        }
    }
}