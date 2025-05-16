using Application.Commands.PropertyTraces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controlador para la gestión de las trazas de propiedades
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyTraceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PropertyTraceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create a Property Trace
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> CreateTrace([FromBody] CreatePropertyTraceCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { Id = result });
        }
    }
}
