using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using Nancy;
using Nancy.Bootstrappers.Autofac;

namespace Mithraeum.Api
{
    public class Bootstrapper 
        : AutofacNancyBootstrapper  
    {
        protected override void ConfigureApplicationContainer(Autofac.ILifetimeScope existingContainer)
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());

            builder.Update(existingContainer.ComponentRegistry);
        }
    }
}