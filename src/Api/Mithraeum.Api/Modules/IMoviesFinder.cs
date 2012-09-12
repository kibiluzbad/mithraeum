using System.Collections.Generic;
using Mithraeum.Api.Model;

namespace Mithraeum.Api.Modules
{
    public interface IMoviesFinder
    {
        IEnumerable<FinderOption> FindByName(string name);
        Movie FindByImdbId(FinderOption imdbid);
    }
}