using System;
using System.Linq;
using Backendmondo.API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Backendmondo.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetProducts()
        {
            var rng = new Random();
            var products = Enumerable.Range(1, 5).Select(index => new ProductDTO
            {
                Id = Guid.NewGuid().ToString(),
                Duration = rng.Next(1, 12),
                Name = index.ToString(),
                Price = 4,
                Tax = 12
            });
            return new ObjectResult(products);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetProduct(string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest();
            }

            var rng = new Random();
            var example = new ProductDTO
            {
                Id = id,
                Duration = rng.Next(1, 12),
                Name = "Test product",
                Price = 4,
                Tax = 12
            };
            return new ObjectResult(example);
        }

        [HttpPost]
        [Route("purchase")]
        public IActionResult PostPurchase([FromBody] PurchaseRequestDTO request)
        {
            if (!(Guid.TryParse(request.ProductId, out var productId)
                || Guid.TryParse(request.UserId, out var userId)))
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
