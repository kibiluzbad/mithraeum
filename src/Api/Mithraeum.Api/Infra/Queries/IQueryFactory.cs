namespace Mithraeum.Api.Infra.Queries
{
    public interface IQueryFactory
    {
        TType Get<TType>();
    }
}