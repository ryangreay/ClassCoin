using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClassCoin.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public string Description { get; set; }
        public bool Pending { get; set; }
    }
}