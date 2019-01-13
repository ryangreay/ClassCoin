using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClassCoin.Models
{
    public class Group
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Classroom> Classrooms { get; set; }
    }
}