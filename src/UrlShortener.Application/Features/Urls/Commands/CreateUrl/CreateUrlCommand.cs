using MediatR;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Features.Urls.Commands.CreateUrl
{
    public class CreateUrlCommand : IRequest<Url>
    {
        public string OriginalUrl { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? CustomAlias { get; set; }
    }
}
