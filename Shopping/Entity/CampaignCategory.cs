using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class CampaignCategory
    {
        public string CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }

        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }

    }
}
