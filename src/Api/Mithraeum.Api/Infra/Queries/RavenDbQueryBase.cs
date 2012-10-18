using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using Raven.Client.Linq;

namespace Mithraeum.Api.Infra.Queries
{
    public abstract class RavenDbQueryBase<TResult> : IQuery<TResult>
    {
        protected readonly IDocumentSession Session;

        protected RavenDbQueryBase(IDocumentSession session)
        {
            Session = session;
        }

        public virtual IEnumerable<TResult> Execute()
        {
            IQueryable<TResult> query = CreateQuery();
            return query
                .ToList();
        }

        protected abstract IQueryable<TResult> CreateQuery();
    }
}