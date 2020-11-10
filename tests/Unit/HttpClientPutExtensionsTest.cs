using System.Net.Http.Formatting;
using System.Net.Http.Tests.Models;
using System.Threading;
using System.Threading.Tasks;
using ProtoBuf.Meta;
using Xunit;

namespace System.Net.Http.Tests.Unit
{
    public class HttpClientPutExtensionsTest
    {
        private readonly HttpClient _client;
        private readonly TypeModel _model;
        private readonly string _uri = "http://localhost/";

        public HttpClientPutExtensionsTest()
        {
            _model = RuntimeTypeModel.Default;
            _client = new HttpClient(FakeHttpMessageHandler.Instance);
        }

        [Fact]
        public async Task PutAsProtoBufAsync_String_WhenClientIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ((HttpClient) null).PutAsProtoBufAsync(_uri, new SimpleType()));
            Assert.Equal("client", exception.ParamName);
        }

        [Fact]
        public async Task PutAsProtoBufAsync_String_WhenUriIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _client.PutAsProtoBufAsync((string) null, new SimpleType()));
            Assert.Equal(
                "An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.",
                exception.Message);
        }

        [Fact]
        public async Task PutAsProtoBufAsync_String_WhenModelIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _client.PutAsProtoBufAsync(_uri, new SimpleType(), null, CancellationToken.None));
            Assert.Equal("model", exception.ParamName);
        }

        [Fact]
        public async Task PutAsProtoBufAsync_String_UsesProtoBufMediaTypeFormatter()
        {
            var response = await _client.PutAsProtoBufAsync(_uri, new SimpleType(), _model);

            var content = Assert.IsType<ObjectContent<SimpleType>>(response.RequestMessage.Content);
            var formatter = Assert.IsType<ProtoBufMediaTypeFormatter>(content.Formatter);
            Assert.Same(_model, formatter.Model);
        }

        [Fact]
        public async Task PutAsProtoBufAsync_Uri_WhenClientIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ((HttpClient) null).PutAsProtoBufAsync(new Uri(_uri), new SimpleType()));
            Assert.Equal("client", exception.ParamName);
        }

        [Fact]
        public async Task PutAsProtoBufAsync_Uri_WhenModelIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _client.PutAsProtoBufAsync(new Uri(_uri), new SimpleType(), null, CancellationToken.None));
            Assert.Equal("model", exception.ParamName);
        }

        [Fact]
        public async Task PutAsProtoBufAsync_Uri_WhenUriIsNull_ThrowsException()
        {
            var exception =
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    _client.PutAsProtoBufAsync((Uri) null, new SimpleType()));
            Assert.Equal(
                "An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.",
                exception.Message);
        }

        [Fact]
        public async Task PutAsProtoBufAsync_Uri_UsesProtoBufMediaTypeFormatter()
        {
            var response = await _client.PutAsProtoBufAsync(new Uri(_uri), new SimpleType(), _model);

            var content = Assert.IsType<ObjectContent<SimpleType>>(response.RequestMessage.Content);
            var formatter = Assert.IsType<ProtoBufMediaTypeFormatter>(content.Formatter);
            Assert.Same(_model, formatter.Model);
        }
    }
}