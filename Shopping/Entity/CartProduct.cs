using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class CartProduct
    {
        public int Quantity { get; set; }

        public string CartId { get; set; }
        public virtual Cart Cart { get; set; }

        public string ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
