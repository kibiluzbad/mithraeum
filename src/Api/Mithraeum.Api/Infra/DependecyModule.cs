using Autofac;
using Mithraeum.Api.Model;
using Mithraeum.Api.Modules;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;

namespace Mithraeum.Api.Infra
{
    public class DependecyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ => new DocumentStore
                                      {
                                          ConnectionStringName = "RavenDB"
                                      }.Initialize())
                .As<IDocumentStore>()
                .SingleInstance();

            builder.Register(_ => _.Resolve<IDocumentStore>().OpenSession())
                .As<IDocumentSession>()
                .InstancePerLifetimeScope();

            builder.RegisterType<Thoth>().As<IMoviesFinder>();
        }
    }
}