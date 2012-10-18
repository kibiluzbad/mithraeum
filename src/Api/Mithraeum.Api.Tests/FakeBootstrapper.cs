using System;
using System.Collections.Generic;
using Autofac;
using Mithraeum.Api.Infra.Queries;
using Mithraeum.Api.Model;
using Mithraeum.Api.Modules;
using Raven.Client;

namespace Mithraeum.Api.Tests
{
    public class FakeBootstrapper : Bootstrapper
    {
        public Func<IDocumentSession> FakeSession { get; set; }
        public Func<IMoviesFinder> FakeFinder { get; set; }
        public Func<IQueryFactory> FakeQueryFactor { get; set; }

        public IEnumerable<Action<ContainerBuilder>> RegisterTypes { get; set; }


        protected override void ConfigureApplicationContainer(ILifetimeScope existingContainer)
        {
            var builder = new ContainerBuilder();

            builder.Register(_ => FakeSession.Invoke())
                .As<IDocumentSession>();

            builder.Register(_ => FakeFinder.Invoke())
                .As<IMoviesFinder>();

            builder.Register(_ =>FakeQueryFactor.Invoke())
              .As<IQueryFactory>();

            if (null != RegisterTypes)
            {
                foreach (var registerType in RegisterTypes)
                {
                    registerType.Invoke(builder);
                }
            }


            builder.Update(existingContainer.ComponentRegistry);
        }
    }
}