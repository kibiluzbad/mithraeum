using System.Reflection;
using Autofac;
using Mithraeum.Api.Model;
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

            builder.RegisterType<Thoth>().As<IMoviesFinder>();
        }
    }
}