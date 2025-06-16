using MediatR;

namespace UrlShortener.Application.Features.Urls.Commands.DeleteUrl
{
    public class DeleteUrlCommand : IRequest
    {
        public string ShortCode { get; set; } = "";
    }
}
