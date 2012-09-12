using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mithraeum.Api.Model;
using Nancy;
using Raven.Client;

namespace Mithraeum.Api.Modules
{
    public class Movies
        : NancyModule
    {
        private readonly IDocumentSession _session;
        private readonly IMoviesFinder _finder;

        public Movies(IDocumentSession session,
                      IMoviesFinder finder)
            : base("/api/movies")
        {
            _session = session;
            _finder = finder;


            After += _ => _session.SaveChanges();

            Get["/"] = _ =>
                           {
                               var movies = _session.Load<Movie>();

                               return Response
                                   .AsJson(movies);
                           };

            Post["/"] = _ =>
                            {
                                var title = (string) Request.Form.name;

                                IEnumerable<FinderOption> list = _finder.FindByName(title);

                                if (1 == list.Count())
                                {
                                    Movie movie = _finder.FindByImdbId(list.FirstOrDefault());

                                    _session.Store(movie, movie.Imdbid);

                                    return Response.AsJson(movie);
                                }

                                var ambigousResult = new Ambiguous
                                                         {
                                                             Title = title,
                                                             Options = list
                                                         };

                                _session.Store(ambigousResult, ambigousResult.Title);

                                return Response
                                    .AsJson(ambigousResult);
                            };
        }
    }
}