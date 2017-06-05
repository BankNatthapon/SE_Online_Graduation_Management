using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;
using OnlineGraduationManagement.Models;
using OnlineGraduationManagement.Filter;

namespace OnlineGraduationManagement.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [SystemAuthen]
        public ActionResult Index()
        {
            string pConStr, sql;
            pConStr = WebConfigurationManager.ConnectionStrings["DB"].ToString();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand();
            com.CommandType = CommandType.Text;
            sql = "select Std_pwd,Std_Id,Std_fac_order,Std_dep,hornor_name,Std_grade,Std_fname,Std_lname,fac_name,Department.dep_id,dep_name,Seat_First.seat_name as seatname,Seat_First.seat_status as seatstatus , Seat_Second.seat_name as seatname2,Seat_Second.seat_status as seatstatus2 from Student ";
            sql = sql + " inner join Department on Student.Std_dep = Department.dep_id";
            sql = sql + " inner join Faculty on Faculty.fac_id = Department.fac_id";
            sql = sql + " inner join Hornor on Hornor.hornor_id = Student.Std_hornor";
            sql = sql + " inner join Seat_First on Seat_First.seat_std_id = Student.id";
            sql = sql + " inner join Seat_Second on Seat_Second.seat_std_id = Student.id";
            sql = sql + " where Student.Std_Id = '"+ Session["Std_Id"] + "'";
            com.CommandText = sql;
            SqlDataReader dr;
            SqlConnection con = new SqlConnection(pConStr);
            com.CommandTimeout = 60;
            com.Connection = con;
            con.Open();
            dr = com.ExecuteReader();
            dt.Load(dr);
            con.Close();

            var model = new List<Student>();
            foreach (DataRow drw in dt.Rows)
            {
                var u = new Student();
                Session["Std_Id"] = drw["Std_Id"].ToString();
                Session["Std_fac_order"] = drw["Std_fac_order"].ToString();
                Session["Std_dep"] = drw["Std_dep"].ToString();
                Session["hornor_name"] = drw["hornor_name"].ToString();
                Session["Std_fname"] = drw["Std_fname"].ToString();
                Session["Std_lname"] = drw["Std_lname"].ToString();
                Session["fac_name"] = drw["fac_name"].ToString();
                Session["dep_name"] = drw["dep_name"].ToString();

                Session["seat_name"] = drw["seatname"].ToString();
                Session["status"] = drw["seatstatus"].ToString();

                Session["seat_name2"] = drw["seatname2"].ToString();
                Session["status2"] = drw["seatstatus2"].ToString();
                model.Add(u);
            }
            return View(model);
        }
    }
}