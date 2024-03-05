using System;
using System.Net;
using System.Text;
using System.Xml;

namespace OpenDARTCSharp
{
    public static partial class OpenDartApi
    {
        private const string ApiBaseUrl = "https://opendart.fss.or.kr/api/";

        private static Encoding Encoding = Encoding.UTF8;

        private static WebClient CreateWebClient()
        {
            WebClient client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.3; WOW64; Trident/7.0)");
            client.Encoding = OpenDartApi.Encoding;
            return client;
        }
        private static byte[] DownloadData(string url)
        {
            using (var client = CreateWebClient())
                return client.DownloadData(url);
        }
        private static string DownloadString(string url)
        {
            using (WebClient client = CreateWebClient())
                return client.DownloadString(url);
        }
        private static XmlDocument DownloadXml(string url)
        {
            string str = DownloadString(url);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(str);
            return xml;
        }

        private static XmlDocument ToXml(this byte[] bytes)
        {
            string str = OpenDartApi.Encoding.GetString(bytes);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(str);
            return doc;
        }

        private static void Validate(XmlNode resultNode, int passValue = 0)
        {
            if (resultNode == null)
                throw new ArgumentNullException(nameof(resultNode), "Node cannot be null.");

            var statusNode = resultNode.SelectSingleNode("status");
            var messageNode = resultNode.SelectSingleNode("message");
            if (statusNode == null || messageNode == null)
                throw new ArgumentException("Invalid XML format. Status and message nodes are required.");

            if (!int.TryParse(statusNode.InnerText, out int statusCode))
                throw new ArgumentException("Invalid status value in XML.");
            if (statusCode == passValue)
                return;
            string errorMessage = messageNode.InnerText;

            throw new DartException(statusCode, errorMessage);
        }

        internal static string ToString<T>(object obj)
        {
            Type t = typeof(T);

            StringBuilder sb = new StringBuilder();
            sb.Append(t.Name);
            sb.Append('(');

            foreach (var field in t.GetFields())
            {
                object[] attributes = field.GetCustomAttributes(typeof(FormatAttribute), false);
                if (attributes.Length > 0)
                    sb.AppendFormat(string.Concat("{0}:{1:", ((FormatAttribute)attributes[0]).format, "},"), field.Name, field.GetValue(obj));
                else
                    sb.AppendFormat("{0}:{1},", field.Name, field.GetValue(obj));
            }

            sb.Append(')');
            return sb.ToString();
        }
    }
}