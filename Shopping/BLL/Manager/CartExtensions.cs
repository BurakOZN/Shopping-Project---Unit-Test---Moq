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

        public static Campaign ApplyDiscounts(this Cart cart, List<Campaign> campaigns)
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
            var result= keyValuePairs.OrderByDescending(x=>x.Value);
            return result.FirstOrDefault().Key;
        }

        private static decimal CalculateCampaign(this List<CartProduct> cartProduct,Campaign campaign)
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
    }
}
