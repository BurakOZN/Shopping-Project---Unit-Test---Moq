using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class Cart : BaseEntity
    {
        public Cart(string name)
        {
            Name = name;
            Products = new List<CartProduct>();
        }
        public string Name { get; set; }

        public virtual List<CartProduct> Products { get; set; }
    }
}
