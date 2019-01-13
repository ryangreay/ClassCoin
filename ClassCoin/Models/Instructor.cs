using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ClassCoin.Models
{
    public class Instructor
    {
        public int InstructorID { get; set; }
        public int UserID { get; set; }
        public string Institution { get; set; }
        public virtual ICollection<Classroom> Classrooms { get; set; }
    }
}