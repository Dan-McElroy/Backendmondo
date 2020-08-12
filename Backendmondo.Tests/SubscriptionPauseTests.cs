using Backendmondo.API.Models;
using NUnit.Framework;
using System;

namespace Backendmondo.Tests
{
    public class SubscriptionPauseTests
    {
        private Subscription _subscription;
        private DateTime _now;

        [SetUp]
        public void Setup()
        {
            _subscription = new Subscription();
            _now = DateTime.Now;

            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 7, _now));
        }

        [Test]
        public void SubscriptionExtendsEndDate_AfterPause()
        {
            SubscriptionHelper.AssertDateEquality(_now.AddMonths(7), _subscription.Expires);
            
            _subscription.Pauses.Add(SubscriptionHelper.GetPause(TimeSpan.FromDays(3)));
            SubscriptionHelper.AssertDateEquality(_now.AddMonths(7).AddDays(3), _subscription.Expires);
        }

        [Test]
        public void SubscriptionEndDateNull_DuringPause()
        {
            SubscriptionHelper.AssertDateEquality(_now.AddMonths(7), _subscription.Expires);

            _subscription.Pauses.Add(SubscriptionHelper.GetActivePause());
            SubscriptionHelper.AssertDateEquality(null, _subscription.Expires);
        }

        [Test]
        public void SubscriptionPauseDurationTotalledCorrectly_WhenAllPausesComplete()
        {
            _subscription.Pauses.Add(SubscriptionHelper.GetPause(TimeSpan.FromDays(3)));
            _subscription.Pauses.Add(SubscriptionHelper.GetPause(TimeSpan.FromHours(2)));
            _subscription.Pauses.Add(SubscriptionHelper.GetPause(new TimeSpan(0, 1, 2, 3, 4)));

            SubscriptionHelper.AssertTimeSpanEquality(new TimeSpan(3, 3, 2, 3, 4), _subscription.TotalPauseDuration);
        }

        [Test]
        public void SubscriptionPauseDurationTotalledCorrectly_WhenOnePauseIncomplete()
        {
            _subscription.Pauses.Add(SubscriptionHelper.GetPause(TimeSpan.FromDays(3)));
            _subscription.Pauses.Add(SubscriptionHelper.GetPause(TimeSpan.FromHours(2)));
            _subscription.Pauses.Add(SubscriptionHelper.GetPause(new TimeSpan(0, 1, 2, 3, 4)));
            _subscription.Pauses.Add(SubscriptionHelper.GetActivePause());

            SubscriptionHelper.AssertTimeSpanEquality(new TimeSpan(3, 3, 2, 3, 4), _subscription.TotalPauseDuration);
        }

        [Test]
        public void SubscriptionPauseDurationTotalledCorrectly_WithNegativePauses()
        {
            _subscription.Pauses.Add(SubscriptionHelper.GetPause(TimeSpan.FromDays(-3)));
            _subscription.Pauses.Add(SubscriptionHelper.GetPause(TimeSpan.FromHours(2)));
            _subscription.Pauses.Add(SubscriptionHelper.GetPause(new TimeSpan(0, 1, 2, 3, 4)));
            _subscription.Pauses.Add(SubscriptionHelper.GetActivePause());

            SubscriptionHelper.AssertTimeSpanEquality(new TimeSpan(0, 3, 2, 3, 4), _subscription.TotalPauseDuration);
        }

        [Test]
        public void SubscriptionIdentifiesActivePause()
        {
            _subscription.Pauses.Add(SubscriptionHelper.GetPause(TimeSpan.FromDays(3), _now + TimeSpan.FromDays(2)));
            _subscription.Pauses.Add(SubscriptionHelper.GetPause(TimeSpan.FromHours(2), _now + TimeSpan.FromDays(3)));
            var activePause = SubscriptionHelper.GetActivePause(_now + TimeSpan.FromDays(4));
            _subscription.Pauses.Add(activePause);
            _subscription.Pauses.Add(SubscriptionHelper.GetPause(new TimeSpan(0, 1, 2, 3, 4), _now + TimeSpan.FromDays(5)));
            _subscription.Pauses.Add(SubscriptionHelper.GetActivePause());

            Assert.AreEqual(activePause, _subscription.ActivePause);
        }
    }
}