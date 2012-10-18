
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Mithraeum.Api.Infra.Indexes;
using Mithraeum.Api.Infra.Queries;
using Raven.Client;
using Raven.Client.Linq;

namespace Mithraeum.Api.Model.Queries
{
    public class MoviesAdvancedSearch : RavenDbQueryBase<Movie>, IMoviesAdvancedSearch
    {
        protected KeywordExpression[] Expressions { get; set; }

        public string Term { get; set; }
        
        public MoviesAdvancedSearch(IDocumentSession session) 
            : base(session)
        {
            Expressions = new[]
                              {
                                  new KeywordExpression(new GetValueUsingRegex())
                              };
        }

        


        protected override IQueryable<Movie> CreateQuery()
        {
            var query =
                Session.Query<Movie_AdvancedSearch.MovieSearchResult, Movie_AdvancedSearch>();

            if (!string.IsNullOrWhiteSpace(Term))
                query = GetCriteria(query);
            ;

            return LinqExtensions.As<Movie>(query)
                .OrderByDescending(c => c.Rating)
                .ThenBy(c => c.Title);
        }

        protected virtual IRavenQueryable<Movie_AdvancedSearch.MovieSearchResult> GetCriteria(IRavenQueryable<Movie_AdvancedSearch.MovieSearchResult> query)
        {
            IDictionary<string, Func<string, Match>> keywords = new Dictionary<string, Func<string, Match>>
                                                                     {
                                                                         {"Gender",term => Regex.Match(term, "Gender:([^:]+)")},
                                                                         {"Director",term => Regex.Match(term, "Director:([^:]+)")},
                                                                     };

            foreach (var keyword in keywords)
            {
                var match = keyword.Value.Invoke(Term);
                
            }
            throw new NotImplementedException();
        }

        
    }
}