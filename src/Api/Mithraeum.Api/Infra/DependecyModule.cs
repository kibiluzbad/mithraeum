using System.Reflection;
using Autofac;
using Microsoft.Practices.ServiceLocation;
using Mithraeum.Api.Infra.Queries;
using Mithraeum.Api.Model;
using Mithraeum.Api.Model.Queries;
using Mithraeum.Api.Modules;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Module = Autofac.Module;

namespace Mithraeum.Api.Infra
{
    public class DependecyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ =>
                                 {
                                     var store = new DocumentStore
                                         {
                                             ConnectionStringName = "RavenDB"
                                         }.Initialize();

                                     IndexCreation.CreateIndexes(Assembly.GetExecutingAssembly(), store);

                                     return store;
                                 })
                .As<IDocumentStore>()
                .SingleInstance();

            builder.Register(_ => _.Resolve<IDocumentStore>().OpenSession())
                .As<IDocumentSession>()
                .InstancePerLifetimeScope();

            builder.RegisterType<Thoth>()
                .As<IMoviesFinder>();

            builder.RegisterType<MoviesAdvancedSearch>()
                .As<IMoviesAdvancedSearch>();

            builder.Register(c=> new QueryFactory(c.Resolve<IServiceLocator>()))
                .As<IQueryFactory>()
                .SingleInstance();

            builder.Register(c => ServiceLocator.Current)
                .As<IServiceLocator>()
                .SingleInstance();

        }
    }
}