using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http.Tests.Unit
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        public static readonly FakeHttpMessageHandler Instance = new FakeHttpMessageHandler();

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK) {RequestMessage = request};
            return Task.FromResult(response);
        }
    }
}