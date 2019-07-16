using Entity;
using System;
using System.Collections.Generic;
using System.Data;
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

        public static double ApplyDiscount(this Cart cart, Coupon coupon = null, Campaign campaign = null)
        {
            double total = cart.GetTotal();

            if (campaign == null && coupon == null)
                return 0;
            if (campaign == null)
                return total > coupon.Limit ? (coupon.DiscountType == DiscountType.Rate ? (coupon.Value / 100) * total : coupon.Value) : 0;
            if (coupon == null)
                return (double)cart.ApplyDiscounts(new List<Campaign>() { campaign }).Value;
            var campDis = (double)cart.ApplyDiscounts(new List<Campaign>() { campaign }).Value;
            total -= campDis;
            var result = total > coupon.Limit ? (coupon.DiscountType == DiscountType.Rate ? (coupon.Value / 100) * total : coupon.Value) : 0;
            return result + campDis;

        }
        public static double GetTotal(this Cart cart)
        {
            double total = 0;
            foreach (var item in cart.Products)
            {
                total += (double)(item.Quantity * item.Product.Price);
            }
            return total;
        }

        public static double getTotalAmountAfterDiscounts(this Cart cart, Campaign campaign = null, Coupon coupon = null)
        {
            return (cart.GetTotal() - cart.ApplyDiscount(coupon, campaign));

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

        public static bool Print(this Cart cart, Campaign campaign, Coupon coupon)
        {

            double Total = cart.GetTotal();
            var campaignDiscount = cart.getCampaignDiscount(campaign);

            var amauntTotal = cart.getTotalAmountAfterDiscounts(campaign, coupon);
            var couponDiscount = Total - campaignDiscount - amauntTotal;

            //For table
            DataTable productTable = new DataTable();

            //Define columns
            productTable.Columns.Add("Kategori Adı");
            productTable.Columns.Add("Ürün Adı");
            productTable.Columns.Add("Miktar");
            productTable.Columns.Add("Birim Fiyat");
            productTable.Columns.Add("Toplam Fiyat");

            var cx = cart.Products.GroupBy(x => x.Product.CategoryId);
            var list = new List<CartProduct>();
            foreach (var item in cx)
            {
                decimal categoryTotal = 0;
                foreach (var cartProduct in item)
                {
                    categoryTotal += cartProduct.Product.Price * cartProduct.Quantity;
                    productTable.Rows.Add(cartProduct.Product.Category.Title, cartProduct.Product.Title, cartProduct.Quantity, cartProduct.Product.Price.ToString(), cartProduct.Quantity * cartProduct.Product.Price);
                }
                productTable.Rows.Add("", "", "", "", categoryTotal);
            }
            productTable.Rows.Add("", "", "", "Toplam", Total);
            productTable.Rows.Add("", "", "", "Kampanya ", "-" + campaignDiscount);
            productTable.Rows.Add("", "", "", "Kupon", "-" + (couponDiscount));
            productTable.Rows.Add("", "", "", "Ödeme Tutarı", amauntTotal);

            try
            {
                DataTable dtbl = productTable;
                IPrint print = new PrintPDF();
                print.ExportDataTableToPdf(dtbl, @"C:\Datalar\test.pdf", "Ürün Listesi");

            }
            catch (Exception ex)
            {
                string exc = ex.Message;
                return false;
            }
            return true;
        }

    }
}
