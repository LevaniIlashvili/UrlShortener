using MediatR;
using UrlShortener.Application.Contracts.Infrastructure;
using UrlShortener.Application.Exceptions;

namespace UrlShortener.Application.Features.Urls.Queries.GetUrlDetails
{
    public class GetUrlDetailsQueryHandler : IRequestHandler<GetUrlDetailsQuery, UrlDetailsVM>
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IClickAnalyticsRepository _clickAnalyticsRepository;

        public GetUrlDetailsQueryHandler(IUrlRepository urlRepository, IClickAnalyticsRepository clickAnalyticsRepository)
        {
            _urlRepository = urlRepository;
            _clickAnalyticsRepository = clickAnalyticsRepository;
        }

        public async Task<UrlDetailsVM> Handle(GetUrlDetailsQuery request, CancellationToken cancellationToken)
        {
            var url = await _urlRepository.GetByShortCodeAsync(request.ShortCode);

            if (url is null)
            {
                throw new NotFoundException("Url not found");
            }

            var clickAnalytics = await _clickAnalyticsRepository.GetClickAnalyticsByShortCode(url.ShortCode);

            var urlDetailsVm = new UrlDetailsVM()
            {
                OriginalUrl = url.OriginalUrl,
                ShortCode = url.ShortCode,
                CreatedAt = url.CreatedAt,
                ExpirationDate = url.ExpirationDate,
                ClickCount = url.ClickCount,
                IsActive = url.IsActive,
                ClickAnalytics = clickAnalytics.ToList()
            };

            return urlDetailsVm;
        }
    }
}
