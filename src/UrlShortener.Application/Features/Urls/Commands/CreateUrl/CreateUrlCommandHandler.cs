using MediatR;
using UrlShortener.Application.Contracts.Infrastructure;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Features.Urls.Commands.CreateUrl
{
    public class CreateUrlCommandHandler : IRequestHandler<CreateUrlCommand, Url>
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IBase62Encoder _base62Encoder;

        public CreateUrlCommandHandler(IUrlRepository urlRepository, IBase62Encoder base62Encoder)
        {
            _urlRepository = urlRepository;
            _base62Encoder = base62Encoder;
        }

        public async Task<Url> Handle(CreateUrlCommand request, CancellationToken cancellationToken)
        {
            string shortCode;

            if (!string.IsNullOrWhiteSpace(request.CustomAlias))
            {
                var existingUrl = await _urlRepository.GetByShortCodeAsync(request.CustomAlias);

                if (existingUrl is not null)
                {
                    throw new Exceptions.ConflictException("Url with this short code already exists");
                }
                shortCode = request.CustomAlias;
            } else
            {
                shortCode = _base62Encoder.Encode(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            }

            var url = new Url()
            {
                OriginalUrl = request.OriginalUrl,
                ShortCode = shortCode,
                ExpirationDate = request.ExpirationDate,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                ClickCount = 0
            };

            await _urlRepository.AddAsync(url);

            return url;
        }
    }
}
