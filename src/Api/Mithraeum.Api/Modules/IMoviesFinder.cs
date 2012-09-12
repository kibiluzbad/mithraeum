using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Mithraeum.Api.Model;
using Nancy.Json;

namespace Mithraeum.Api.Modules
{
    public interface IMoviesFinder
    {
        IEnumerable<FinderOption> FindByName(string name);
        Movie FindByImdbId(FinderOption imdbid);
    }

    public class Thoth : IMoviesFinder
    {
        private const string Root = "http://thothapp.heroku.com/";
        
        public IEnumerable<FinderOption> FindByName(string name)
        {
            string json = HttpHelper.Get(string.Format("{0}/search/{1}", Root, name));
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<IEnumerable<FinderOption>>(json);
        }

        public Movie FindByImdbId(FinderOption imdbid)
        {
            throw new System.NotImplementedException();
        }
    }

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