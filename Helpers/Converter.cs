using System;
using System.Xml.Serialization;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Xml.Linq;

namespace Proxomo
{

    internal class Converter
    {

        public static string Convert<T>(T obj, CommunicationType format, bool indent = false, bool includetype = false)
        {
            if (format == CommunicationType.XML)
            {
                return ToXML<T>(obj, includetype, indent);
            }
            else if (format == CommunicationType.JSON)
            {
                return ToJSON<T>(obj);
            }
            else
            {
                return string.Empty;
            }

        }

        private static string ToXML<T>(T obj, bool includetype, bool indent = false)
        {
            if (includetype)
            {
                XElement xml = XMLConverter.ToXml(obj, null, includetype);
                
                if(indent) {
                    return xml.ToString(); 
                }
                else
                { 
                    return xml.ToString(SaveOptions.DisableFormatting);
                }
                   
            }
            else
            {
                System.Xml.Serialization.XmlSerializerNamespaces ns = new System.Xml.Serialization.XmlSerializerNamespaces();
                XmlSerializer xs = new XmlSerializer(typeof(T));
                StringBuilder sbuilder = new StringBuilder();
                var xmlws = new System.Xml.XmlWriterSettings() { OmitXmlDeclaration = true, Indent = indent };

                ns.Add(string.Empty, string.Empty);

                using (var writer = System.Xml.XmlWriter.Create(sbuilder, xmlws))
                {
                    xs.Serialize(writer, obj, ns);
                }

                string result = sbuilder.ToString();

                ns = null;
                xs = null;
                sbuilder = null;
                xmlws = null;

                return result;
            }
        }

        private static string ToJSON<T>(T obj)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                string result = string.Empty;
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                ser.WriteObject(ms, obj);
                result = encoding.GetString(ms.ToArray());

                ms.Close();
                encoding = null;
                ser = null;

                return result;
            }
        }

    }

}
