﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using AutofacContrib.CommonServiceLocator;
using Microsoft.Practices.ServiceLocation;
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

            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(existingContainer));
        }

        protected override void ApplicationStartup(ILifetimeScope container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            Nancy.Json.JsonSettings.MaxJsonLength = int.MaxValue;
        }
    }
}