﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Backendmondo.API.Context;
using Backendmondo.API.Models;
using Backendmondo.API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Backendmondo.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;

        public ProductsController(IApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetProducts()
        {
            return new ObjectResult(_context.Products.Select(product => product.ToDTO()));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProduct(string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest("Given ID has an invalid format.");
            }

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound("No product could be found with that ID.");
            }
            return new ObjectResult(product.ToDTO());
        }

        [HttpPost]
        [Route("purchase")]
        public IActionResult PostPurchase([FromBody] PurchaseRequestDTO request)
        {
            if (!(Guid.TryParse(request.ProductId, out var productId)))
            {
                return BadRequest("Given product ID has an invalid format.");
            }
            return NoContent();
        }

        [HttpPost]
        [Route("secret/add")]
        public async Task<IActionResult> PostAddProduct([FromBody] ProductDTO product)
        {
            _context.Products.Add(new Product { DurationMonths = product.Duration, Name = product.Name, PriceUSD = product.Price, TaxUSD = product.Tax });
            await _context.Save();

            return Ok("Product successfully added to store.");
        }
    }
}
