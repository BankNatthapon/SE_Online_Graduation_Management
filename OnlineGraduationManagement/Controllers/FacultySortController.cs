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
    public class FacultySortController : Controller
    {
        // GET: FaSort
        [SystemAuthen]
        [AdminOnly]
        public ActionResult Index()
        {
            ViewBag.Alert = null;
            string pConStr, sql;
            pConStr = WebConfigurationManager.ConnectionStrings["DB"].ToString(); // การเชื่อมต่อ database
            DataTable dt = new DataTable();// เอาคลาส table มาประกาศเป็นชื่อ dt
            SqlCommand com = new SqlCommand();// เอาไว้ยัด sqlcommand ไว้ในนี้
            com.CommandType = CommandType.Text;
            sql = "select fac_id,fac_name from Faculty";
            com.CommandText = sql;
            SqlDataReader dr; // เตรียมรับผลจาก Data
            SqlConnection con = new SqlConnection(pConStr); // เตรียมเชื่อมต่อ
            com.CommandTimeout = 60; // กำหนดเวลาที่จะ timeout นั่นคือ 60
            com.Connection = con;
            con.Open();
            dr = com.ExecuteReader();
            dt.Load(dr);
            con.Close();

            var model = new List<Faculty>(); // สร้าง model ให้รับเป็น list
            foreach (DataRow drw in dt.Rows)
            {
                var u = new Faculty();
                u.fac_id = drw["fac_id"].ToString();
                u.fac_name = drw["fac_Name"].ToString();
                ViewBag.fac_id = drw["fac_id"].ToString();
                model.Add(u); // ยัดข้อมูลของ u เข้าไปใน model เรื่ยๆ จนกลายเป็น list
            }
            return View(model);
        }
        [SystemAuthen]
        [AdminOnly]
        [HttpPost]
        public ActionResult sort()
        {
            var k =Request.Form["id"];

            
                string pConStr, sql="";
                pConStr = WebConfigurationManager.ConnectionStrings["DB"].ToString(); // การเชื่อมต่อ database
                DataTable dt = new DataTable();// เอาคลาส table มาประกาศเป็นชื่อ dt
                SqlCommand com = new SqlCommand();// เอาไว้ยัด sqlcommand ไว้ในนี้
            com.CommandType = CommandType.Text;
            for (var i = 1; i <= 4; i++)
            {
                sql = sql + " update Student set Std_fac_order='"+ Request.Form[i.ToString()] + "' where Std_fac = '" + i+"'";
            }
            com.CommandText = sql;
                SqlDataReader dr; // เตรียมรับผลจาก Data
                SqlConnection con = new SqlConnection(pConStr); // เตรียมเชื่อมต่อ
                com.CommandTimeout = 60; // กำหนดเวลาที่จะ timeout นั่นคือ 60
                com.Connection = con;
                con.Open();
                dr = com.ExecuteReader();
                dt.Load(dr);
                con.Close();


            return RedirectToAction("Index","AdminMa");
        }
    }
}