using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class Campaign : BaseEntity
    {
        public decimal DiscountValue { get; set; }
        public int DiscountParameter { get; set; }
        public DiscountType DiscountType { get; set; }

        public virtual List<CampaignCategory> Categories { get; set; }
    }

    public enum DiscountType
    {
        Rate,
        Amount
    }
}
