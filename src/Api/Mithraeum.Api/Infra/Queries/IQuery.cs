using System.Collections.Generic;

namespace Mithraeum.Api.Infra.Queries
{
    public interface IQuery<out TResult>
    {
        IEnumerable<TResult> Execute();
    }
}