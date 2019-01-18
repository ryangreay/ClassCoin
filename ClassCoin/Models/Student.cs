using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ClassCoin.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public double Funds { get; set; }
        public virtual ICollection<Reward> Rewards { get; set; }
        public virtual ICollection<Classroom> Classrooms { get; set; }
    }
}