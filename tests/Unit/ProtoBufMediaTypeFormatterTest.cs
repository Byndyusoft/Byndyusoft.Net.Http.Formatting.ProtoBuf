using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting.Models;
using System.Threading.Tasks;
using ProtoBuf.Meta;
using Xunit;

namespace System.Net.Http.Formatting.Unit
{
    public class ProtoBufMediaTypeFormatterTest
    {
        private readonly ProtoBufHttpContent _content;
        private readonly TransportContext _context = null;
        private readonly ProtoBufMediaTypeFormatter _formatter;
        private readonly IFormatterLogger _logger = null;

        public ProtoBufMediaTypeFormatterTest()
        {
            _formatter = new ProtoBufMediaTypeFormatter();
            _content = new ProtoBufHttpContent(_formatter.Model);
        }

        [Fact]
        public void DefaultConstructor()
        {
            // Act
            var formatter = new ProtoBufMediaTypeFormatter();

            // Assert
            Assert.NotNull(formatter.Model);
        }

        [Fact]
        public void ConstructorWithModel()
        {
            // Arrange
            var model = RuntimeTypeModel.Create();

            // Act
            var formatter = new ProtoBufMediaTypeFormatter(model);

            // Assert
            Assert.Same(model, formatter.Model);
        }

        [Theory]
        [InlineData(typeof(IInterface), false)]
        [InlineData(typeof(AbstractClass), false)]
        [InlineData(typeof(NonPublicClass), false)]
        [InlineData(typeof(Dictionary<string, object>), false)]
        [InlineData(typeof(string), true)]
        [InlineData(typeof(SimpleType), true)]
        [InlineData(typeof(ComplexType), true)]
        public void CanReadType_ReturnsFalse_ForAnyUnsupportedModelType(Type modelType, bool expectedCanRead)
        {
            // Act
            var result = _formatter.CanReadType(modelType);

            // Assert
            Assert.Equal(expectedCanRead, result);
        }

        [Theory]
        [InlineData(typeof(IInterface), false)]
        [InlineData(typeof(AbstractClass), false)]
        [InlineData(typeof(NonPublicClass), false)]
        [InlineData(typeof(Dictionary<string, object>), false)]
        [InlineData(typeof(string), true)]
        [InlineData(typeof(SimpleType), true)]
        [InlineData(typeof(ComplexType), true)]
        public void CanWriteType_ReturnsFalse_ForAnyUnsupportedModelType(Type modelType, bool expectedCanRead)
        {
            // Act
            var result = _formatter.CanWriteType(modelType);

            // Assert
            Assert.Equal(expectedCanRead, result);
        }

        [Theory]
        [InlineData("application/protobuf")]
        [InlineData("application/x-protobuf")]
        public void HasProperSupportedMediaTypes(string mediaType)
        {
            // Assert
            Assert.Contains(mediaType, _formatter.SupportedMediaTypes.Select(content => content.ToString()));
        }

        [Fact]
        public async Task ReadFromStreamAsync_WhenTypeIsNull_ThrowsException()
        {
            // Act
            var exception =
                await Assert.ThrowsAsync<ArgumentNullException>(
                    () => _formatter.ReadFromStreamAsync(null, _content.Stream, _content, _logger));

            // Assert
            Assert.Equal("type", exception.ParamName);
        }

        [Fact]
        public async Task ReadFromStreamAsync_WhenStreamIsNull_ThrowsException()
        {
            // Act
            var exception =
                await Assert.ThrowsAsync<ArgumentNullException>(
                    () => _formatter.ReadFromStreamAsync(typeof(object), null, _content, _logger));

            // Assert
            Assert.Equal("readStream", exception.ParamName);
        }

        [Fact]
        public async Task ReadFromStreamAsync_ReadsNullObject()
        {
            // Assert
            _content.WriteObject<object>(null);

            // Act
            var result = await _formatter.ReadFromStreamAsync(typeof(object), _content.Stream, _content, _logger);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ReadFromStreamAsync_ReadsBasicType()
        {
            // Arrange
            var expectedInt = 10;
            _content.WriteObject(expectedInt);

            // Act
            var result = await _formatter.ReadFromStreamAsync(typeof(int), _content.Stream, _content, _logger);

            // Assert
            Assert.Equal(expectedInt, result);
        }

        [Fact]
        public async Task ReadFromStreamAsync_ReadsSimpleTypes()
        {
            // Arrange
            var input = SimpleType.Create();

            _content.WriteObject(input);

            // Act
            var result = await _formatter.ReadFromStreamAsync(typeof(SimpleType), _content.Stream, _content, _logger);

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<SimpleType>(result);
            model.Verify();
        }

        [Fact]
        public async Task ReadFromStreamAsync_ReadsComplexTypes()
        {
            // Arrange
            var input = ComplexType.Create();
            _content.WriteObject(input);

            // Act
            var result = await _formatter.ReadFromStreamAsync(typeof(ComplexType), _content.Stream, _content, _logger);

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ComplexType>(result);
            model.Verify();
        }


        [Fact]
        public async Task WriteToStreamAsync_WhenTypeIsNull_ThrowsException()
        {
            // Act
            var exception =
                await Assert.ThrowsAsync<ArgumentNullException>(
                    () => _formatter.WriteToStreamAsync(null, new object(), _content.Stream, _content, _context));

            // Assert
            Assert.Equal("type", exception.ParamName);
        }

        [Fact]
        public async Task WriteToStreamAsync_WhenStreamIsNull_ThrowsException()
        {
            // Act
            var exception =
                await Assert.ThrowsAsync<ArgumentNullException>(
                    () => _formatter.WriteToStreamAsync(typeof(object), new object(), null, _content, _context));

            // Assert
            Assert.Equal("writeStream", exception.ParamName);
        }

        [Fact]
        public async Task WriteToStreamAsync_WritesNullObject()
        {
            // Act
            await _formatter.WriteToStreamAsync(typeof(object), null, _content.Stream, _content, _context);

            // Assert
            var result = _content.ReadObject<object>();
            Assert.Null(result);
            Assert.Equal(0, _content.Headers.ContentLength);
        }

        [Fact]
        public async Task WriteToStreamAsync_WritesBasicType()
        {
            // Arrange
            var expectedInt = 10;

            // Act
            await _formatter.WriteToStreamAsync(typeof(int), expectedInt, _content.Stream, _content, _context);

            // Assert
            var result = _content.ReadObject<int>();
            Assert.Equal(expectedInt, result);
            Assert.NotEqual(0, _content.Headers.ContentLength);
        }

        [Fact]
        public async Task WriteToStreamAsync_WritesSimplesType()
        {
            // Arrange
            var input = SimpleType.Create();

            // Act
            await _formatter.WriteToStreamAsync(typeof(SimpleType), input, _content.Stream, _content, _context);

            // Assert
            var result = _content.ReadObject<SimpleType>();
            Assert.NotEqual(0, _content.Headers.ContentLength);
            result.Verify();
        }

        [Fact]
        public async Task WriteToStreamAsync_WritesComplexType()
        {
            // Arrange
            var input = ComplexType.Create();

            // Act
            await _formatter.WriteToStreamAsync(typeof(ComplexType), input, _content.Stream, _content, _context);

            // Assert
            var result = _content.ReadObject<ComplexType>();
            Assert.NotEqual(0, _content.Headers.ContentLength);
            result.Verify();
        }

        private class ProtoBufHttpContent : StreamContent
        {
            public ProtoBufHttpContent(TypeModel model) : this(new MemoryStream())
            {
                Model = model;
            }

            private ProtoBufHttpContent(MemoryStream stream)
                : base(stream)
            {
                Stream = stream;
            }

            public MemoryStream Stream { get; }
            public TypeModel Model { get; }

            public void WriteObject<T>(T value)
            {
                if (value != null) Model.Serialize(Stream, value);
                Stream.Position = 0;
            }

            public T ReadObject<T>()
            {
                if (Stream.Length == 0)
                    return default;

                Stream.Position = 0;
                return Model.Deserialize<T>(Stream);
            }
        }

        private interface IInterface
        {
        }

        private abstract class AbstractClass
        {
        }

        private class NonPublicClass
        {
        }
    }
}