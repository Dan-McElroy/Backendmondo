using System;
using System.Linq;
using Backendmondo.API.Context;
using Backendmondo.API.Helpers;
using Backendmondo.API.Models;
using Backendmondo.API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Backendmondo.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly IApplicationDbContext _context;

        public SubscriptionController(IApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetSubscription([FromQuery] string email)
        {
            // TODO: Validate email address

            var subscription = _context.Subscriptions
                .AsEnumerable()
                .FirstOrDefault(
                    subscription => subscription.User.MatchesEmailAddress(email));

            if (subscription == null)
            {
                return NotFound("No subscription found for the given email address.");
            }

            return new ObjectResult(subscription.ToDTO());
        }

        [HttpPost]
        [Route("{id}/pause")]
        public IActionResult PostPause(string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest("Given ID has an invalid format.");
            }

            var subscription = _context.Subscriptions.Find(guid);

            var currentPause = subscription.Pauses
                .FirstOrDefault(pause => pause.Started <= DateTime.UtcNow && pause.Ended == null);

            if (currentPause != null)
            {
                return BadRequest("Subscription is already paused.");
            }

            var pause = new SubscriptionPause()
            {
                Subscription = subscription,
                Started = DateTime.UtcNow
            };

            _context.SubscriptionPauses.Add(pause);
            
            _context.Save();

            return NoContent();
        }

        [HttpPost]
        [Route("{id}/resume")]
        public IActionResult PostResume(string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest("Given ID has an invalid format.");
            }

            var subscription = _context.Subscriptions.Find(guid);

            var currentPause = subscription.Pauses
                .FirstOrDefault(pause => pause.Started <= DateTime.UtcNow && pause.Ended == null);

            if (currentPause == null)
            {
                return BadRequest("Subscription is already active.");
            }

            currentPause.Ended = DateTime.UtcNow;

            _context.Save();

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteSubscription(string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest("Given ID has an invalid format.");
            }
            return Ok("Subscription was successfully cancelled.");
        }
    }
}
