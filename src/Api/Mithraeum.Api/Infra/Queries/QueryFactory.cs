using Autofac;
using Microsoft.Practices.ServiceLocation;

namespace Mithraeum.Api.Infra.Queries
{
    public class QueryFactory : IQueryFactory
    {
        private readonly IServiceLocator _serviceLocator;
        
        public QueryFactory(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public TType Get<TType>()
        {
            return _serviceLocator.GetInstance<TType>();
        }
    }
}