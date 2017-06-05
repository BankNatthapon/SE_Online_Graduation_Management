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
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection c)
        {
            try
            {
                string pConStr, sql;
                pConStr = WebConfigurationManager.ConnectionStrings["DB"].ToString(); // การเชื่อมต่อ database
                DataTable dt = new DataTable();// เอาคลาส table มาประกาศเป็นชื่อ dt
                DataTable dt1 = new DataTable();
                SqlCommand com = new SqlCommand();// เอาไว้ยัด sqlcommand ไว้ในนี้
                com.CommandType = CommandType.Text;
                SqlDataReader dr, dr1; // เตรียมรับผลจาก Data
                SqlConnection con = new SqlConnection(pConStr); // เตรียมเชื่อมต่อ
                com.CommandTimeout = 60; // กำหนดเวลาที่จะ timeout นั่นคือ 60
                com.Connection = con;
                con.Open();
                sql = "select * from Admin where Admin_Id ='"+c["usr"]+"' and Password = '"+ c["pwd"] + "'";
                com.CommandText = sql;
                dr = com.ExecuteReader();
                dt.Load(dr);

                if (dt.Rows.Count == 0) // ไม่มี user แลพ password ที่ match กัน จะเด้งกลับไปหน้าเดิม
                {
                    dt.Clear();
                    con.Close();
                    pConStr = WebConfigurationManager.ConnectionStrings["DB"].ToString();
                    com.CommandType = CommandType.Text;
                    com.CommandTimeout = 60; // กำหนดเวลาที่จะ timeout นั่นคือ 60
                    com.Connection = con;
                    con.Open();
                    //insert Customer
                    sql = "select * from Student where Std_Id ='"+c["usr"]+"' and Std_pwd ='"+c["pwd"]+"'";
                    com.CommandText = sql;
                    dr1 = com.ExecuteReader();
                    dt1.Load(dr1);
                    if (dt1.Rows.Count == 0)
                    {
                        return View("Index");
                    }
                    foreach (DataRow drw1 in dt1.Rows)    // เอา username เข้าไปเก็บใน session
                    {
                        Session["id"] = drw1["Std_Id"].ToString();
                        Session["Std_Id"] = drw1["Std_Id"].ToString();
                        Session["Au_Status"] = drw1["Au_Status"].ToString();
                        Session["Std_fname"] = drw1["Std_fname"].ToString();
                        Session["Std_lname"] = drw1["Std_lname"].ToString();
                    }
                    dt.Clear();
                    con.Close();
                    Session["usr"] = c["usr"];
                    return RedirectToAction("Index", "User");
                }

                foreach (DataRow drw in dt.Rows)    // เอา username เข้าไปเก็บใน session
                {
                    Session["Admin_Id"] = drw["Admin_Id"].ToString();
                    Session["Au_Status"] = drw["Au_Status"].ToString();
                    Session["Name"] = drw["Name"].ToString();
                   
                }
                Session["usr"] = c["usr"];
                con.Close();
                return RedirectToAction("Index", "AdminMa"); // ถ้าทำสำเร็จแล้วให้ไปที่หน้า userlogin
            }
            catch
            {
                return View("Index");
            }

        }

        public ActionResult Logoff()
        {
           
            Session.Abandon();
            return RedirectToAction("Index","Home");
        }
    }
}