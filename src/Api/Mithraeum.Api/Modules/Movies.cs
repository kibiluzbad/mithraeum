using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mithraeum.Api.Infra;
using Mithraeum.Api.Model;
using Nancy;
using Raven.Client;

namespace Mithraeum.Api.Modules
{
    public class Movies
        : NancyModule
    {
        public Movies(IDocumentSession session,
                      IMoviesFinder finder)
            : base("/api/movies")
        {
            After += _ => session.SaveChanges();
            Get["/"] = _ =>
                           {
                               var movies = session
                                   .Query<Movie>()
                                   .OrderByDescending(c=>c.Rating)
                                   .ThenBy(c=>c.Title)
                                   .ToList();

                               return Response
                                   .AsJson(movies);
                           };

            Post["/"] = _ =>
                            {
                                var title = (string) Request.Form.name;

                                IEnumerable<FinderOption> list = finder.FindByName(title);

                                if (1 == list.Count())
                                {
                                    Movie movie = finder.FindByImdbId(list.FirstOrDefault());

                                    session.Store(movie, movie.Imdbid);

                                    return Response.AsJson(movie);
                                }

                                var ambigousResult = new Ambiguous
                                                         {
                                                             Title = title,
                                                             Options = list,
                                                             Slug = title.Slugify()
                                                         };

                                session.Store(ambigousResult, ambigousResult.Slug);

                                return Response
                                    .AsJson(ambigousResult);
                            };

            Post["/(?<imdbid>tt[\\d]+)"] = _ =>
                                               {
                                                   
                                                   var imdbid = (string) _.imdbid;

                                                   var movie = finder.FindByImdbId(new FinderOption() {Imdbid = imdbid});

                                                   session.Store(movie, movie.Imdbid);

                                                   return Response.AsJson(movie);

                                               };
        }
    }
}