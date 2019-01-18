using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassCoin.Models
{
    public class Instructor
    {
        public int InstructorID { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public string Institution { get; set; }
        public virtual ICollection<Classroom> Classrooms { get; set; }
    }
}