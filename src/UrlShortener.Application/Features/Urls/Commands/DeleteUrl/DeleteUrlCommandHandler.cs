using MediatR;
using UrlShortener.Application.Contracts.Infrastructure;

namespace UrlShortener.Application.Features.Urls.Commands.DeleteUrl
{
    public class DeleteUrlCommandHandler : IRequestHandler<DeleteUrlCommand>
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IClickAnalyticsRepository _clickAnalyticsRepository;

        public DeleteUrlCommandHandler(
            IUrlRepository urlRepository,
            IClickAnalyticsRepository clickAnalyticsRepository)
        {
            _urlRepository = urlRepository;
            _clickAnalyticsRepository = clickAnalyticsRepository;
        }

        public async Task Handle(DeleteUrlCommand request, CancellationToken cancellationToken)
        {
            var url = await _urlRepository.GetByShortCodeAsync(request.ShortCode);

            if (url is null)
            {
                throw new Exceptions.NotFoundException("Url not found");
            }

            await _urlRepository.DeleteAsync(url.ShortCode);
            await _clickAnalyticsRepository.DeleteByShortCode(url.ShortCode);
        }
    }
}
