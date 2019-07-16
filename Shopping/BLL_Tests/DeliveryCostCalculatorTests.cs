using Entity;
using NUnit.Framework;
using BLL.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Tests
{
    [TestFixture]
    public class DeliveryCostCalculatorTests
    {
        [Test]
        public void DeliveryCostCalculator_CalculateFor3Product_Returns()
        {
            var category = new Category("food");
            category.SubCategory = new List<Category>() { new Category("Sub1"), new Category("Sub2") };

            var product = new Product("Apple", 100.0m, category.SubCategory[0]);
            var product3 = new Product("Banana", 50.0m, category.SubCategory[0]);
            var product2 = new Product("Almonds", 150.0m, category.SubCategory[1]);
            var cart = new Cart("TestCart");

            cart.AddProduct(product, 3);
            cart.AddProduct(product3, 2);
            cart.AddProduct(product2, 5);

            DeliveryCostCalculator calculator = new DeliveryCostCalculator(1, 2, 3);
            var data = calculator.calculateFor(cart);

            Assert.That(data, Is.EqualTo(11));

        }
    }
}
