#if !__MonoCS__ 
namespace Nancy.Bootstrappers.Autofac.Tests
{
    using Bootstrapper;
    using Nancy.Tests.Unit.Bootstrapper.Base;
    using global::Autofac;

    public class AutofacBootstrapperBaseFixture : BootstrapperBaseFixtureBase<ILifetimeScope>
    {
        private readonly AutofacNancyBootstrapper boostrapper;

        public AutofacBootstrapperBaseFixture()
        {
            this.boostrapper = new FakeAutofacNancyBootstrapper(this.Configuration);
        }

        protected override NancyBootstrapperBase<ILifetimeScope> Bootstrapper
        {
            get { return this.boostrapper; }
        }
    }
}
#endif