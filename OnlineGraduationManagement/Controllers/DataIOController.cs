using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using OnlineGraduationManagement.Models;
using OnlineGraduationManagement.Filter;

using ClosedXML.Excel;
using System.IO;
using System.Configuration;


namespace OnlineGraduationManagement.Controllers
{
    public class DataIOController : Controller
    {
        // GET: DataIO
        [SystemAuthen]
        [AdminOnly]
        public ActionResult Index()
        {
            return View();
        }

        [SystemAuthen]
        [AdminOnly]
        public ActionResult Export(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Student"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.Worksheets.Add(dt, "Sheet1");

                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename=SqlExport.xlsx");
                                using (MemoryStream MyMemoryStream = new MemoryStream())
                                {
                                    wb.SaveAs(MyMemoryStream);
                                    MyMemoryStream.WriteTo(Response.OutputStream);
                                    Response.Flush();
                                    Response.End();
                                }
                            }
                        }
                    }
                }
            }
            return View();
        }

        // =============================== import Data ==========================================
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
        OleDbConnection Econ;

        [SystemAuthen]
        [AdminOnly]
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase file)
        {
            try
            {
                string pConStr, sql;
                pConStr = WebConfigurationManager.ConnectionStrings["DB"].ToString();
                DataTable dt = new DataTable();
                SqlCommand com = new SqlCommand();
                com.CommandType = CommandType.Text;
                sql = "delete from TestIMDB";
                com.CommandText = sql;
                SqlDataReader dr;
                SqlConnection con = new SqlConnection(pConStr);
                com.CommandTimeout = 60;
                com.Connection = con;
                con.Open();
                dr = com.ExecuteReader();
                dt.Load(dr);
                con.Close();

                string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                string filepath = "/excelfolder/" + filename;
                file.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
                InsertExceldata(filepath, filename);
                return RedirectToAction("Index", "DataIO");

            }
            catch
            {

                return View();
            }

            
        }
        private void ExcelConn(string filepath)
        {
            
            string constr = string.Format(@"Provider=Microsoft.ACE.OleDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", filepath);
            Econ = new OleDbConnection(constr);

        }

        private void InsertExceldata(string fileepath, string filename)
        {
            string fullpath = Server.MapPath("/excelfolder/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "Sheet1$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            Econ.Open();

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();
            oda.Fill(ds);

            DataTable dt = ds.Tables[0];

            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "TestIMDB";
            objbulk.ColumnMappings.Add("Std_Id", "Std_Id");
            objbulk.ColumnMappings.Add("Std_fname", "Std_fname");
            objbulk.ColumnMappings.Add("Std_lname", "Std_lname");
            objbulk.ColumnMappings.Add("Std_pwd", "Std_pwd");
            objbulk.ColumnMappings.Add("Std_grade", "Std_grade");
            objbulk.ColumnMappings.Add("Std_fac", "Std_fac");
            objbulk.ColumnMappings.Add("Std_dep", "Std_dep");
            objbulk.ColumnMappings.Add("Std_fac_order", "Std_fac_order");
            objbulk.ColumnMappings.Add("Au_Status", "Au_Status");
            objbulk.ColumnMappings.Add("Std_hornor", "Std_hornor");
            con.Open();
            objbulk.WriteToServer(dt);
            con.Close();
        }
        // ===========================================================================

    }
}