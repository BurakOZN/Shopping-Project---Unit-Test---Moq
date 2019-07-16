using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class Coupon : BaseEntity
    {
        public Coupon(int limit,double value,DiscountType type)
        {
            Limit = limit;
            Value = value;
            DiscountType = type;
        }
        public double Value { get; set; }
        public int Limit { get; set; }
        public DiscountType DiscountType { get; set; }
    }
}
