using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Mithraeum.Api.Model;
using Nancy.Json;

namespace Mithraeum.Api.Infra
{
    public class Thoth : IMoviesFinder
    {
        private const string Root = "http://localhost:4567";

        public IEnumerable<FinderOption> FindByName(string name)
        {
            string json = HttpHelper.Get(string.Format("{0}/search/{1}", Root, name));
            var serializer = new JavaScriptSerializer();
            var result = serializer.Deserialize<IEnumerable<FinderOption>>(json);
            return result
                .Where(c => null != c.Imdbid && Regex.IsMatch(c.Imdbid, "tt[\\d]+"))
                .Distinct(new UniqueImdbid())
                .ToList();
        }

        public Movie FindByImdbId(FinderOption option)
        {
            string json = HttpHelper.Get(string.Format("{0}/movie/{1}", Root, option.Imdbid));
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<Movie>(json);
        }
    }

    public class UniqueImdbid : IEqualityComparer<FinderOption>
    {
        public bool Equals(FinderOption x, FinderOption y)
        {
            return x.Imdbid.Equals(y.Imdbid);
        }

        public int GetHashCode(FinderOption obj)
        {
            return obj.Imdbid.GetHashCode() + 64987;
        }
    }
}