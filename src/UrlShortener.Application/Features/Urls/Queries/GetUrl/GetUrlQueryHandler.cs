using MediatR;
using UrlShortener.Application.Contracts.Infrastructure;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Features.Urls.Queries.GetUrl
{
    public class GetUrlQueryHandler : IRequestHandler<GetUrlQuery, Url>
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IClickAnalyticsRepository _clickAnalyticsRepository;

        public GetUrlQueryHandler(IUrlRepository urlRepository, IClickAnalyticsRepository clickAnalyticsRepository)
        {
            _urlRepository = urlRepository;
            _clickAnalyticsRepository = clickAnalyticsRepository;
        }

        public async Task<Url> Handle(GetUrlQuery request, CancellationToken cancellationToken)
        {
            var url = await _urlRepository.GetByShortCodeAsync(request.ShortCode);

            if (url is null)
            {
                throw new Exceptions.NotFoundException("Url not found");
            }

            url.ClickCount++;
            await _urlRepository.UpdateAsync(url);

            var clickAnalytics = new ClickAnalytics()
            {
                ShortCode = url.ShortCode,
                ClickDate = DateTime.UtcNow,
                IpAddress = request.IpAddress,
                UserAgent = request.UserAgent
            };
            await _clickAnalyticsRepository.AddAsync(clickAnalytics);

            return url;
        }
    }
}
