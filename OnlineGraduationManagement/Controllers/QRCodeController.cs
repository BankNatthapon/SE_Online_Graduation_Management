using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neodynamic.Web.MVC.Barcode;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;
using OnlineGraduationManagement.Models;
using OnlineGraduationManagement.Filter;

namespace OnlineGraduationManagement.Controllers
{
    public class QRCodeController : Controller
    {
        // GET: QRCode
        [SystemAuthen]
        [AdminOnly]
        public ActionResult Index()
        {
            string pConStr, sql;
            pConStr = WebConfigurationManager.ConnectionStrings["DB"].ToString(); // การเชื่อมต่อ database
            DataTable dt = new DataTable();// เอาคลาส table มาประกาศเป็นชื่อ dt
            SqlCommand com = new SqlCommand();// เอาไว้ยัด sqlcommand ไว้ในนี้
            com.CommandType = CommandType.Text;
            sql = "Select * from Faculty";
            com.CommandText = sql;
            SqlDataReader dr; // เตรียมรับผลจาก Data
            SqlConnection con = new SqlConnection(pConStr); // เตรียมเชื่อมต่อ
            com.CommandTimeout = 60; // กำหนดเวลาที่จะ timeout นั่นคือ 60
            com.Connection = con;
            con.Open();
            dr = com.ExecuteReader();
            dt.Load(dr);
            con.Close();

            var model = new List<Student>(); // สร้าง model ให้รับเป็น list
            foreach (DataRow drw in dt.Rows)
            {
                var u = new Student();
                u.fac_id = drw["fac_id"].ToString();
                u.fac_name = drw["fac_name"].ToString();
                model.Add(u); // ยัดข้อมูลของ u เข้าไปใน model เรื่ยๆ จนกลายเป็น list
            }
            return View(model);
        }
        [SystemAuthen]
        [AdminOnly]
        public ActionResult Barcode(string id)
        {
            string pConStr, sql;
            pConStr = WebConfigurationManager.ConnectionStrings["DB"].ToString(); // การเชื่อมต่อ database
            DataTable dt = new DataTable();// เอาคลาส table มาประกาศเป็นชื่อ dt
            SqlCommand com = new SqlCommand();// เอาไว้ยัด sqlcommand ไว้ในนี้
            com.CommandType = CommandType.Text;
            sql = "Select Student.Std_Id,Student.Std_lname,Student.Std_fname,Faculty.fac_name from Student INNER JOIN Faculty ON Student.Std_fac = Faculty.fac_id where Student.Std_fac = '" + id +"'";
            com.CommandText = sql;
            SqlDataReader dr; // เตรียมรับผลจาก Data
            SqlConnection con = new SqlConnection(pConStr); // เตรียมเชื่อมต่อ
            com.CommandTimeout = 60; // กำหนดเวลาที่จะ timeout นั่นคือ 60
            com.Connection = con;
            con.Open();
            dr = com.ExecuteReader();
            dt.Load(dr);
            con.Close();

            var model = new List<Student>(); // สร้าง model ให้รับเป็น list
            foreach (DataRow drw in dt.Rows)
            {
                var u = new Student();
                u.Std_Id = drw["Std_Id"].ToString();
                u.Std_fname = drw["Std_fname"].ToString();
                u.Std_lname = drw["Std_lname"].ToString();
                u.fac_name = drw["fac_name"].ToString();
                ViewBag.name = drw["fac_name"].ToString();
                model.Add(u); // ยัดข้อมูลของ u เข้าไปใน model เรื่ยๆ จนกลายเป็น list
            }
            return View(model);
        }
        [SystemAuthen]
        [AdminOnly]
        public ActionResult QRcode(string id)
        {
            string pConStr, sql;
            pConStr = WebConfigurationManager.ConnectionStrings["DB"].ToString(); // การเชื่อมต่อ database
            DataTable dt = new DataTable();// เอาคลาส table มาประกาศเป็นชื่อ dt
            SqlCommand com = new SqlCommand();// เอาไว้ยัด sqlcommand ไว้ในนี้
            com.CommandType = CommandType.Text;
            sql = "Select Student.Std_Id,Student.Std_lname,Student.Std_fname,Faculty.fac_name from Student INNER JOIN Faculty ON Student.Std_fac = Faculty.fac_id where Student.Std_fac = '" + id + "'";
            com.CommandText = sql;
            SqlDataReader dr; // เตรียมรับผลจาก Data
            SqlConnection con = new SqlConnection(pConStr); // เตรียมเชื่อมต่อ
            com.CommandTimeout = 60; // กำหนดเวลาที่จะ timeout นั่นคือ 60
            com.Connection = con;
            con.Open();
            dr = com.ExecuteReader();
            dt.Load(dr);
            con.Close();

            var model = new List<Student>(); // สร้าง model ให้รับเป็น list
            foreach (DataRow drw in dt.Rows)
            {
                var u = new Student();
                u.Std_Id = drw["Std_Id"].ToString();
                u.Std_fname = drw["Std_fname"].ToString();
                u.Std_lname = drw["Std_lname"].ToString();
                u.fac_name = drw["fac_name"].ToString();
                ViewBag.name = drw["fac_name"].ToString();
                model.Add(u); // ยัดข้อมูลของ u เข้าไปใน model เรื่ยๆ จนกลายเป็น list
            }
            return View(model);
        }
        public void GetBarcodeImage(string valueToEncode)
        {
            //Create an instance of BarcodeProfessional class
            using (Neodynamic.Web.MVC.Barcode.BarcodeProfessional bcp = new Neodynamic.Web.MVC.Barcode.BarcodeProfessional())
            {
                //Set the desired barcode type or symbology
                bcp.Symbology = Neodynamic.Web.MVC.Barcode.Symbology.QRCode;
                //Set value to encode
                bcp.Code = valueToEncode;
                //Generate barcode image
                byte[] imgBuffer = bcp.GetBarcodeImage(System.Drawing.Imaging.ImageFormat.Png);
                //Write image buffer to Response obj
                System.Web.HttpContext.Current.Response.ContentType = "image/png";
                System.Web.HttpContext.Current.Response.BinaryWrite(imgBuffer);
            }
        }
    }
}