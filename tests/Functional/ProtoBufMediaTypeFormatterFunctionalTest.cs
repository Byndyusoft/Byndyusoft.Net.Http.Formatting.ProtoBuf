using System.IO;
using System.Net.Http.Formatting;
using System.Net.Http.Formatting.Protobuf;
using System.Net.Http.Tests.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace System.Net.Http.Tests.Functional
{
    public class ProtoBufMediaTypeFormatterFunctionalTest : MvcTestFixture
    {
        private readonly ProtoBufMediaTypeFormatter _formatter;

        public ProtoBufMediaTypeFormatterFunctionalTest()
        {
            _formatter = new ProtoBufMediaTypeFormatter();
        }

        protected override void ConfigureHttpClient(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Add(ProtoBufConstants.DefaultMediaTypeHeader);
        }

        protected override void ConfigureMvc(IMvcCoreBuilder builder)
        {
            builder.AddProtoBufNet(
                options => { options.Model = ProtoBufConstants.DefaultTypeModel; });
        }

        [Fact]
        public async Task PostAsMessagePackAsync()
        {
            // Arrange
            var input = new SimpleType
            {
                Property = 10,
                Enum = SeekOrigin.Current,
                Field = "string",
                Array = new[] {1, 2},
                Nullable = 100
            };

            // Act
            var response = await Client.PostAsProtoBufAsync("/protobuf-formatter", input);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<SimpleType>(new[] {_formatter});

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<SimpleType>(result);

            Assert.Equal(input.Property, model.Property);
            Assert.Equal(input.Field, model.Field);
            Assert.Equal(input.Enum, model.Enum);
            Assert.Equal(input.Array, model.Array);
            Assert.Equal(input.Nullable, model.Nullable);
        }

        [Fact]
        public async Task PutAsMessagePackAsync()
        {
            // Arrange
            var input = new SimpleType
            {
                Property = 10,
                Enum = SeekOrigin.Current,
                Field = "string",
                Array = new[] {1, 2},
                Nullable = 100
            };

            // Act
            var response = await Client.PutAsProtoBufAsync("/protobuf-formatter", input);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<SimpleType>(new[] {_formatter});

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<SimpleType>(result);

            Assert.Equal(input.Property, model.Property);
            Assert.Equal(input.Field, model.Field);
            Assert.Equal(input.Enum, model.Enum);
            Assert.Equal(input.Array, model.Array);
            Assert.Equal(input.Nullable, model.Nullable);
        }
    }
}