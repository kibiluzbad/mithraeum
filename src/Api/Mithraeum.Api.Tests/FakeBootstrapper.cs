using System;
using Autofac;
using Mithraeum.Api.Modules;
using Raven.Client;

namespace Mithraeum.Api.Tests
{
    public class FakeBootstrapper : Bootstrapper
    {
        public Func<IDocumentSession> FakeSession { get; set; }
        public Func<IMoviesFinder> FakeFinder { get; set; }


        protected override void ConfigureApplicationContainer(ILifetimeScope existingContainer)
        {
            var builder = new ContainerBuilder();

            builder.Register(_ => FakeSession.Invoke())
                .As<IDocumentSession>();

            builder.Register(_ => FakeFinder.Invoke())
                .As<IMoviesFinder>();
                  

            builder.Update(existingContainer.ComponentRegistry);
        }
    }
}