using System;
using Entity;
using NUnit.Framework;
using BLL.Manager;
using System.Linq;
using System.Collections.Generic;

namespace BLL_Tests
{
    [TestFixture]
    public class CartTests
    {
        Category category;
        [SetUp]
        public void SetUp()
        {
            category = new Category("food");
        }
        [Test]
        public void AddProduct_DifferentProducts_ReturnsProdoctsCount()
        {
            var product = new Product("Apple", 1000.0m, category);
            var product2 = new Product("Almonds", 150.0m, category);
            var cart = new Cart("TestCart");

            cart.AddProduct(product, 3);
            cart.AddProduct(product2, 1);

            Assert.That(cart.Products.Count, Is.EqualTo(2));
        }

        [Test]
        public void AddProduct_SameProducts_ReturnsProdoctsCountr()
        {
            var product = new Product("Apple", 1000.0m, category);
            var product2 = new Product("Almonds", 150.0m, category);
            var cart = new Cart("TestCart");

            cart.AddProduct(product, 3);
            cart.AddProduct(product, 3);
            cart.AddProduct(product2, 1);

            Assert.That(cart.Products.Count, Is.EqualTo(2));
        }

        [Test]
        public void ApplyCampaign_DifferentCampaign_MaxDiscount()
        {
            category.SubCategory = new List<Category>() { new Category("Sub1"), new Category("Sub2") };

            var product = new Product("Apple", 100.0m, category.SubCategory[0]);
            var product3 = new Product("Banana", 50.0m, category.SubCategory[0]);
            var product2 = new Product("Almonds", 150.0m, category.SubCategory[1]);
            var cart = new Cart("TestCart");

            cart.AddProduct(product, 3);
            cart.AddProduct(product3, 2);
            cart.AddProduct(product2, 5);


            var campaign = new Campaign(category.SubCategory[0], 20, 3, DiscountType.Rate);
            var campaign2 = new Campaign(category.SubCategory[1], 50, 5, DiscountType.Rate);
            var campaign3 = new Campaign(category, 50, 5, DiscountType.Rate);
            //var campaign3 = new Campaign(category, 5, 5, DiscountType.Amount);

            var discount = cart.ApplyDiscounts(new List<Campaign>() { campaign, campaign2, campaign3 });

            Assert.That(discount.Key, Is.EqualTo(campaign3));
        }

        [Test]
        public void ApplyCampaign_CampaignTypeAmount_MaxDiscount()
        {
            category.SubCategory = new List<Category>() { new Category("Sub1"), new Category("Sub2") };

            var product = new Product("Apple", 100.0m, category.SubCategory[0]);
            var product3 = new Product("Banana", 50.0m, category.SubCategory[0]);
            var product2 = new Product("Almonds", 150.0m, category.SubCategory[1]);
            var cart = new Cart("TestCart");

            cart.AddProduct(product, 3);
            cart.AddProduct(product3, 2);
            cart.AddProduct(product2, 5);


            var campaign = new Campaign(category.SubCategory[0], 20, 2, DiscountType.Rate);
            var campaign2 = new Campaign(category.SubCategory[1], 50, 2, DiscountType.Rate);
            var campaign3 = new Campaign(category, 500, 5, DiscountType.Amount);
            //var campaign3 = new Campaign(category, 5, 5, DiscountType.Amount);

            var discount = cart.ApplyDiscounts(new List<Campaign>() { campaign, campaign2, campaign3 });

            Assert.That(discount.Key, Is.EqualTo(campaign3));
        }

        [Test]
        public void ApplyCampaign_NoDiscount_ReturnNull()
        {
            category.SubCategory = new List<Category>() { new Category("Sub1"), new Category("Sub2") };

            var product = new Product("Apple", 100.0m, category.SubCategory[0]);
            var product3 = new Product("Banana", 50.0m, category.SubCategory[0]);
            var product2 = new Product("Almonds", 150.0m, category.SubCategory[1]);
            var cart = new Cart("TestCart");

            cart.AddProduct(product, 3);
            cart.AddProduct(product3, 2);
            cart.AddProduct(product2, 5);


            var campaign = new Campaign(category.SubCategory[0], 20, 8, DiscountType.Rate);
            var campaign2 = new Campaign(category.SubCategory[1], 50, 15, DiscountType.Rate);
            var campaign3 = new Campaign(category, 5, 55, DiscountType.Amount);
            //var campaign3 = new Campaign(category, 5, 5, DiscountType.Amount);

            var discount = cart.ApplyDiscounts(new List<Campaign>() { campaign, campaign2, campaign3 });

            Assert.That(discount.Key, Is.Null);
        }

        [Test]
        public void CalculateDiscount_Rate20foodCategory_ReturnDiscountValue()
        {
            category.SubCategory = new List<Category>() { new Category("Sub1"), new Category("Sub2") };

            var product = new Product("Apple", 100.0m, category.SubCategory[0]);
            var product3 = new Product("Banana", 50.0m, category.SubCategory[0]);
            var product2 = new Product("Almonds", 150.0m, category.SubCategory[1]);
            var cart = new Cart("TestCart");

            cart.AddProduct(product, 3);
            cart.AddProduct(product3, 2);
            cart.AddProduct(product2, 5);


            var campaign = new Campaign(category.SubCategory[0], 20, 3, DiscountType.Rate);
            var campaign2 = new Campaign(category.SubCategory[1], 50, 15, DiscountType.Rate);
            var campaign3 = new Campaign(category, 5, 55, DiscountType.Amount);
            //var campaign3 = new Campaign(category, 5, 5, DiscountType.Amount);

            var discount = cart.ApplyDiscounts(new List<Campaign>() { campaign, campaign2, campaign3 });

            Assert.That(discount.Value, Is.EqualTo(80));
        }

        [Test]
        public void ApplyCoupon_Rate100_Returns10()
        {
            category.SubCategory = new List<Category>() { new Category("Sub1"), new Category("Sub2") };

            var product = new Product("Apple", 100.0m, category.SubCategory[0]);
            var product3 = new Product("Banana", 50.0m, category.SubCategory[0]);
            var product2 = new Product("Almonds", 150.0m, category.SubCategory[1]);
            var cart = new Cart("TestCart");

            cart.AddProduct(product, 3);
            cart.AddProduct(product3, 2);
            cart.AddProduct(product2, 5);

            Coupon coupon = new Coupon(100, 10, DiscountType.Rate);


            var result = cart.ApplyDiscount(coupon);

            Assert.That(result, Is.EqualTo(115));
        }

        [Test]
        public void getTotalAmountAfterDiscounts_SomeProductCart_ReturnValue()
        {
            category.SubCategory = new List<Category>() { new Category("Sub1"), new Category("Sub2") };

            var product = new Product("Apple", 100.0m, category.SubCategory[0]);
            var product3 = new Product("Banana", 50.0m, category.SubCategory[0]);
            var product2 = new Product("Almonds", 150.0m, category.SubCategory[1]);
            var cart = new Cart("TestCart");

            cart.AddProduct(product, 3);
            cart.AddProduct(product3, 2);
            cart.AddProduct(product2, 5);

            var campaign = new Campaign(category.SubCategory[0], 20, 3, DiscountType.Rate);
            Coupon coupon = new Coupon(100, 10, DiscountType.Rate);

            var result = cart.getTotalAmountAfterDiscounts(campaign, coupon);

            Assert.That(result, Is.EqualTo(963));
        }

        [Test]
        public void getCouponDiscount_SomeProductCart_ReturnValue()
        {
            category.SubCategory = new List<Category>() { new Category("Sub1"), new Category("Sub2") };

            var product = new Product("Apple", 100.0m, category.SubCategory[0]);
            var product3 = new Product("Banana", 50.0m, category.SubCategory[0]);
            var product2 = new Product("Almonds", 150.0m, category.SubCategory[1]);
            var cart = new Cart("TestCart");

            cart.AddProduct(product, 3);
            cart.AddProduct(product3, 2);
            cart.AddProduct(product2, 5);

            Coupon coupon = new Coupon(100, 10, DiscountType.Rate);

            var result = cart.getCouponDiscount(coupon);

            Assert.That(result, Is.EqualTo(115));
        }

        [Test]
        public void getCampaignDiscount_SomeProductCart_ReturnValue()
        {
            category.SubCategory = new List<Category>() { new Category("Sub1"), new Category("Sub2") };

            var product = new Product("Apple", 100.0m, category.SubCategory[0]);
            var product3 = new Product("Banana", 50.0m, category.SubCategory[0]);
            var product2 = new Product("Almonds", 150.0m, category.SubCategory[1]);
            var cart = new Cart("TestCart");

            cart.AddProduct(product, 3);
            cart.AddProduct(product3, 2);
            cart.AddProduct(product2, 5);

            var campaign = new Campaign(category.SubCategory[0], 20, 3, DiscountType.Rate);

            var result = cart.getCampaignDiscount(campaign);

            Assert.That(result, Is.EqualTo(80));
        }

        [Test]
        public void getDeliveryCost_SomeProductCart_ReturnValue()
        {
            category.SubCategory = new List<Category>() { new Category("Sub1"), new Category("Sub2") };

            var product = new Product("Apple", 100.0m, category.SubCategory[0]);
            var product3 = new Product("Banana", 50.0m, category.SubCategory[0]);
            var product2 = new Product("Almonds", 150.0m, category.SubCategory[1]);
            var cart = new Cart("TestCart");

            cart.AddProduct(product, 3);
            cart.AddProduct(product3, 2);
            cart.AddProduct(product2, 5);


            var result = cart.getDeliveryCost(1, 2, 3);

            Assert.That(result, Is.EqualTo(11));
        }
    }
}
