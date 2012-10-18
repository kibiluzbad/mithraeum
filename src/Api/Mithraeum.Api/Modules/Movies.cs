using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mithraeum.Api.Infra;
using Mithraeum.Api.Infra.Indexes;
using Mithraeum.Api.Infra.Queries;
using Mithraeum.Api.Model;
using Mithraeum.Api.Model.Queries;
using Nancy;
using Raven.Client;
using Raven.Client.Linq;

namespace Mithraeum.Api.Modules
{
    public class Movies
        : NancyModule
    {
        private readonly IDocumentSession _session;
        private readonly IMoviesFinder _finder;
        private readonly IQueryFactory _queryFactory;

        public Movies(IDocumentSession session,
                      IMoviesFinder finder,
                      IQueryFactory queryFactory)
            : base("/api/movies")
        {
            _session = session;
            _finder = finder;
            _queryFactory = queryFactory;

            After += _ => _session.SaveChanges();

            Get["/"] = _ =>
                           {
                               var term = (string) Request.Query.term;

                               var query = _queryFactory.Get<IMoviesAdvancedSearch>();
                               query.Term = term;

                               var movies = query.Execute();

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

                                                   var movie = _finder.FindByImdbId(new FinderOption() {Imdbid = imdbid});

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
                                            var movie = _finder.FindByImdbId(new FinderOption() { Imdbid = imdbid });

                                            _session.Delete(m);
                                            _session.SaveChanges();
                                            _session.Store(movie,imdbid);
                                        }

                                        return HttpStatusCode.OK;
                                    };
        }
    }
}