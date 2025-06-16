using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Features.Urls.Queries.GetUrl;

namespace UrlShortener.Api.Controllers
{
    [ApiController]
    public class RedirectController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RedirectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{shortCode}")]
        public async Task<ActionResult> RedirectToOriginal(string shortCode)
        {
            var query = new GetUrlQuery
            {
                ShortCode = shortCode,
                UserAgent = HttpContext.Request.Headers.UserAgent.ToString(),
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
            };

            var result = await _mediator.Send(query);

            return Redirect(result.OriginalUrl);
        }
    }
}
