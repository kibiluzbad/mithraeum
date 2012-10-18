using Mithraeum.Api.Infra.Queries;

namespace Mithraeum.Api.Model.Queries
{
    public interface IMoviesAdvancedSearch : IQuery<Movie>
    {
        string Term { get; set; }
    }
}