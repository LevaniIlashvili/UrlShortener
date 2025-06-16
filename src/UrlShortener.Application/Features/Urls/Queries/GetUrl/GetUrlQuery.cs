using MediatR;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Features.Urls.Queries.GetUrl
{
    public class GetUrlQuery : IRequest<Url>
    {
        public string ShortCode { get; set; }
        public string UserAgent { get; set; }
        public string IpAddress { get; set; }
    }
}
