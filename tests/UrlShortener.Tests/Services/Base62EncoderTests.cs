using UrlShortener.Application.Contracts.Infrastructure;
using UrlShortener.Infrastructure.Services;

namespace UrlShortener.Tests.Services
{
    public class Base62EncoderTests
    {
        private readonly IBase62Encoder _base62Encoder;

        public Base62EncoderTests()
        {
            _base62Encoder = new Base62Encoder();
        }

        [Theory]
        [InlineData(52, "0")]
        [InlineData(61, "9")]
        [InlineData(62, "ba")]
        public void Encode_ShouldReturnCorrectBase62(long input, string expected)
        {
            var result = _base62Encoder.Encode(input);
            Assert.Equal(result, expected);
        }
    }
}
