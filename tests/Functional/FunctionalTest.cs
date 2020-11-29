using System.Net.Http.Formatting.Models;
using System.Net.Http.ProtoBuf;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace System.Net.Http.Formatting.Functional
{
    public class FunctionalTest : MvcTestFixture
    {
        private readonly ProtoBufMediaTypeFormatter _formatter;

        public FunctionalTest()
        {
            _formatter = new ProtoBufMediaTypeFormatter();
        }

        protected override void ConfigureHttpClient(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Add(ProtoBufDefaults.MediaTypeHeader);
        }

        protected override void ConfigureMvc(IMvcCoreBuilder builder)
        {
            builder.AddProtoBufNet(
                options => { options.Model = ProtoBufDefaults.TypeModel; });
        }

        [Fact]
        public async Task PostAsProtoBufAsync()
        {
            // Arrange
            var input = SimpleType.Create();

            // Act
            var response = await Client.PostAsProtoBufAsync("/protobuf-formatter", input);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<SimpleType>(new[] {_formatter});

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<SimpleType>(result);
            model.Verify();
        }

        [Fact]
        public async Task PutAsProtoBufAsync()
        {
            // Arrange
            var input = SimpleType.Create();

            // Act
            var response = await Client.PutAsProtoBufAsync("/protobuf-formatter", input);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<SimpleType>(new[] {_formatter});

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<SimpleType>(result);
            model.Verify();
        }
    }
}