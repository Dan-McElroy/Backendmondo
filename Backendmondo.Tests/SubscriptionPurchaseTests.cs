using Backendmondo.API.Models;
using NUnit.Framework;
using System;

namespace Backendmondo.Tests
{
    public class SubscriptionPurchaseTests
    {
        private Subscription _subscription;
        private DateTime _now;

        [SetUp]
        public void Setup()
        {
            _subscription = new Subscription();
            _now = DateTime.Now;
        }

        [Test]
        public void SubscriptionCalculatesPurchaseDateCorrectly_WithMultiplePurchases()
        {
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(1)));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(2)));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(4)));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(-20)));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(1000)));

            SubscriptionHelper.AssertDateEquality(_now.AddDays(-20), _subscription.Purchased);
        }


        [Test]
        public void SubscriptionCalculatesPurchaseDateCorrectly_WithNoPurchases()
        {
            Assert.AreEqual(null, _subscription.Purchased);
        }

        [Test]
        public void SubscriptionCalculatesDurationCorrectly_WithMultiplePurchases()
        {
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 20, _now.AddDays(1)));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 19, _now.AddDays(2)));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 18, _now.AddDays(4)));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 17, _now.AddDays(-20)));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 16, _now.AddDays(1000)));

            Assert.AreEqual(20 + 19 + 18 + 17 + 16, _subscription.TotalDurationMonths);
        }

        [Test]
        public void SubscriptionCalculatesDurationCorrectly_WithNoPurchases()
        {
            Assert.AreEqual(0, _subscription.TotalDurationMonths);
        }

        [Test]
        public void SubscriptionCalculatesDurationCorrectly_WithNegativeDurationPurchases()
        {
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, -20, _now.AddDays(1)));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 19, _now.AddDays(2)));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 18, _now.AddDays(4)));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 17, _now.AddDays(-20)));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 16, _now.AddDays(1000)));

            Assert.AreEqual(19 + 18 + 17 + 16, _subscription.TotalDurationMonths);
        }

        [Test]
        public void SubscriptionCalculatesCostCorrectly_WithMultiplePurchases()
        {
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(1), price: 10));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(2), price: 15));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(4), price: 22.5f));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(-20), price: 100));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(1000), price: 0));

            Assert.AreEqual(10 + 15 + 22.5f + 100, _subscription.TotalPurchaseCostUSD);
        }

        [Test]
        public void SubscriptionCalculatesCostCorrectly_WithNoPurchases()
        {
            Assert.AreEqual(0, _subscription.TotalPurchaseCostUSD);
        }

        [Test]
        public void SubscriptionCalculatesCostCorrectly_WithAlteredCostPurchases()
        {
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(1), price: 10));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(2), price: 15));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(4), price: 22.5f));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(-20), price: 100));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(1000), price: 0));

            var product = new Product { DurationMonths = 1, PriceUSD = 100, TaxUSD = 100 };
            var purchase = new ProductPurchase(product, _subscription);
            _subscription.ProductsPurchased.Add(purchase);
            Assert.AreEqual(10 + 15 + 22.5f + 100 + 100, _subscription.TotalPurchaseCostUSD);

            product.PriceUSD = 50;
            Assert.AreEqual(10 + 15 + 22.5f + 100 + 100, _subscription.TotalPurchaseCostUSD);
        }

        [Test]
        public void SubscriptionCalculatesTaxCorrectly_WithMultiplePurchases()
        {
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(1), tax: 10));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(2), tax: 15));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(4), tax: 22.5f));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(-20), tax: 100));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(1000), tax: 0));

            Assert.AreEqual(10 + 15 + 22.5f + 100, _subscription.TotalTaxCostUSD);
        }

        [Test]
        public void SubscriptionCalculatesTaxCorrectly_WithNoPurchases()
        {
            Assert.AreEqual(0, _subscription.TotalTaxCostUSD);
        }

        [Test]
        public void SubscriptionCalculatesTaxCorrectly_WithNegativeTaxPurchases()
        {
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(1), tax: 10));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(2), tax: 15));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(4), tax: 22.5f));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(-20), tax: 100));
            _subscription.ProductsPurchased.Add(SubscriptionHelper.GetPurchase(_subscription, 1, _now.AddDays(1000), tax: 0));

            var product = new Product { DurationMonths = 1, PriceUSD = 100, TaxUSD = 100 };
            var purchase = new ProductPurchase(product, _subscription);
            _subscription.ProductsPurchased.Add(purchase);
            Assert.AreEqual(10 + 15 + 22.5f + 100 + 100, _subscription.TotalTaxCostUSD);

            product.TaxUSD = 50;
            Assert.AreEqual(10 + 15 + 22.5f + 100 + 100, _subscription.TotalTaxCostUSD);
        }
    }
}
