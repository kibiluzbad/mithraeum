
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
            IDictionary<string, Func<string, string>> keywords = new Dictionary<string, Func<string, string>>
                                                                     {
                                                                         {"Gender",term =>
                                                                                       {
                                                                                           var match = Regex.Match(
                                                                                               term, "Gender:([^:]+)");
                                                                                           
                                                                                           return !match.Success 
                                                                                               ? null 
                                                                                               : match.Groups[1].Value;
                                                                                       }},
                                                                         {"Director",term =>
                                                                                         {
                                                                                             var match = Regex.Match(
                                                                                               term, "Director:([^:]+)");
                                                                                           
                                                                                           return !match.Success 
                                                                                               ? null 
                                                                                               : match.Groups[1].Value;
                                                                                         }},
                                                                     };
        }

        
    }
}