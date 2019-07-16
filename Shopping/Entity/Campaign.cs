using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class Campaign : BaseEntity
    {
        public Campaign(Category category, decimal value, int parameter, DiscountType type)
        {
            DiscountValue = value;
            DiscountParameter = parameter;
            DiscountType = type;
            Categories = new List<CampaignCategory>();
            Categories.Add(new CampaignCategory() { CampaignId = base.Id, CategoryId = category.Id, Category = category, Campaign = this });
            foreach (var item in CategoryManager.FindCategories(category))
            {
                Categories.Add(new CampaignCategory() { CampaignId = base.Id, CategoryId = item.Id, Campaign=this,Category=item });
            }

        }
        public decimal DiscountValue { get; set; }
        public int DiscountParameter { get; set; }
        public DiscountType DiscountType { get; set; }

        public virtual List<CampaignCategory> Categories { get; set; }
    }
    public static class CategoryManager
    {
        public static List<Category> FindCategories(Category category)
        {
            List<Category> categories = new List<Category>();
            if (category.SubCategory == null)
                return categories;
            foreach (var item in category.SubCategory)
            {
                if (item.SubCategory == null)
                    categories.Add(item);
                else
                    categories.AddRange(FindCategories(item));
            }
            return categories;
        }
    }
    public enum DiscountType
    {
        Rate,
        Amount
    }
}
