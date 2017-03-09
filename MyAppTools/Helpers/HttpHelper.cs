using MyAppTools.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyAppTools.Helpers
{
    public interface IHttpHelper
    {
        HttpResponseMessage Get(string requestUrl, Dictionary<string, string> headers = null);
        string GetString(string requestUrl, Dictionary<string, string> headers = null);
        string ReadString(HttpResponseMessage reponseObj);
        HttpResponseMessage Post<T>(T requestObj, string Url, Dictionary<string, string> headers = null) where T : class;
        T Get<T>(string requestUrl, Dictionary<string, string> headers = null) where T : class;

    }
    public class HttpHelper : IHttpHelper
    {

        public HttpResponseMessage Get(string requestUrl, Dictionary<string, string> headers = null)

        {
            var strResponse = string.Empty;
            HttpResponseMessage response = null;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                response = client.GetAsync(requestUrl).Result;
            }
            return response;
        }

        public T Get<T>(string requestUrl, Dictionary<string, string> headers = null)
         where T : class
        {
            T f;
            var strResponse = string.Empty;
            HttpResponseMessage response = null;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                response = client.GetAsync(requestUrl).Result;
                strResponse = response.Content.ReadAsStringAsync().Result;
                f = SerializationServices.DeserializeJson<T>(strResponse);
            }

            return f;
        }
        //public string GetString(string requestUrl)

        //{
        //    var strResponse = string.Empty;
        //    HttpResponseMessage response = null;
        //    using (var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        response = client.GetAsync(requestUrl).Result;
        //        strResponse = response.Content.ReadAsStringAsync().Result;
        //    }

        //    return strResponse;
        //}
        public string GetString(string requestUrl, Dictionary<string, string> headers = null)

        {
            var strResponse = string.Empty;
            HttpResponseMessage response = null;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }


                response = client.GetAsync(requestUrl).Result;
                strResponse = response.Content.ReadAsStringAsync().Result;
            }

            return strResponse;
        }
        public string ReadString(HttpResponseMessage reponseObj)

        {
            return reponseObj.Content.ReadAsStringAsync().Result;
        }

        public HttpResponseMessage Post<T>(T requestObj, string Url, Dictionary<string, string> headers = null)
           where T : class
        {
            HttpResponseMessage response = null;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }


                //var d = SerializationServices.SerializeJson(requestObj);
                response = client.PostAsJsonAsync(Url, requestObj).Result;


            }
            return response;
        }

        //public void Dispose()
        //{
        //    this.Dispose();
        //}
    }
}
