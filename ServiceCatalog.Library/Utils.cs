using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;

using ServiceCatalog.Models;
using ServiceCatalog.Library;


namespace ServiceCatalog.Library
{
    public class Utils
    {
        public static int CInt(object value)
        {
            if (value == null)
                return (0);

            var data = value.ToString();
            data = data.Replace(",", "");
            var isNum = new System.Text.RegularExpressions.Regex("^([-]|[0-9])[0-9]*$");
            var m = isNum.Match(data);
            return m.Success ? (Convert.ToInt32(data)) : (0);
        }

        public static long CLong(object value)
        {
            if (value == null)
                return (0);

            var data = value.ToString();
            data = data.Replace(",", "");
            var isNum = new System.Text.RegularExpressions.Regex("^([-]|[0-9])[0-9]*$");
            var m = isNum.Match(data);
            return m.Success ? (Convert.ToInt64(data)) : (0);
        }

        public static double CDouble(object value)
        {
            var data = value.ToString();
            data = data.Replace(",", "");
            var isNum = new System.Text.RegularExpressions.Regex("^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$|^([-]|[0-9])[0-9]*$");
            var m = isNum.Match(data);
            return m.Success ? (Convert.ToDouble(data)) : (0.00);
        }

        public static decimal CDecimal(object value)
        {
            var data = value.ToString();
            data = data.ToString(CultureInfo.InvariantCulture).Replace(",", "");
            var isNum = new System.Text.RegularExpressions.Regex("^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$|^([-]|[0-9])[0-9]*$");
            var m = isNum.Match(data);
            return m.Success ? (Convert.ToDecimal(data)) : (Convert.ToDecimal("0.00"));
        }

        public static string GetPathOf(string fullpath)
        {
            var lastIdx = fullpath.LastIndexOf("\\", StringComparison.Ordinal);
            return (fullpath.Substring(0, lastIdx));
        }

        public static void ByteToFile(byte[] buffer, string targetFile)
        {
            try
            {
                var stf = new MemoryStream(buffer);
                var stt = File.Create(targetFile);
                var br = new BinaryReader(stf);
                var bw = new BinaryWriter(stt);
                bw.Write(br.ReadBytes((int)stf.Length));
                bw.Flush();
                bw.Close();
                br.Close();
                stf = null;
                stt = null;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static byte[] FileToByte(string sourceFile)
        {
            try
            {
                var b = File.ReadAllBytes(sourceFile);
                return (b);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static string GetConfig(string key)
        {
            /*
            if (key.Equals("CN"))
                return (EncryptData.Decrypt(ConfigurationSettings.AppSettings[key]));
            else
            */

            return (ConfigurationSettings.AppSettings[key]);
        }

        public static string Left(string str, int length)
        {
            return str.Substring(0, Math.Min(length, str.Length));
        }

        public static string Right(string original, int numberCharacters)
        {
            return original.Substring(numberCharacters > original.Length ? 0 : original.Length - numberCharacters);
        }

        public static string ConvertDatetime(string date)
        {
            var stratDate = date.Split('/');
            string newDate = new DateTime(Convert.ToInt32(stratDate[2]) - 543, Convert.ToInt32(stratDate[1]), Convert.ToInt32(stratDate[0])).ToString("yyyy-MM-dd", new CultureInfo("en-US"));

            return newDate;

        }

        public static string ConvertDatetimeToString(DateTime datetime)
        {
            return datetime.Date.ToString("dd/MM/yyyy", new CultureInfo("th-TH"));
        }

        public static DateTime ConvertStringToDatetime(string datetime)
        {
            var stratDate = datetime.Split('/');

            return new DateTime(Convert.ToInt32(stratDate[2]) - 543, Convert.ToInt32(stratDate[1]), Convert.ToInt32(stratDate[0]));

            //var stratDate = datetime.Split('-');
            //string newDate = new DateTime(Convert.ToInt32(stratDate[0]) + 543, Convert.ToInt32(stratDate[1]), Convert.ToInt32(stratDate[2])).ToString("dd/MM/yyyy", new CultureInfo("en-TH"));

            //return newDate;
        }

        public static DateTime ConvertStringToDatetimeEn(string datetime)
        {
            var stratDate = datetime.Split('-');

            return new DateTime(Convert.ToInt32(stratDate[0]), Convert.ToInt32(stratDate[1]), Convert.ToInt32(stratDate[2]));

            //var stratDate = datetime.Split('-');
            //string newDate = new DateTime(Convert.ToInt32(stratDate[0]) + 543, Convert.ToInt32(stratDate[1]), Convert.ToInt32(stratDate[2])).ToString("dd/MM/yyyy", new CultureInfo("en-TH"));

            //return newDate;
        }

        public static DateTime ConvertStringToDatetimeEnSlash(DateTime datetime)
        {
            var stratDate = datetime.ToString().Split('/');

            return new DateTime(Convert.ToInt32(stratDate[0]), Convert.ToInt32(stratDate[1]), Convert.ToInt32(stratDate[2]));

        }
        public static string ConvertTimeToString(DateTime datetime)
        {
            return datetime.ToString("H:mm", new CultureInfo("th-TH"));
        }

        public static DateTime ConvertStringToDatetimeEmail(string datetime)
        {
            var stratDate = datetime.Split('/');
            var strYear = stratDate[2].ToString().Split(' ');
            var strTime = strYear[1].ToString().Split(':');

            DateTime dateTime = new DateTime(Convert.ToInt32(strYear[0]) - 543, Convert.ToInt32(stratDate[1]), Convert.ToInt32(stratDate[0]), Convert.ToInt32(strTime[0]) + 7, Convert.ToInt32(strTime[1]), Convert.ToInt32(strTime[2]));
            return dateTime;
        }


        //Implemented based on interface, not part of algorithm
        public static string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }

        //Core recursion function
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }


        //get name month
        public Array ListMonth(string language = "EN", string format = "S")
        {
            var arrayMonth = new string[12];
            if (language == "EN")
            {
                if (format == "S")
                {
                    arrayMonth = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                }
            }
            return (arrayMonth);
        }
        public List<DateTime> getAllDates(int year, int month)
        {
            var ret = new List<DateTime>();
            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                ret.Add(new DateTime(year, month, i));
            }
            return ret;
        }
        //public JsonResult GetBrandMaster()
        //{
        //    List<SelectListItem> listBrandMaster = new List<SelectListItem>();
        //    using (SqlConnection Connection = new SqlConnection(GetConfig("ServiceCatalogDB")))
        //    {
        //        Connection.Open();
        //        var command = new SqlCommand("P_Search_Brand", Connection);
        //        command.CommandType = CommandType.StoredProcedure;
        //        SqlDataReader dr2 = command.ExecuteReader();
        //        while (dr2.Read())
        //        {
        //            listBrandMaster.Add(new SelectListItem() { Value = dr2["BrandId"].ToString(), Text = dr2["BrandId"].ToString() + "/" + dr2["BrandName"].ToString() });
        //        }
        //    }
        //    return Json(listBrandMaster, JsonRequestBehavior.AllowGet);
        //}
        public static string savePathFile(string DocumentNo)
        {
            string getDocumentNo = "";

            using (SqlConnection Connection = new SqlConnection(GetConfig("HRIS_DB")))
            {
                Connection.Open();
                var cmdSearch = new SqlCommand("P_Save_PathFile", Connection);

                cmdSearch.CommandType = CommandType.StoredProcedure;
                cmdSearch.Parameters.AddWithValue("@inReqNo", DocumentNo);
                cmdSearch.Parameters.AddWithValue("@inFileName", DocumentNo);
                cmdSearch.Parameters.AddWithValue("@inFilePath", DocumentNo);
                cmdSearch.Parameters.AddWithValue("@inFileNo", DocumentNo);
                cmdSearch.Parameters.AddWithValue("@inUser", DocumentNo);
                SqlParameter returnResult = new SqlParameter("@outResult", SqlDbType.NVarChar, 1000);
                returnResult.Direction = ParameterDirection.Output;
                cmdSearch.Parameters.Add(returnResult);
                int INSID = cmdSearch.ExecuteNonQuery();
                getDocumentNo = cmdSearch.Parameters["@outResult"].Value.ToString();
                cmdSearch.Dispose();
                Connection.Close();
            }
            return getDocumentNo;
        }
        public static string DeleteProductApiToDb(string PartNo, int BrandId)
        {
            string getResult = "";

            using (SqlConnection Connection = new SqlConnection(GetConfig("ServiceCatalogDB")))
            {
                Connection.Open();
                var cmdSearch = new SqlCommand("P_Delete_Product_Api", Connection);

                cmdSearch.CommandType = CommandType.StoredProcedure;
                cmdSearch.Parameters.AddWithValue("@inStkcode", PartNo);
                cmdSearch.Parameters.AddWithValue("@inBrandId", BrandId);
                SqlParameter returnResult = new SqlParameter("@outResult", SqlDbType.NVarChar, 1000);
                returnResult.Direction = ParameterDirection.Output;
                cmdSearch.Parameters.Add(returnResult);
                int INSID = cmdSearch.ExecuteNonQuery();
                getResult = cmdSearch.Parameters["@outResult"].Value.ToString();
                cmdSearch.Dispose();
                Connection.Close();
            }
            return getResult;
        }  
    }
}
