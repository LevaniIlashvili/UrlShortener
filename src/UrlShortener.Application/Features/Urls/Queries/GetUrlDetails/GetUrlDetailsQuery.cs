using MediatR;

namespace UrlShortener.Application.Features.Urls.Queries.GetUrlDetails
{
    public class GetUrlDetailsQuery : IRequest<UrlDetailsVM>
    {
        public string ShortCode { get; set; }
    }
}
