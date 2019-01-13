using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClassCoin.Models
{
    public class Reward
    {
        public int RewardID { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}