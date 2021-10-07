using Basket.Api.Model;
using Basket.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Basket.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class BasketController : ControllerBase
    {
        private readonly IMediator mediator;

        public BasketController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Model.Basket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Model.Basket>> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("An id must be provided");
            }

            var product = await mediator.Send(new GetBasket { Id = id });

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Model.Basket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Model.Basket>> CreateOrReplace([FromBody] Model.Basket basket)
        {
            if (basket == null)
            {
                return BadRequest("An basket must be provided");
            }

            basket = await mediator.Send(new CreateOrReplaceBasket { Basket = basket });

            if (basket == null)
            {
                // I don't think is is what should happen here, but it's okay for now
                return NotFound();
            }

            return Ok(basket);
        }

        [HttpPut("{id}/items")]
        [ProducesResponseType(typeof(Model.Basket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Model.Basket>> AddItems(string id, [FromBody] List<BasketItem> items)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("An id must be provided");
            }

            if (items == null)
            {
                return BadRequest("Items must be provided");
            }

            var basket = await mediator.Send(new AddItems { Id = id, Items = items });

            if (basket == null)
            {
                // I don't think is is what should happen here, but it's okay for now
                return NotFound();
            }

            return Ok(basket);
        }

        [HttpDelete("{id}/items")]
        [ProducesResponseType(typeof(Model.Basket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Model.Basket>> Update(string id, [FromBody] List<BasketItem> items)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("An id must be provided");
            }

            if (items == null)
            {
                return BadRequest("Items must be provided");
            }

            var basket = await mediator.Send(new RemoveItems { Id = id, Items = items });

            if (basket == null)
            {
                // I don't think is is what should happen here, but it's okay for now
                return NotFound();
            }

            return Ok(basket);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status418ImATeapot)]
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("An id must be provided");
            }

            var deleted = await mediator.Send(new DeleteBasket { Id = id });

            if(!deleted)
            {
                // We need more information on why this failed to provide a specific error code
                // for now indicate that we failed because of some conflict condition
                return StatusCode(418);
            }

            return NoContent();
        }
    }
}
