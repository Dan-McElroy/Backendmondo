using Backendmondo.API.Models;
using NUnit.Framework;
using System;

namespace Backendmondo.Tests
{
    internal static class SubscriptionHelper
    {
        public static ProductPurchase GetPurchase(Subscription subscription, int months, DateTime purchased, float price = 0, float tax = 0)
            => new ProductPurchase(new Product { DurationMonths = months, PriceUSD = price, TaxUSD = tax }, subscription) { Purchased = purchased };

        public static SubscriptionPause GetPause(TimeSpan pauseLength, DateTime? started = null)
            => new SubscriptionPause { Started = started ?? DateTime.Now, Ended = DateTime.Now + pauseLength };

        public static SubscriptionPause GetActivePause(DateTime? started = null)
            => new SubscriptionPause { Started = started ?? DateTime.Now };

        public static void AssertDateEquality(DateTime? first, DateTime? second)
            => Assert.AreEqual(first?.ToString("yyyy-MM-ddTHH:mm:ss"), second?.ToString("yyyy-MM-ddTHH:mm:ss"));

        public static void AssertTimeSpanEquality(TimeSpan? first, TimeSpan? second)
            => Assert.AreEqual(first?.TotalMilliseconds ?? 0, second?.TotalMilliseconds ?? 0, 1);
    }
}
