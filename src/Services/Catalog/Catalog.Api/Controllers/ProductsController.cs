using Catalog.Api.Model;
using Catalog.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProductsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var products = await mediator.Send(new GetProducts());

            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }

        [HttpGet("search/names/{name}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("A name must be provided");
            }

            var products = await mediator.Send(new GetProducts { Name = name });

            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }

        [HttpGet("search/categories/{category}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> GetByCategoryName(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return BadRequest("A category must be provided");
            }

            var products = await mediator.Send(new GetProducts { Category = category });

            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("An id must be provided");
            }

            var product = await mediator.Send(new GetProduct { Id = id });

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> Create([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product information must be provided");
            }

            var created = await mediator.Send(new CreateProduct { Product = product });

            if (created == null)
            {
                // I don't think is is what should happen here, but it's okay for now
                return NotFound();
            }

            return StatusCode(201, created);

        }

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> Update(string id, [FromBody] JsonPatchDocument<Product> updates)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("An id must be provided");
            }

            if (updates == null)
            {
                return BadRequest("Product information must be provided");
            }

            var product = await mediator.Send(new UpdateProduct { Id = id, Updates = updates });

            if (product == null)
            {
                // I don't think is is what should happen here, but it's okay for now
                return NotFound();
            }

            return Ok(product);
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

            var deleted = await mediator.Send(new DeleteProduct { Id = id });

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
