using System.Linq;
using Mithraeum.Api.Model;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Mithraeum.Api.Infra.Indexes
{
    public class Movie_AdvancedSearch : AbstractIndexCreationTask<Movie, Movie_AdvancedSearch.MovieSearchResult>
    {
        public class MovieSearchResult : Movie
        {
            public virtual string Query { get; set; }
        }

        public Movie_AdvancedSearch()
        {
            Map = movies => from movie in movies
                            select new
                                       {
                                           movie.Imdbid,
                                           movie.Genres,
                                           movie.Picture_Path,
                                           movie.Plot,
                                           movie.Rating,
                                           movie.Title,
                                           movie.Year,
                                           Query = new object[]
                                                       {
                                                           movie.Title,
                                                           movie.Year
                                                       }
                                .Concat(movie.Genres)
                                       };

            Indexes.Add(c => c.Query, FieldIndexing.Analyzed);
        }
    }
}