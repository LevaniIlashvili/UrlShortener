using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Features.Urls.Commands.CreateUrl;
using UrlShortener.Application.Features.Urls.Commands.DeleteUrl;
using UrlShortener.Application.Features.Urls.Commands.UpdateUrl;
using UrlShortener.Application.Features.Urls.Queries.GetUrlDetails;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Api.Controllers
{
    [Route("api/urls")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UrlController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Url>> CreateUrl(CreateUrlCommand request)
        {
            var url = await _mediator.Send(request);

            return CreatedAtAction(nameof(GetUrlDetails), new { url.ShortCode }, url);
        }

        [HttpGet("{shortCode}")]
        public async Task<ActionResult<UrlDetailsVM>> GetUrlDetails(string shortCode)
        {
            var query = new GetUrlDetailsQuery() { ShortCode = shortCode };
            var urlDetails = await _mediator.Send(query);

            return Ok(urlDetails);
        }

        [HttpPut("{shortCode}")]
        public async Task<ActionResult> UpdateUrl([FromBody] UpdateUrlCommand request, [FromRoute] string shortCode)
        {
            request.ShortCode = shortCode;

            await _mediator.Send(request);

            return NoContent();
        }

        [HttpDelete("{shortCode}")]
        public async Task<ActionResult> DeleteUrl(string shortCode)
        {
            var command = new DeleteUrlCommand() { ShortCode = shortCode };
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
