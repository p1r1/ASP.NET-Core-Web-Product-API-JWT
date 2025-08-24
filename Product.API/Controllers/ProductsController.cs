using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Products.Commands;
using Application.Products.Queries;
using Core.DTOs;

namespace Product.API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var products = await _mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Application.Products.Commands.CreateProductCommand command) {
            var productId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAll), new { id = productId }, productId);
        }
    }
}