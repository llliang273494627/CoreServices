﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BackServices.Common.Helper
{
    /// <summary>
    /// 基于 RestSharp 封装HttpHelper
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// Get 请求
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="baseUrl">根域名:http://apk.neters.club/</param>
        /// <param name="url">接口:api/xx/yy</param>
        /// <param name="pragm">参数:id=2&name=老张</param>
        /// <returns></returns>
        public async static Task<T> GetApi<T>(string url, string pragm = "")
        {
            try
            {
                var client = new RestClient(url);
                url = string.IsNullOrEmpty(pragm) ? string.Empty : $"{url}?{pragm}";
                var request = await client.ExecuteAsync(new RestRequest(url, Method.GET));
                if (request.StatusCode != HttpStatusCode.OK)
                {
                    return (T)Convert.ChangeType(request.ErrorMessage, typeof(T));
                }
                dynamic temp = Newtonsoft.Json.JsonConvert.DeserializeObject(request.Content, typeof(T));
                return (T)temp;
            }
            catch (Exception ex)
            {
                HelperLog.Error($"Get请求失败！url={url},pragm={pragm}", ex);
            }
            return default(T);
        }

        /// <summary>
        /// Post 请求
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="url">完整的url</param>
        /// <param name="body">post body,可以匿名或者反序列化</param>
        /// <returns></returns>
        public async static Task<T> PostApi<T>(string url, object body = null)
        {
            try
            {
                var client = new RestClient($"{url}");
                IRestRequest queest = new RestRequest
                {
                    Method = Method.POST,
                };
                queest.AddHeader("Accept", "application/json");
                queest.AddJsonBody(body);
                var result = await client.ExecuteAsync(queest);
                if (result.StatusCode != HttpStatusCode.OK)
                {
                    return (T)Convert.ChangeType(result.ErrorMessage, typeof(T));
                }
                dynamic temp = Newtonsoft.Json.JsonConvert.DeserializeObject(result.Content, typeof(T));
                return (T)temp;
            }
            catch (Exception ex)
            {
                HelperLog.Error($"POST请求失败！url={url},body={Newtonsoft.Json.JsonConvert.SerializeObject(body)}", ex);
            }
            return default(T);
        }

        /// <summary>
        /// .net 3.5 版本
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static string RequestPost(string url, object body)
        {
            string ponseStr = string.Empty;
            string bodyStr = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "POST";

                bodyStr = Newtonsoft.Json.JsonConvert.SerializeObject(body);
                byte[] bys = System.Text.ASCIIEncoding.ASCII.GetBytes(bodyStr);
                request.ContentLength = bys.Length;

                using (Stream requestSream = request.GetRequestStream())
                {
                    requestSream.Write(bys, 0, bys.Length);
                    requestSream.Flush();
                    requestSream.Close();
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (Stream receiveStream = response.GetResponseStream())
                {
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    ponseStr = readStream.ReadToEnd();
                    readStream.Close();
                    receiveStream.Close();
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error($"POST请求失败！url={url},body={bodyStr}", ex);
            }
            return ponseStr;
        }

        /// <summary>
        /// .net 3.5 版本
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string RequestGet(string url)
        {
            string ponseStr = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (Stream receiveStream = response.GetResponseStream())
                {
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    ponseStr = readStream.ReadToEnd();
                    readStream.Close();
                    receiveStream.Close();
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error($"POST请求失败！url={url}", ex);
            }
            return ponseStr;
        }
    }
}
