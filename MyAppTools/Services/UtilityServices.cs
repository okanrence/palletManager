using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;

namespace MyAppTools.Services
{

    public class UtilityServices
    {
        public  static string arrayToString(string[] array)
        {
            var value = string.Empty;

            foreach (var item in array)
            {
                value += $"'{item}',";
            }

            
            value = value.Remove(value.Length - 2, 2);
            value = value.Remove(0, 1);

            return value;
        }

        public static string GetObjectString(object foo)
        {
            var result = string.Empty;
            foreach (var prop in foo.GetType().GetProperties())
            {
                result = result + $"{prop.Name}({prop.GetValue(foo, null)})";

            }
            return result;
        }

        public enum HASH_TYPES
        {
            SHA1 = 1,
            SHA256 = 2,
            SHA512 = 3,
            SHA384 = 4
        }
        public enum HASH_ENCODING
        {
            HEX = 1,
            Base64 = 2,
        }

        public static string CreateHash(string input, HASH_TYPES hashType, HASH_ENCODING hashEncoding)
        {

            HashAlgorithm hashAlgorithm = null;

            if (hashType == HASH_TYPES.SHA256)
                hashAlgorithm = (HashAlgorithm)new SHA256Managed();

            else if (hashType == HASH_TYPES.SHA512)
                hashAlgorithm = (HashAlgorithm)new SHA512Managed();

            else if (hashType == HASH_TYPES.SHA1)
                hashAlgorithm = (HashAlgorithm)new SHA1Managed();

            else if (hashType == HASH_TYPES.SHA384)
                hashAlgorithm = (HashAlgorithm)new SHA384Managed();


            byte[] bytes = Encoding.UTF8.GetBytes(input.Trim());

            var hashBytes = hashAlgorithm.ComputeHash(bytes);

            var hashValue = string.Empty;

            if (hashEncoding == HASH_ENCODING.Base64)
            {
                hashValue = Convert.ToBase64String(hashBytes);

            }
            else if (hashEncoding == HASH_ENCODING.HEX)
            {
                string hex = BitConverter.ToString(hashBytes);
                hashValue = hex.Replace("-", "");
            }
            else
            {
                hashValue = Encoding.UTF8.GetString(hashBytes);
            }
            //if (hashValue.EndsWith("=="))
            //    hashValue = hashValue.Remove(hashValue.Length - 2, 2);
            return hashValue;
        }


        public static string GetClientIp(HttpRequestMessage request = null)
        {
            string ClientIPAddress = string.Empty;

            // request = request ?? Request;
            if (request != null)
            {
                if (request.Properties.ContainsKey("MS_HttpContext"))
                {
                    ClientIPAddress = ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
                }
                else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
                {
                    RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                    ClientIPAddress = prop.Address;
                }
            }

            else if (System.Web.HttpContext.Current != null)
            {
                ClientIPAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
                if (string.IsNullOrEmpty(ClientIPAddress))
                {
                    ClientIPAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                }
                if (string.IsNullOrEmpty(ClientIPAddress))
                {
                    ClientIPAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
            }
            else
            {
                var sb = new StringBuilder();
                // Get the hostname
                string myHost = System.Net.Dns.GetHostName();
                var myIPs = Dns.GetHostEntry(myHost);
                foreach (var myIP in myIPs.AddressList)
                    if (!myIP.IsIPv6LinkLocal)
                        ClientIPAddress = myIP.ToString();

            }
            return ClientIPAddress;
        }

        public static string GetHeaderValue(HttpRequestMessage request, string headerKey)
        {
            try
            {
                IEnumerable<string> headers = request.Headers.GetValues(headerKey);
                var headerValue = headers.FirstOrDefault();
                return headerValue;
            }
            catch
            {
                return string.Empty;
            }
          
        }
    }
}
