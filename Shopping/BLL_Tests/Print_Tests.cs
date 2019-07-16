using Entity;
using BLL.Manager;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BLL_Tests
{
    [TestFixture]
    public class Print_Tests
    {
        [Test]
        public void Print_CartItem_ReturnTrue()
        {

            var category = new Category("food");
            category.SubCategory = new List<Category>() { new Category("Sub1"), new Category("Sub2") };

            var product = new Product("Apple", 100.0m, category.SubCategory[0]);
            var product3 = new Product("Banana", 50.0m, category.SubCategory[1]);
            var product2 = new Product("Almonds", 150.0m, category);
            var cart = new Cart("TestCart");

            cart.AddProduct(product, 3);
            cart.AddProduct(product3, 2);
            cart.AddProduct(product2, 5);

            var campaign = new Campaign(category.SubCategory[0], 20, 3, DiscountType.Rate);
            Coupon coupon = new Coupon(100, 10, DiscountType.Amount);

            var save=cart.Print(campaign, coupon);
            


            Assert.That(save, Is.True);
        }
    }
}
