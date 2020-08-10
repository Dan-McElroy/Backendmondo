using System;
using Backendmondo.API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Backendmondo.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriptionController : ControllerBase
    {
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetSubscription(string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest();
            }

            var rng = new Random();
            var example = new SubscriptionDTO
            {
                Id = id,
                Duration = rng.Next(1, 12),
                StartDate = DateTime.UtcNow - TimeSpan.FromDays(30),
                EndDate = DateTime.UtcNow + TimeSpan.FromDays(30),
                Price = 4,
                Tax = 12
            };
            return new ObjectResult(example);
        }

        [HttpPost]
        [Route("{id}/pause")]
        public IActionResult PostPause(string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest();
            }
            return NoContent();
        }

        [HttpPost]
        [Route("{id}/resume")]
        public IActionResult PostResume(string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest();
            }
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteSubscription(string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
