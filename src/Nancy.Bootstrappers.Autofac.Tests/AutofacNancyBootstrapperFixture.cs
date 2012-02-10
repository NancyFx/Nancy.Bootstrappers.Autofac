namespace Nancy.Bootstrappers.Autofac.Tests
{
    using System.Linq;
    using Nancy.Tests;
    using Nancy.Tests.Fakes;
    using Xunit;
    using Nancy.Routing;
    using Nancy.Bootstrapper;

    public class AutofacNancyBootstrapperFixture
    {
        private readonly FakeAutofacNancyBootstrapper bootstrapper;

        public AutofacNancyBootstrapperFixture()
        {
            this.bootstrapper = new FakeAutofacNancyBootstrapper();
            this.bootstrapper.Initialise();
        }

        [Fact]
        public void GetEngine_ReturnsEngine()
        {
            // Given
            // When
            var result = this.bootstrapper.GetEngine();

            // Then
            result.ShouldNotBeNull();
            result.ShouldBeOfType<INancyEngine>();
        }

        [Fact]
        public void GetAllModules_Returns_As_MultiInstance()
        {
            // Given
            this.bootstrapper.GetEngine();
            var context = new NancyContext();

            // When
            var output1 = this.bootstrapper.GetAllModules(context).FirstOrDefault(nm => nm.GetType() == typeof(FakeNancyModuleWithBasePath));
            var output2 = this.bootstrapper.GetAllModules(context).FirstOrDefault(nm => nm.GetType() == typeof(FakeNancyModuleWithBasePath));

            // Then
            output1.ShouldNotBeNull();
            output2.ShouldNotBeNull();
            output1.ShouldNotBeSameAs(output2);
        }

        [Fact]
        public void GetModuleByKey_Returns_As_MultiInstance()
        {
            // Given
            this.bootstrapper.GetEngine();
            var context = new NancyContext();

            // When
            var output1 = this.bootstrapper.GetModuleByKey(typeof(FakeNancyModuleWithBasePath).FullName, context);
            var output2 = this.bootstrapper.GetModuleByKey(typeof(FakeNancyModuleWithBasePath).FullName, context);

            // Then
            output1.ShouldNotBeNull();
            output2.ShouldNotBeNull();
            output1.ShouldNotBeSameAs(output2);
        }

        [Fact]
        public void GetAllModules_Configures_Child_Container()
        {
            // Given
            this.bootstrapper.GetEngine();
            this.bootstrapper.RequestContainerConfigured = false;

            // When
            this.bootstrapper.GetAllModules(new NancyContext());

            // Then
            this.bootstrapper.RequestContainerConfigured.ShouldBeTrue();
        }

        [Fact]
        public void GetModuleByKey_Configures_Child_Container()
        {
            // Given
            this.bootstrapper.GetEngine();
            this.bootstrapper.RequestContainerConfigured = false;

            // When
            this.bootstrapper.GetModuleByKey(typeof(FakeNancyModuleWithBasePath).FullName, new NancyContext());

            // Then
            this.bootstrapper.RequestContainerConfigured.ShouldBeTrue();
        }

        [Fact]
        public void GetEngine_ConfigureApplicationContainer_Should_Be_Called()
        {
            // Given
            // When
            this.bootstrapper.GetEngine();

            // Then
            this.bootstrapper.ApplicationContainerConfigured.ShouldBeTrue();
        }

        [Fact]
        public void GetEngine_Defaults_Registered_In_Container()
        {
            // Given
            // When
            this.bootstrapper.GetEngine();

            // Then
            this.bootstrapper.Resolve<INancyModuleCatalog>().ShouldNotBeNull();
            this.bootstrapper.Resolve<IRouteResolver>().ShouldNotBeNull();
            this.bootstrapper.Resolve<INancyEngine>().ShouldNotBeNull();
            this.bootstrapper.Resolve<IModuleKeyGenerator>().ShouldNotBeNull();
            this.bootstrapper.Resolve<IRouteCache>().ShouldNotBeNull();
            this.bootstrapper.Resolve<IRouteCacheProvider>().ShouldNotBeNull();
        }

        [Fact]
        public void Get_Module_By_Key_Gives_Same_Request_Lifetime_Instance_To_Each_Dependency()
        {
            // Given
            this.bootstrapper.GetEngine();

            // When
            var result = this.bootstrapper.GetModuleByKey(new DefaultModuleKeyGenerator().GetKeyForModuleType(typeof(FakeNancyModuleWithDependency)), new NancyContext()) as FakeNancyModuleWithDependency;

            // Then
            result.FooDependency.ShouldNotBeNull();
            result.FooDependency.ShouldBeSameAs(result.Dependency.FooDependency);
        }

        [Fact]
        public void Should_return_the_same_instance_when_getmodulebykey_is_called_multiple_times_with_the_same_context()
        {
            // Given
            this.bootstrapper.GetEngine();
            var context = new NancyContext();

            // When
            var result = this.bootstrapper.GetModuleByKey(new DefaultModuleKeyGenerator().GetKeyForModuleType(typeof(FakeNancyModuleWithDependency)), context) as FakeNancyModuleWithDependency;
            var result2 = this.bootstrapper.GetModuleByKey(new DefaultModuleKeyGenerator().GetKeyForModuleType(typeof(FakeNancyModuleWithDependency)), context) as FakeNancyModuleWithDependency;

            // Then
            result.FooDependency.ShouldNotBeNull();
            result2.FooDependency.ShouldNotBeNull();
            result.FooDependency.ShouldBeSameAs(result2.FooDependency);
        }

        [Fact]
        public void Should_not_return_the_same_instance_when_getmodulebykey_is_called_multiple_times_with_different_context()
        {
            // Given
            this.bootstrapper.GetEngine();
            var context1 = new NancyContext();
            var context2 = new NancyContext();

            // When
            
            var result = this.bootstrapper.GetModuleByKey(new DefaultModuleKeyGenerator().GetKeyForModuleType(typeof(FakeNancyModuleWithDependency)), context1) as FakeNancyModuleWithDependency;
            var result2 = this.bootstrapper.GetModuleByKey(new DefaultModuleKeyGenerator().GetKeyForModuleType(typeof(FakeNancyModuleWithDependency)), context2) as FakeNancyModuleWithDependency;

            // Then
            result.FooDependency.ShouldNotBeNull();
            result2.FooDependency.ShouldNotBeNull();
            result.FooDependency.ShouldNotBeSameAs(result2.FooDependency);
        }
    }
}
