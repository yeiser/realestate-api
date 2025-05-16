using Application.Commands.Owners;
using Application.DTOs;
using Application.Queries.Owners;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controlador para gestionar los propietarios
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OwnersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OwnersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns all owners
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllOwnersQuery());
            return Ok(result);
        }

        /// <summary>
        /// Create a new Owner
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateOwnerCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.IdOwner }, result);
        }

        /// <summary>
        /// Return a registered owner 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("El ID es obligatorio.");

            var result = await _mediator.Send(new GetOwnerByIdQuery(id));
            return Ok(result);
        }

        /// <summary>
        /// Search for owners by name
        /// </summary>
        /// <param name="name">Owner name</param>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<ActionResult<List<OwnerDto>>> SearchByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("El parámetro 'name' es requerido.");

            var result = await _mediator.Send(new GetOwnerByNameQuery(name));
            return Ok(result);
        }

        /// <summary>
        /// Update an owner's data
        /// </summary>
        /// <param name="id">Owner id</param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateOwnerCommand command)
        {
            if (id != command.IdOwner)
                return BadRequest("El ID en la ruta no coincide con el del cuerpo");

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
