using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class Category : BaseEntity
    {
        public Category(string title)
        {
            Title = title;
        }
        public string Title { get; set; }


        //For Circle FK
        public string ParentId { get; set; }
        public virtual Category ParentCategory { get; set; }
        public virtual List<Category> SubCategory { get; set; }


        public virtual List<Product> Products { get; set; }

        public virtual List<CampaignCategory> Campaigns { get; set; }

    }
}
