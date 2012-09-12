using Autofac;
using Raven.Client;
using Raven.Client.Embedded;

namespace Mithraeum.Api.Infra
{
    public class DependecyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ => new EmbeddableDocumentStore().Initialize())
                .As<IDocumentStore>()
                .SingleInstance();

            builder.Register(_ => _.Resolve<IDocumentStore>().OpenSession())
                .As<IDocumentSession>()
                .InstancePerLifetimeScope();
        }
    }
}