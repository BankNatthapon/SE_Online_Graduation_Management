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
    public class SeatController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GenSeat(string row, string col)
        {
            string pConStr, sql;
            pConStr = WebConfigurationManager.ConnectionStrings["DB"].ToString();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand();
            com.CommandType = CommandType.Text;
            SqlDataReader dr;
            SqlConnection con = new SqlConnection(pConStr);

            sql = "select id from Student Order by Std_fac_order, Std_dep, Std_hornor, Case when Std_hornor< 3 then Std_grade else '' End Desc , Case when Std_hornor = 3 then Std_fname else '' End Asc";

            com.CommandText = sql;
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

                u.id = drw["id"].ToString();

                model.Add(u);
            }

            var seat_row = int.Parse(row);
            var seat_col = int.Parse(col);

            var num = 1;
            var num1 = 0;
            var num2 = 0;
            string[] Seat_Name = new string[seat_row * seat_col];
            string[] Seat_Name_st = new string[] { "AA", "AB", "AC" ,"AD","AE","AF","AG","AH","AI","AJ","AK","AL","AM","AN","AO","AP","AQ","AR","AS","AT","AU","AV","AW","AX","AY","AY"
                                                ,"BA", "BB", "BC" ,"BD","BE","BF","BG","BH","BI","BJ","BK","BL","BM","BN","BO","BP","BQ","BR","BS","BT","BU","BV","BW","BX","BY","BY"};

            for (var i = 1; i <= seat_row; i++)
            {
                for (var j = 1; j <= seat_col; j++)
                {
                    Seat_Name[num2++] = Seat_Name_st[num1] + num;
                    ++num;
                }
                num = 1;
                num1 = num1 + 1;
            }

            var seat_count = 0;
            var seat_number = 1;

            sql = "delete from Seat_First ";

            foreach (var k in model)
            {
                sql = sql + " insert into Seat_First ([seat_id], [seat_std_id], [seat_name], [seat_status]) Values ('" + seat_number + "', '" + k.id + "', '" + Seat_Name[seat_count] + "', 'UnCheck') ";

                com.CommandText = sql;
                com.CommandTimeout = 60;
                com.Connection = con;
                con.Open();
                dr = com.ExecuteReader();
                dt.Load(dr);
                con.Close();

                seat_count++;
                seat_number++;
            }

            Session["row"] = row;
            Session["col"] = col;

            return RedirectToAction("ShowSeat", "Seat", new { row = Session["row"], col = Session["col"] });
        }

        public ActionResult GenSeatSecond(string row, string col)
        {
            string pConStr, sql;
            pConStr = WebConfigurationManager.ConnectionStrings["DB"].ToString();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand();
            com.CommandType = CommandType.Text;
            SqlDataReader dr;
            SqlConnection con = new SqlConnection(pConStr);

            sql = " select seat_std_id as id from Seat_First where seat_status = 'Check'";

            com.CommandText = sql;
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

                u.id = drw["id"].ToString();

                model.Add(u);
            }

            var seat_row = int.Parse(row);
            var seat_col = int.Parse(col);

            var num = 1;
            var num1 = 0;
            var num2 = 0;
            string[] Seat_Name = new string[seat_row * seat_col];
            string[] Seat_Name_st = new string[] { "AA", "AB", "AC" ,"AD","AE","AF","AG","AH","AI","AJ","AK","AL","AM","AN","AO","AP","AQ","AR","AS","AT","AU","AV","AW","AX","AY","AY"
                                                ,"BA", "BB", "BC" ,"BD","BE","BF","BG","BH","BI","BJ","BK","BL","BM","BN","BO","BP","BQ","BR","BS","BT","BU","BV","BW","BX","BY","BY"};

            for (var i = 1; i <= seat_row; i++)
            {
                for (var j = 1; j <= seat_col; j++)
                {
                    Seat_Name[num2++] = Seat_Name_st[num1] + num;
                    ++num;
                }
                num = 1;
                num1 = num1 + 1;
            }

            var seat_count = 0;
            var seat_number = 1;

            sql = "delete from Seat_Second ";

            foreach (var k in model)
            {
                sql = sql + " insert into Seat_Second ([seat_id], [seat_std_id], [seat_name], [seat_status]) Values ('" + seat_number + "', '" + k.id + "', '" + Seat_Name[seat_count] + "', 'UnCheck') ";

                com.CommandText = sql;
                com.CommandTimeout = 60;
                com.Connection = con;
                con.Open();
                dr = com.ExecuteReader();
                dt.Load(dr);
                con.Close();

                seat_count++;
                seat_number++;
            }

            Session["row"] = row;
            Session["col"] = col;

            return RedirectToAction("ShowSeatSecond", "Seat", new { row = Session["row"], col = Session["col"] });
        }

        public ActionResult ShowSeat(string row, string col)
        {
            string pConStr, sql;
            pConStr = WebConfigurationManager.ConnectionStrings["DB"].ToString();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand();
            com.CommandType = CommandType.Text;
            SqlDataReader dr;
            SqlConnection con = new SqlConnection(pConStr);

            var seat_row = int.Parse(row);
            var seat_col = int.Parse(col);

            sql = "select * from Seat_First";

            com.CommandText = sql;
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
                u.seat_id = drw["seat_id"].ToString();
                u.seat_std_id = drw["seat_std_id"].ToString();
                u.seat_name = drw["seat_name"].ToString();
                u.seat_status = drw["seat_status"].ToString();

                model.Add(u);
            }

            ViewBag.row = seat_row;
            ViewBag.col = seat_col;

            return View(model);
        }

        public ActionResult ShowSeatSecond(string row, string col)
        {
            string pConStr, sql;
            pConStr = WebConfigurationManager.ConnectionStrings["DB"].ToString();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand();
            com.CommandType = CommandType.Text;
            SqlDataReader dr;
            SqlConnection con = new SqlConnection(pConStr);

            var seat_row = int.Parse(row);
            var seat_col = int.Parse(col);

            sql = "select * from Seat_Second";

            com.CommandText = sql;
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
                u.seat_id = drw["seat_id"].ToString();
                u.seat_std_id = drw["seat_std_id"].ToString();
                u.seat_name = drw["seat_name"].ToString();
                u.seat_status = drw["seat_status"].ToString();

                model.Add(u);
            }

            ViewBag.row = seat_row;
            ViewBag.col = seat_col;

            return View(model);
        }

        public ActionResult ShowData(int id)
        {
            string pConStr, sql;
            pConStr = WebConfigurationManager.ConnectionStrings["DB"].ToString();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand();
            com.CommandType = CommandType.Text;
            SqlDataReader dr;
            SqlConnection con = new SqlConnection(pConStr);

            sql = "select * from Student where id = '" + id + "' ";

            com.CommandText = sql;
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
                u.Std_fname = drw["Std_fname"].ToString();
                u.Std_lname = drw["Std_lname"].ToString();
                u.Std_grade = drw["Std_grade"].ToString();
                u.Std_fac = drw["Std_fac"].ToString();
                u.Std_dep = drw["Std_dep"].ToString();
                u.Std_fac_order = drw["Std_fac_order"].ToString();

                Session["Std_fname"] = drw["Std_fname"].ToString();
                Session["Std_lname"] = drw["Std_lname"].ToString();
                Session["Std_grade"] = drw["Std_grade"].ToString();
                Session["Std_fac"] = drw["Std_fac"].ToString();
                Session["Std_dep"] = drw["Std_dep"].ToString();

                model.Add(u);
            }
            Session["Student_Check"] = "OK";

            return RedirectToAction("ShowSeat", "Seat", new { row = Session["row"], col = Session["col"] });
        }

        public ActionResult ShowDataSecond(int id)
        {
            string pConStr, sql;
            pConStr = WebConfigurationManager.ConnectionStrings["DB"].ToString();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand();
            com.CommandType = CommandType.Text;
            SqlDataReader dr;
            SqlConnection con = new SqlConnection(pConStr);

            sql = "select * from Student where id = '" + id + "' ";

            com.CommandText = sql;
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
                u.Std_fname = drw["Std_fname"].ToString();
                u.Std_lname = drw["Std_lname"].ToString();
                u.Std_grade = drw["Std_grade"].ToString();
                u.Std_fac = drw["Std_fac"].ToString();
                u.Std_dep = drw["Std_dep"].ToString();
                u.Std_fac_order = drw["Std_fac_order"].ToString();

                Session["Std_fname"] = drw["Std_fname"].ToString();
                Session["Std_lname"] = drw["Std_lname"].ToString();
                Session["Std_grade"] = drw["Std_grade"].ToString();
                Session["Std_fac"] = drw["Std_fac"].ToString();
                Session["Std_dep"] = drw["Std_dep"].ToString();

                model.Add(u);
            }
            Session["Student_Check"] = "OK";

            return RedirectToAction("ShowSeatSecond", "Seat", new { row = Session["row"], col = Session["col"] });
        }

        public ActionResult DeleteSession()
        {
            Session["Student_Check"] = "NOT OK";
            return RedirectToAction("Index", "Seat");
        }
        [HttpPost]
        public ActionResult CheckSeat(string IdForCheckSeat)
        {
            string pConStr, sql;
            pConStr = WebConfigurationManager.ConnectionStrings["DB"].ToString();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand();
            com.CommandType = CommandType.Text;
            SqlDataReader dr;
            SqlConnection con = new SqlConnection(pConStr);

            sql = "update Seat_First set seat_status = 'Check' where seat_std_id = (select seat_std_id from Student join Seat_First on Student.id = Seat_First.seat_std_id where Student.Std_Id = '" + IdForCheckSeat + "')";

            com.CommandText = sql;
            com.CommandTimeout = 60;
            com.Connection = con;
            con.Open();
            dr = com.ExecuteReader();
            dt.Load(dr);
            con.Close();

            return RedirectToAction("ShowSeat", "Seat", new { row = Session["row"], col = Session["col"] });
        }
        [HttpPost]
        public ActionResult CheckSecondSeat(string IdForCheckSeat)
        {
            string pConStr, sql;
            pConStr = WebConfigurationManager.ConnectionStrings["DB"].ToString();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand();
            com.CommandType = CommandType.Text;
            SqlDataReader dr;
            SqlConnection con = new SqlConnection(pConStr);

            sql = "update Seat_Second set seat_status = 'Check' where seat_std_id = (select seat_std_id from Student join Seat_Second on Student.id = Seat_Second.seat_std_id where Student.Std_Id = '" + IdForCheckSeat + "')";

            com.CommandText = sql;
            com.CommandTimeout = 60;
            com.Connection = con;
            con.Open();
            dr = com.ExecuteReader();
            dt.Load(dr);
            con.Close();

            return RedirectToAction("ShowSeatSecond", "Seat", new { row = Session["row"], col = Session["col"] });
        }
    }
}