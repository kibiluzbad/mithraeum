using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Mithraeum.Api.Infra
{
    public static class HttpHelper
    {
        public static string Get(string url, IDictionary<string,object> parameters = null)
        {
            var sb = new StringBuilder();
            var buf = new byte[8192];

            var request = (HttpWebRequest)
                          WebRequest.Create(url);

            var response = (HttpWebResponse)
                           request.GetResponse();

            Stream resStream = response.GetResponseStream();

            int count = 0;

            do
            {
                if (resStream != null) count = resStream.Read(buf, 0, buf.Length);

                if (count == 0) continue;

                string tempString = Encoding.ASCII.GetString(buf, 0, count);
                
                sb.Append(tempString);
            }
            while (count > 0);

            return sb.ToString();
        }
    }
}