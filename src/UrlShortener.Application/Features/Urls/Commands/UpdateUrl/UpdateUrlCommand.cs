using MediatR;

namespace UrlShortener.Application.Features.Urls.Commands.UpdateUrl
{
    public class UpdateUrlCommand : IRequest
    {
        public string ShortCode { get; set; } = "";
        public string OriginalUrl { get; set; } = "";
        public DateTime? ExpirationDate { get; set; }
    }
}
