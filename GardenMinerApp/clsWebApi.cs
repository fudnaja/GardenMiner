using BusCom;
using System;
using System.Collections.Generic;
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
    }
}
