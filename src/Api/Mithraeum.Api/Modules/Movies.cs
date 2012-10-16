using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mithraeum.Api.Infra;
using Mithraeum.Api.Infra.Indexes;
using Mithraeum.Api.Model;
using Nancy;
using Raven.Client;
using Raven.Client.Linq;

namespace Mithraeum.Api.Modules
{
    public class Movies
        : NancyModule
    {
        private readonly IDocumentSession _session;

        public Movies(IDocumentSession session,
                      IMoviesFinder finder)
            : base("/api/movies")
        {
            _session = session;
            
            After += _ => _session.SaveChanges();

            Get["/"] = _ =>
                           {
                               var term = (string) Request.Query.term;

                               var query = string.IsNullOrWhiteSpace(term)
                                               ? _session.Query<Movie_AdvancedSearch.MovieSearchResult, Movie_AdvancedSearch>()
                                               : _session.Query<Movie_AdvancedSearch.MovieSearchResult, Movie_AdvancedSearch>().Search(c => c.Query,
                                                                               string.Format("*{0}*", term),
                                                                               options: SearchOptions.Or,
                                                                               escapeQueryOptions:
                                                                                   EscapeQueryOptions.AllowAllWildcards);

                               var movies = query
                                   .As<Movie>()
                                   .OrderByDescending(c => c.Rating)
                                   .ThenBy(c => c.Title)
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

                                    _session.Store(movie, movie.Imdbid);

                                    return Response.AsJson(movie);
                                }

                                var ambigousResult = new Ambiguous
                                                         {
                                                             Title = title,
                                                             Options = list,
                                                             Slug = title.Slugify()
                                                         };

                                _session.Store(ambigousResult, ambigousResult.Slug);

                                return Response
                                    .AsJson(ambigousResult);
                            };

            Post["/(?<imdbid>tt[\\d]+)"] = _ =>
                                               {

                                                   var imdbid = (string) _.imdbid;

                                                   var movie = finder.FindByImdbId(new FinderOption() {Imdbid = imdbid});

                                                   _session.Store(movie, movie.Imdbid);

                                                   return Response.AsJson(movie);

                                               };

            Delete["/(?<imdbid>tt[\\d]+)"] = _ =>
                                                 {

                                                     var imdbid = (string) _.imdbid;

                                                     var movie = _session.Load<Movie>(imdbid);

                                                     _session.Delete(movie);

                                                     return Response.AsJson(movie);

                                                 };

            Get["/UpdateAll"] = _ =>
                                    {
                                        foreach (var m in _session.Query<Movie>().ToList())
                                        {
                                            var imdbid = m.Imdbid;
                                            var movie = finder.FindByImdbId(new FinderOption() { Imdbid = imdbid });

                                            _session.Delete(m);
                                            _session.SaveChanges();
                                            _session.Store(movie,imdbid);
                                        }

                                        return HttpStatusCode.OK;
                                    };
        }
    }
}