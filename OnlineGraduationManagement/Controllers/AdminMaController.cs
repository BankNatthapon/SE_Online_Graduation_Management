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
    public class AdminMaController : Controller
    {
        // GET: AdminMa
        [SystemAuthen]
        [AdminOnly]
        public ActionResult Index()
        {
            string pConStr, sql;
            pConStr = WebConfigurationManager.ConnectionStrings["DB"].ToString();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand();
            com.CommandType = CommandType.Text;
            sql = "select Std_pwd,Std_Id,Std_fac_order,Std_dep,hornor_name,Std_grade,Std_fname,Std_lname,fac_name,Department.dep_id,dep_name from Student ";
            sql = sql + " inner join Department on Student.Std_dep = Department.dep_id";
            sql = sql + " inner join Faculty on Faculty.fac_id = Department.fac_id";
            sql = sql + " inner join Hornor on Hornor.hornor_id = Student.Std_hornor";
            sql = sql + " Order by Std_fac_order, Std_dep, Std_hornor, Case when Std_hornor< 3";
            sql = sql + " then Std_grade else '' End Desc, Case when Std_hornor = 3 then Std_fname else '' End Asc";
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
                u.Std_Id = drw["Std_Id"].ToString();
                u.Std_fac_order = drw["Std_fac_order"].ToString();
                u.Std_dep = drw["Std_dep"].ToString();
                u.hornor_name = drw["hornor_name"].ToString();
                u.Std_grade = drw["Std_grade"].ToString();
                u.Std_fname = drw["Std_fname"].ToString();
                u.Std_lname = drw["Std_lname"].ToString();
                u.fac_name = drw["fac_name"].ToString();
                u.dep_name = drw["dep_name"].ToString();
                u.dep_id = drw["dep_id"].ToString();
                u.Std_pwd = drw["Std_pwd"].ToString();
                model.Add(u);
            }
            return View(model);
        }

        [SystemAuthen]
        [AdminOnly]
        [HttpPost]
        public ActionResult EditStudent(string id, string fname, string lname, string password)
        {
            try
            {
                string pConStr, sql;
                pConStr = WebConfigurationManager.ConnectionStrings["DB"].ToString();
                DataTable dt = new DataTable();
                SqlCommand com = new SqlCommand();
                com.CommandType = CommandType.Text;
                sql = "update Student set Std_fname = " + "'" + fname + "'" + ",Std_lname = " + "'" + lname + "'" + ",Std_pwd = '" + password + "' where Std_Id = '" + id + "'";
                com.CommandText = sql;
                SqlDataReader dr;
                SqlConnection con = new SqlConnection(pConStr);
                com.CommandTimeout = 60;
                com.Connection = con;
                con.Open();
                dr = com.ExecuteReader();
                dt.Load(dr);
                con.Close();
                return RedirectToAction("Index");
            }
            catch
            {

                return View();
            }
        }

        [SystemAuthen]
        [AdminOnly]
        [HttpPost]
        public ActionResult DeleteStudent(string id1)
        {
            try
            {
                string pConStr, sql;
                pConStr = WebConfigurationManager.ConnectionStrings["DB"].ToString();
                DataTable dt = new DataTable();
                SqlCommand com = new SqlCommand();
                com.CommandType = CommandType.Text;
                sql = "delete from Student where Std_Id ='" + id1 + "'";
                com.CommandText = sql;
                SqlDataReader dr;
                SqlConnection con = new SqlConnection(pConStr);
                com.CommandTimeout = 60;
                com.Connection = con;
                con.Open();
                dr = com.ExecuteReader();
                dt.Load(dr);
                con.Close();
                return RedirectToAction("Index");
            }
            catch
            {

                return View();
            }
        }
    }
}