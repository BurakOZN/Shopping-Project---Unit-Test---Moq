using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Manager
{
    public class DeliveryCostCalculator
    {
        private readonly decimal _costPerDelivery;
        private readonly decimal _costPerProduct;
        private readonly decimal _fixedCost;
        public DeliveryCostCalculator(decimal costPerDelivery, decimal costPerProduct, decimal fixedCost)
        {
            _costPerDelivery = costPerDelivery;
            _costPerProduct = costPerProduct;
            _fixedCost = fixedCost;

        }
        public double calculateFor(Cart cart)
        {
            var NumberOfDeliveries = cart.Products.GroupBy(x => x.Product.CategoryId).Count();
            var NumberOfProducts = cart.Products.GroupBy(x => x.Product.Id).Count();
            return (double)(_costPerDelivery * NumberOfDeliveries+_costPerProduct* NumberOfProducts+_fixedCost);
        }
    }
}
