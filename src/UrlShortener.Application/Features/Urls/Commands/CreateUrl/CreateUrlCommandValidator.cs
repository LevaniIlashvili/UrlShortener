using FluentValidation;

namespace UrlShortener.Application.Features.Urls.Commands.CreateUrl
{
    public class CreateUrlCommandValidator : AbstractValidator<CreateUrlCommand>
    {
        public CreateUrlCommandValidator()
        {
            RuleFor(x => x.OriginalUrl)
                .NotEmpty().WithMessage("Original URL is required")
                .Must(BeAValidUrl).WithMessage("Original URL must be a valid absolute URL.");

            RuleFor(x => x.ExpirationDate)
                .Must(BeAValidFutureDate)
                .When(x => x.ExpirationDate.HasValue)
                .WithMessage("Expiration date should be in the future");

            RuleFor(x => x.CustomAlias)
                .NotEmpty()
                .When(x => x.CustomAlias is not null)
                .WithMessage("Custom alias must not be empty if provided.");

            RuleFor(x => x.CustomAlias)
                .Matches("^[a-zA-Z0-9_-]+$")
                .When(x => !string.IsNullOrEmpty(x.CustomAlias))
                .WithMessage("Custom alias can only contain letters, numbers, dashes and underscores.");

        }

        private bool BeAValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        private bool BeAValidFutureDate(DateTime? date)
        {
            return date > DateTime.UtcNow;
        }
    }
}
