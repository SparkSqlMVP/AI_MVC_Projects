using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataPlatformManagement.Controllers
{
    public class GasMeterController : Controller
    {
        // GET: GasMeter
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            DataSet mydata = GetmultipleData();
            try
            {
                var DeviceInfoList = mydata.Tables[0].AsEnumerable().Select(dataRow => new DeviceInfo
                {
                    Id = dataRow.Field<int>("Id").ToString(),
                    ImagePath = dataRow.Field<string>("ImagePath"),// + ,
                    OCRText = dataRow.Field<string>("OCRText"),
                    OCRValue = dataRow.Field<string>("OCRValue"),
                    MeterValue = dataRow.Field<string>("MeterValue"),
                    DeviceSN = dataRow.Field<string>("DeviceSN"),
                    ctime = dataRow.Field<DateTime>("ctime").ToString("yyyy-MM-dd HH:mm:ss"),
                }).ToList();

                return Json(DeviceInfoList.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                // log v2
                throw;
            }
        }




        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            DataSet ds = GetSigleData(id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DeviceInfo di = new DeviceInfo();
                di.Id = ds.Tables[0].Rows[0]["Id"].ToString();
                di.ImagePath = ds.Tables[0].Rows[0]["ImagePath"].ToString();
                di.OCRText = ds.Tables[0].Rows[0]["OCRText"].ToString();
                di.OCRValue = ds.Tables[0].Rows[0]["OCRValue"].ToString();
                di.MeterValue = ds.Tables[0].Rows[0]["MeterValue"].ToString();
                di.DeviceSN = ds.Tables[0].Rows[0]["DeviceSN"].ToString();
                di.ctime = ds.Tables[0].Rows[0]["ctime"].ToString();
                return View(di);
            }
            else
            {
                return View(new DeviceInfo());
            }
        }
        [HttpPost]
        public ActionResult AddOrEdit(DeviceInfo dev)
        {

            //SAVE
            if (UpdateSigleData(dev) > 0)
            {
                return Json(new { success = true, message = "Edit Successfully" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, message = "Edit Fail" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (DelSigleData(id) > 0)
            {
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, message = "Deleted Fail" }, JsonRequestBehavior.AllowGet);
            }
           
        }


        [HttpGet]
        public ActionResult ImageView(int id = 0)
        {
            DataSet ds = GetSigleData(id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DeviceInfo di = new DeviceInfo();
                di.Id = ds.Tables[0].Rows[0]["Id"].ToString();
                di.ImagePath = ds.Tables[0].Rows[0]["ImagePath"].ToString();
                di.OCRText = ds.Tables[0].Rows[0]["OCRText"].ToString();
                di.OCRValue = ds.Tables[0].Rows[0]["OCRValue"].ToString();
                di.MeterValue = ds.Tables[0].Rows[0]["MeterValue"].ToString();
                di.DeviceSN = ds.Tables[0].Rows[0]["DeviceSN"].ToString();
                di.ctime = ds.Tables[0].Rows[0]["ctime"].ToString();
                return View(di);
            }
            else
            {
                return View(new DeviceInfo());
            }
        }


        public DataSet GetSigleData(int id)
        {
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["mysqlcon"].ToString();
            MySqlConnection conn = new MySqlConnection(connStr);

            try
            {
                conn.Open();
                DataSet DeviceInfoDS = MySqlHelper.ExecuteDataset(conn, string.Format(" select * from gasdevice_model750_rundata  where id={0}  ", id));
                conn.Close();

                return DeviceInfoDS;

            }
            catch (Exception ex)
            {
                conn.Close();
                // log v2
                throw;
            }
        }
        public int DelSigleData(int id)
        {
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["mysqlcon"].ToString();
            MySqlConnection conn = new MySqlConnection(connStr);

            try
            {
                conn.Open();
                int rows = MySqlHelper.ExecuteNonQuery(conn, string.Format(" delete from  gasdevice_model750_rundata  where id={0}  ", id));
                conn.Close();
                return rows;

            }
            catch (Exception ex)
            {
                conn.Close();
                // log v2
                throw;
            }
        }

        public DataSet GetmultipleData()
        {
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["mysqlcon"].ToString();
            MySqlConnection conn = new MySqlConnection(connStr);

            try
            {
                conn.Open();
                DataSet DeviceInfoDS = MySqlHelper.ExecuteDataset(conn, string.Format(" select * from gasdevice_model750_rundata "));
                conn.Close();
                return DeviceInfoDS;

            }
            catch (Exception ex)
            {
                conn.Close();
                // log v2
                throw;
            }
        }

        public int UpdateSigleData(DeviceInfo dev)
        {
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["mysqlcon"].ToString();
            MySqlConnection conn = new MySqlConnection(connStr);

            try
            {
                conn.Open();
                string sql = string.Format(" update gasdevice_model750_rundata set MeterValue='{0}'  where id={1}  ",
                                    dev.MeterValue, dev.Id);
                int rows = MySqlHelper.ExecuteNonQuery(conn, sql);
                conn.Close();
                return rows;
            }
            catch (Exception ex)
            {
                conn.Close();
                // log v2
                throw;
            }
        }

        public class DeviceInfo
        {
            public string Id { set; get; }
            public string ImagePath { set; get; }
            public string OCRText { set; get; }
            public string OCRValue { set; get; }
            public string MeterValue { set; get; }
            [Required(ErrorMessage = "This Field is Required")]
            public string DeviceSN { set; get; }
            public string ctime { set; get; }
        }

    }
}