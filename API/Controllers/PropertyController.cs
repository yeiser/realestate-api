using Application.Commands.Properties;
using Application.Queries.Properties;
using Application.Queries.PropertyImages;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controlador para la gestion de propiedades
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PropertyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns all properties
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllPropertiesQuery());
            return Ok(result);
        }

        /// <summary>
        /// Create a property
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreatePropertyWithImageCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.IdProperty }, result);
        }

        /// <summary>
        /// Returns the data of a property 
        /// </summary>
        /// <param name="id">Property id</param>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _mediator.Send(new GetPropertyByIdQuery { Id = id });
            return Ok(result);
        }

        /// <summary>
        /// Search for a property by name, address, and price range
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] SearchPropertiesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Add images to a property
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("AddImage/{propertyId}")]
        public async Task<IActionResult> AddImage(string propertyId, [FromBody] AddPropertyImageCommand command)
        {
            if (propertyId != command.PropertyId)
                return BadRequest("El PropertyId en la ruta no coincide con el del cuerpo");

            var result = await _mediator.Send(command);
            return Ok(new { ImageId = result });
        }

        /// <summary>
        /// Updates a property's data
        /// </summary>
        /// <param name="id">Property Id</param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdatePropertyCommand command)
        {
            if (id != command.IdProperty)
                return BadRequest("El ID de la ruta no coincide con el del cuerpo de la solicitud.");

            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Returns the data of a property 
        /// </summary>
        /// <param name="id">Property id</param>
        /// <returns></returns>
        [HttpGet("GetImagesByPropertyId/{id}")]
        public async Task<IActionResult> GetImagesByPropertyId(string id)
        {
            var result = await _mediator.Send(new GetImagesByPropertyIdQuery { Id = id });
            return Ok(result);
        }
    }
}
