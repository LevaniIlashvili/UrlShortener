using Moq;
using UrlShortener.Application.Contracts.Infrastructure;
using UrlShortener.Application.Exceptions;
using UrlShortener.Application.Features.Urls.Commands.CreateUrl;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Tests.Handlers
{
    public class CreateUrlCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldThrow_WhenCustomAliasExists()
        {
            // Arrange
            var mockRepo = new Mock<IUrlRepository>();
            var mockEncoder = new Mock<IBase62Encoder>();

            var customAlias = "shortcode123";
            var url = new Url()
            {
                ShortCode = customAlias
            };

            mockRepo.Setup(r => r.GetByShortCodeAsync(customAlias)).ReturnsAsync(url);

            var handler = new CreateUrlCommandHandler(mockRepo.Object, mockEncoder.Object);
            var command = new CreateUrlCommand() { OriginalUrl = "https://example.com" , CustomAlias = customAlias};

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ConflictException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public void Validate_ShouldFail_WhenExpirationDateIsInThePast()
        {
            // Arrange
            var validator = new CreateUrlCommandValidator();
            var command = new CreateUrlCommand
            {
                OriginalUrl = "https://example.com",
                ExpirationDate = DateTime.UtcNow.AddDays(-1)
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "ExpirationDate");
        }

        [Fact]
        public void Validate_ShouldFail_WhenUrlFormatNotValid()
        {
            // Arrange
            var validator = new CreateUrlCommandValidator();
            var command = new CreateUrlCommand
            {
                OriginalUrl = "example"
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "OriginalUrl");
        }
    }
}
