using MediatR;
using UrlShortener.Application.Contracts.Infrastructure;

namespace UrlShortener.Application.Features.Urls.Commands.UpdateUrl
{
    public class UpdateUrlCommandHandler : IRequestHandler<UpdateUrlCommand>
    {
        private readonly IUrlRepository _urlRepository;

        public UpdateUrlCommandHandler(IUrlRepository urlRepository)
        {
            _urlRepository = urlRepository;
        }

        public async Task Handle(UpdateUrlCommand request, CancellationToken cancellationToken)
        {
            var existingUrl = await _urlRepository.GetByShortCodeAsync(request.ShortCode);

            if (existingUrl is null)
            {
                throw new Exceptions.NotFoundException("Url not found");
            }

            existingUrl.ExpirationDate = request.ExpirationDate;
            existingUrl.OriginalUrl = request.OriginalUrl;

            await _urlRepository.UpdateAsync(existingUrl);
        }
    }
}
