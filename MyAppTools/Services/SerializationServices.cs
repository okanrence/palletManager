using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MyAppTools.Services
{

    public class SerializationServices 
    {
        public static T DeserializeJson<T> (string JsonString)
        {
            return JsonConvert.DeserializeObject<T>(JsonString);
        }

        public static string SerializeJson<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static T DeserializeXml<T>(string xmlString)
        {
            try
            {
                var ser = new XmlSerializer(typeof(T));

                using (StringReader sr = new StringReader(xmlString))
                {
                    return (T)ser.Deserialize(sr);
                }
            }
            catch
            {
              throw;
            }

        }

        public static string SerializeXml<T>(T value)
        {
            if (value == null)
            {
                return null;
            }
            try
            {
                var serializer = new XmlSerializer(typeof(T));

                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                var settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string

                settings.OmitXmlDeclaration = true;

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, value, ns);
                    }
                    return textWriter.ToString();
                }
            }
            catch
            {
                return null;
            }


        }
    }
}