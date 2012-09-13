using System.Collections.Generic;

namespace Mithraeum.Api.Model
{
    public interface IMoviesFinder
    {
        IEnumerable<FinderOption> FindByName(string name);
        Movie FindByImdbId(FinderOption imdbid);
    }
}