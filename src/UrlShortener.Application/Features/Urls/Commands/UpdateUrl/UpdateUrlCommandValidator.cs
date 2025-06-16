using FluentValidation;

namespace UrlShortener.Application.Features.Urls.Commands.UpdateUrl
{
    public class UpdateUrlCommandValidator : AbstractValidator<UpdateUrlCommand>
    {
        public UpdateUrlCommandValidator()
        {
            RuleFor(x => x.OriginalUrl)
                .NotEmpty().WithMessage("Original URL is required")
                .Must(BeAValidUrl).WithMessage("Original URL must be a valid absolute URL.");

            RuleFor(x => x.ExpirationDate)
                .Must(BeAValidFutureDate)
                .When(x => x.ExpirationDate.HasValue)
                .WithMessage("Expiration date should be in the future");

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
