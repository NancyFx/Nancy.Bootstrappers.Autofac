#if !__MonoCS__ 
namespace Nancy.Bootstrappers.Autofac.Tests
{
    using Bootstrapper;
    using Nancy.Tests.Unit.Bootstrapper.Base;
    using global::Autofac;

    public class AutofacBootstrapperBaseFixture : BootstrapperBaseFixtureBase<ILifetimeScope>
    {
        private readonly AutofacNancyBootstrapper bootstrapper;

        public AutofacBootstrapperBaseFixture()
        {
            this.bootstrapper = new FakeAutofacNancyBootstrapper(this.Configuration);
        }

        protected override NancyBootstrapperBase<ILifetimeScope> Bootstrapper
        {
            get { return this.bootstrapper; }
        }
    }
}
#endif