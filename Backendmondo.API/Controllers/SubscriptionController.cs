using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Backendmondo.API.Context;
using Backendmondo.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Backendmondo.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private const string EmailRegexConfigKey = "EmailRegex";

        private readonly IApplicationDbContext _context;
        private readonly string _emailRegex;

        public SubscriptionController(IApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _emailRegex = configuration[EmailRegexConfigKey];
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetSubscription([FromQuery] string email)
        {
            if (!IsValidEmailAddress(email))
            {
                ModelState.AddModelError(nameof(email), "The email field is not a valid email address.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var subscription = _context.Subscriptions
                .Include(subscription => subscription.User)
                .Include(subscription => subscription.ProductsPurchased)
                .ThenInclude(purchase => purchase.Product)
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
        public async Task<IActionResult> PostPause(string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                ModelState.AddModelError(nameof(id), "Given ID has an invalid format.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var subscription = _context.Subscriptions.Find(guid);

            var currentPause = subscription.Pauses
                .FirstOrDefault(pause => pause.Started <= DateTime.UtcNow && pause.Ended == null);

            if (currentPause != null)
            {
                return BadRequest("Subscription with the given ID is already paused.");
            }

            var pause = new SubscriptionPause()
            {
                Subscription = subscription,
                Started = DateTime.UtcNow
            };

            _context.SubscriptionPauses.Add(pause);
            
            await _context.Save();

            return NoContent();
        }

        [HttpPost]
        [Route("{id}/resume")]
        public async Task<IActionResult> PostResume(string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                ModelState.AddModelError(nameof(id), "Given ID has an invalid format.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var subscription = _context.Subscriptions.Find(guid);

            var currentPause = subscription.Pauses
                .FirstOrDefault(pause => pause.Started <= DateTime.UtcNow && pause.Ended == null);

            if (currentPause == null)
            {
                return BadRequest("Subscription is already active.");
            }

            currentPause.Ended = DateTime.UtcNow;

            await _context.Save();

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteSubscription(string id)
        {
            if (!Guid.TryParse(id, out var subscriptionId))
            {
                ModelState.AddModelError(nameof(id), "Given ID has an invalid format.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var subscription = _context.Subscriptions.Find(subscriptionId);
            if (subscription == null)
            {
                return NotFound("No subscription found with the given ID.");
            }

            _context.Subscriptions.Remove(subscription);

            await _context.Save();

            return Ok("Subscription was successfully cancelled.");
        }

        private bool IsValidEmailAddress(string email)
        {
            return Regex.IsMatch(email, _emailRegex);
        }
    }
}
