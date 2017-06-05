using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnlineGraduationManagement.Models
{
    public class Student
    {
        public string Std_Id { get; set; }
        public string Std_fname { get; set; }
        public string Std_lname { get; set; }
        public string Std_fac { get; set; }
        public string fac_id { get; set; }
        public string fac_name { get; set; }

        public string Std_grade { get; set; }
        public string hornor_name { get; set; }
        public string dep_name { get; set; }
        public string Std_pwd { get; set; }
        public string Std_fac_order { get; set; }
        public string Std_dep { get; set; }
        public string dep_id { get; set; }

        public string id { get; set; }
        public string seat_id { get; set; }
        public string seat_std_id { get; set; }
        public string seat_name { get; set; }
        public string seat_status { get; set; }

        public string seat_id2 { get; set; }
        public string seat_std_id2 { get; set; }
        public string seat_name2 { get; set; }
        public string seat_status2 { get; set; }
    }
}