using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Manager
{
    public static class CartExtensions
    {
        public static Cart AddProduct(this Cart cart, Product product, int Quantity)
        {
            var productCart = cart.Products.FirstOrDefault(x => x.ProductId == product.Id);
            if (productCart != null)
                productCart.Quantity += Quantity;
            else
                cart.Products.Add(new CartProduct() { ProductId = product.Id, Quantity = Quantity, Product = product, Cart = cart, CartId = cart.Id });
            return cart;
        }
        /// <summary>
        /// Campanya ile kategorileri ayıklar sadece o kampanyaya ait kategoriye uygular
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="campaigns"></param>
        /// <returns></returns>
        public static KeyValuePair<Campaign, decimal> ApplyDiscounts(this Cart cart, List<Campaign> campaigns)
        {
            Campaign campaign = null;
            Dictionary<Campaign, decimal> keyValuePairs = new Dictionary<Campaign, decimal>();

            foreach (var item in campaigns)
            {
                int i = 0;
                List<CartProduct> cartProducts = new List<CartProduct>();
                foreach (var cat in item.Categories)
                {
                    foreach (var pro in cart.Products)
                    {
                        if (cat.CategoryId == pro.Product.CategoryId)
                        {
                            i += pro.Quantity;
                            cartProducts.Add(pro);
                        }
                    }
                }
                if (item.DiscountParameter <= i)
                {
                    decimal value = cartProducts.CalculateCampaign(item);

                    keyValuePairs.Add(item, value);
                }

            }
            var result = keyValuePairs.OrderByDescending(x => x.Value).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Kampanyayı yüm ürünlere uygular
        /// </summary>
        /// <param name="cartProduct"></param>
        /// <param name="campaign"></param>
        /// <returns></returns>
        private static decimal CalculateCampaign(this List<CartProduct> cartProduct, Campaign campaign)
        {
            decimal value = 0;
            if (campaign.DiscountType == DiscountType.Amount)
                return campaign.DiscountValue;
            foreach (var camp in cartProduct)
            {
                value += camp.Quantity * camp.Product.Price * (campaign.DiscountValue / 100);
            }
            return value;
        }

        public static decimal ApplyDiscount(this Cart cart, Coupon coupon = null, Campaign campaign = null)
        {
            decimal total = 0;
            foreach (var item in cart.Products)
            {
                total += item.Quantity * item.Product.Price;
            }
            if (campaign == null && coupon == null)
                return total;
            if (campaign == null)
                return total > coupon.Limit ? (coupon.DiscountType == DiscountType.Rate ? (1 - coupon.Value / 100) * total : total - coupon.Value) : total;
            if (coupon == null)
                return total - cart.ApplyDiscounts(new List<Campaign>() { campaign }).Value;
            total -= cart.ApplyDiscounts(new List<Campaign>() { campaign }).Value;
            return total > coupon.Limit ? (coupon.DiscountType == DiscountType.Rate ? (1 - coupon.Value / 100) * total : total - coupon.Value) : total;

        }

        public static double getTotalAmountAfterDiscounts(this Cart cart, Campaign campaign = null, Coupon coupon = null)
        {
            return (double)cart.ApplyDiscount(coupon, campaign);

        }
        public static double getCouponDiscount(this Cart cart, Coupon coupon)
        {
            return (double)cart.ApplyDiscount(coupon);
        }
        public static double getCampaignDiscount(this Cart cart, Campaign campaign)
        {
            return (double)cart.ApplyDiscount(null, campaign);
        }
        public static double getDeliveryCost(this Cart cart, decimal costPerDelivery, decimal costPerProduct, decimal fixedCost)
        {
            return (new DeliveryCostCalculator(costPerDelivery, costPerProduct, fixedCost)).calculateFor(cart);
        }




    }
}
