using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClassCoin.Models
{
    public class Store
    {
        public int StoreID { get; set; }
        public virtual ICollection<Reward> Rewards { get; set; }
    }
}