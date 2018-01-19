using BusCom;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GardenMinerApp
{
   public class clsWebApi
    {
        public static string WebApiHelper(string url, string json, string app_id, string app_key)
        {
            LogFile vLog = new LogFile();
            vLog.WriteLogEvent("Web Api URL:" + url);
            try
            {
                HttpWebRequest request;
                HttpWebResponse response;
                request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = true;
                request.ContentType = "application/json; charset=UTF-8";
                request.Method = "POST";
                request.Headers.Add("app_id", app_id);
                request.Headers.Add("app_key", app_key);
                request.KeepAlive = true;
                byte[] postBytes = UTF8Encoding.UTF8.GetBytes(json);
                request.ContentLength = postBytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(postBytes, 0, postBytes.Length);
                requestStream.Close();
                response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string result = reader.ReadToEnd().ToString();
                response.Close();
                vLog.WriteLogEvent("Web Api return:" + result);
                return result;
            }
            catch (Exception ex)
            {
                vLog.WriteLogError("clsWebApi.WebApiHelper(" + url + "," + json + ") -> Exception| " + ex.Message);
                return string.Empty;
            }
        }

        public static string GetCoinsWhattomine()
        {
            string url = "https://whattomine.com/coins.json";
            LogFile vLog = new LogFile();
            vLog.WriteLogEvent("Web Api URL:" + url);
            try
            {
                HttpWebRequest request;
                HttpWebResponse response;
                request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = true;
                request.ContentType = "application/json; charset=UTF-8";
                request.Method = "GET";
                request.KeepAlive = true;
                response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string result = reader.ReadToEnd().ToString();
                response.Close();
                vLog.WriteLogEvent("Web Api return:" + result);
                return result;
            }
            catch (Exception ex)
            {
                vLog.WriteLogError("clsWebApi.WebApiHelper() -> Exception| " + ex.Message);
                return string.Empty;
            }
        }

        public static string GetCoinsNews()
        {
            string url = "https://whattomine.com/calculators.json";
            LogFile vLog = new LogFile();
            vLog.WriteLogEvent("Web Api URL:" + url);
            try
            {
                HttpWebRequest request;
                HttpWebResponse response;
                request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = true;
                request.ContentType = "application/json; charset=UTF-8";
                request.Method = "GET";
                request.KeepAlive = true;
                response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string result = reader.ReadToEnd().ToString();
                response.Close();
                vLog.WriteLogEvent("Web Api return:" + result);
                return result;
            }
            catch (Exception ex)
            {
                vLog.WriteLogError("clsWebApi.WebApiHelper() -> Exception| " + ex.Message);
                return string.Empty;
            }
        }

        public static string GetCoinsStatus(string url)
        {
            LogFile vLog = new LogFile();
            vLog.WriteLogEvent("Web Api URL:" + url);
            try
            {
                HttpWebRequest request;
                HttpWebResponse response;
                request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = true;
                request.ContentType = "application/json; charset=UTF-8";
                request.Method = "GET";
                request.KeepAlive = true;
                response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string result = reader.ReadToEnd().ToString();
                response.Close();
                vLog.WriteLogEvent("Web Api return:" + result);
                return result;
            }
            catch (Exception ex)
            {
                vLog.WriteLogError("clsWebApi.WebApiHelper() -> Exception| " + ex.Message);
                return string.Empty;
            }
        }

        public static double GetCoinsExplorer(string url)
        {
            LogFile vLog = new LogFile();
            vLog.WriteLogEvent("Web Api URL:" + url);
            try
            {
                double amount = 0;
                HttpWebRequest request;
                HttpWebResponse response;
                request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = true;
                request.ContentType = "application/json; charset=UTF-8";
                request.Method = "GET";
                request.KeepAlive = true;
                response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string result = reader.ReadToEnd().ToString();
                if(result!=string.Empty)
                {
                    amount = double.Parse(result);
                }
                response.Close();
                vLog.WriteLogEvent("Web Api return:" + result);
                return amount;
            }
            catch (Exception ex)
            {
                vLog.WriteLogError("clsWebApi.WebApiHelper() -> Exception| " + ex.Message);
                return 0;
            }
        }

        public static double GetCoinsBTC()
        {
            Converter vCon = new Converter();
            double BTC_Price = 0;
            string url = "https://bx.in.th/api/";
            LogFile vLog = new LogFile();
            vLog.WriteLogEvent("Web Api URL:" + url);
            try
            {
                HttpWebRequest request;
                HttpWebResponse response;
                request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = true;
                request.ContentType = "application/json; charset=UTF-8";
                request.Method = "GET";
                request.KeepAlive = true;
                response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string result = reader.ReadToEnd().ToString();
                if(result!=string.Empty)
                {
                    result = "{\"res\":" + result + "}";
                    DataSet ds = vCon.JsonToDataset(result);
                    for(int i=0;i<ds.Tables.Count;i++)
                    {
                        string table_name = ds.Tables[i].TableName;
                        string primary_currency = ds.Tables[i].Rows[0]["primary_currency"].ToString();
                        string secondary_currency = ds.Tables[i].Rows[0]["secondary_currency"].ToString();
                        double last_price = double.Parse(ds.Tables[i].Rows[0]["last_price"].ToString());
                        if (primary_currency =="THB" && secondary_currency=="BTC")
                        {
                            BTC_Price = last_price;
                            i = ds.Tables.Count;
                        }
                    }
                }
                response.Close();
                vLog.WriteLogEvent("Web Api return:" + result);
                return BTC_Price;
            }
            catch (Exception ex)
            {
                vLog.WriteLogError("clsWebApi.WebApiHelper() -> Exception| " + ex.Message);
                return 0;
            }
        }

        public static double GetCoinsRate(string Coin)
        {
            Converter vCon = new Converter();
            double Price = 0;
            string url = "https://min-api.cryptocompare.com/data/price?fsym=" + Coin + "&tsyms=BTC,USD,THB";
            LogFile vLog = new LogFile();
            vLog.WriteLogEvent("Web Api URL:" + url);
            try
            {
                HttpWebRequest request;
                HttpWebResponse response;
                request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = true;
                request.ContentType = "application/json; charset=UTF-8";
                request.Method = "GET";
                request.KeepAlive = true;
                response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string result = reader.ReadToEnd().ToString();
                if (result != string.Empty)
                {
                    result = "{\"res\":{\"rate\":" + result + "}}";
                    DataSet ds = vCon.JsonToDataset(result);
                    if(ds.Tables.Contains("rate"))
                    {
                        Price = double.Parse(ds.Tables["rate"].Rows[0]["BTC"].ToString());
                    }
                }
                response.Close();
                vLog.WriteLogEvent("Web Api return:" + result);
                return Price;
            }
            catch (Exception ex)
            {
                vLog.WriteLogError("clsWebApi.WebApiHelper() -> Exception| " + ex.Message);
                return 0;
            }
        }
    }
}
