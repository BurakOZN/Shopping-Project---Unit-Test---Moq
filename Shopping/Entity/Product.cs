using System;
using System.Collections.Generic;

namespace Entity
{
    public class Product : BaseEntity
    {
        public Product()
        {

        }
        public Product(string title,decimal price, Category _category)
        {
            Title = title;
            Price = price;
            CategoryId = _category.Id;
            Category = _category;
        }
        public string Title { get; set; }
        public decimal Price { get; set; }

        //Category foreign key
        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual List<CartProduct> Carts { get; set; }

    }
}
