using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClassCoin.Models
{
    public class Classroom
    {
        public int ClassroomID { get; set; }
        public string ClassCode { get; set; } //of the form 13A4FE1
        public string ClassName { get; set; }
        public string ClassSubject { get; set; }
        public List<int> ClassGrades { get; set; }
        public virtual ICollection<Instructor> Instructors { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Group> Groups { get; set; }       
        public Store Store { get; set; }
    }
}