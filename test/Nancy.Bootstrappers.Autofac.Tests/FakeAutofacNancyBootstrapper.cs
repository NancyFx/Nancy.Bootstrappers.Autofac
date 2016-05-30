namespace Nancy.Bootstrappers.Autofac.Tests
{
    using System;
    using Autofac;
    using Bootstrapper;
    using Nancy.Tests.Fakes;
    using global::Autofac;

    public class FakeAutofacNancyBootstrapper : AutofacNancyBootstrapper
    {
        public bool RequestContainerConfigured { get; set; }
        public bool ApplicationContainerConfigured { get; set; }

        private readonly Func<ITypeCatalog, NancyInternalConfiguration> configuration;

        public FakeAutofacNancyBootstrapper()
        {
        }

        public FakeAutofacNancyBootstrapper(Func<ITypeCatalog, NancyInternalConfiguration> configuration)
        {
            this.configuration = configuration;
        }

        protected override Func<ITypeCatalog, NancyInternalConfiguration> InternalConfiguration
        {
            get { return configuration ?? base.InternalConfiguration; }
        }

        protected override void ConfigureRequestContainer(ILifetimeScope container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            var builder = new ContainerBuilder();
            builder.RegisterType<Foo>().As<IFoo>().SingleInstance();
            builder.RegisterType<Dependency>().As<IDependency>().SingleInstance();
            builder.Update(container.ComponentRegistry);

            this.RequestContainerConfigured = true;
        }

        protected override void ConfigureApplicationContainer(ILifetimeScope existingContainer)
        {
            base.ConfigureApplicationContainer(existingContainer);
            this.ApplicationContainerConfigured = true;
        }

        public T Resolve<T>()
        {
            return this.ApplicationContainer.Resolve<T>();
        }
    }
}