namespace Nancy.Bootstrappers.Autofac.Tests
{
    using System.Linq;
    using Nancy.Tests;
    using Nancy.Tests.Fakes;
    using Xunit;
    using Nancy.Routing;
    using Nancy.Bootstrapper;

    /// <summary>
    /// Nancy bootstrapper for the Autofac container.
    /// </summary>
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
        public void GetModule_Configures_Child_Container()
        {
            // Given
            this.bootstrapper.GetEngine();
            this.bootstrapper.RequestContainerConfigured = false;

            // When
            this.bootstrapper.GetModule(typeof(FakeNancyModuleWithBasePath), new NancyContext());

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
        public void Get_Module_Gives_Same_Request_Lifetime_Instance_To_Each_Dependency()
        {
            // Given
            this.bootstrapper.GetEngine();

            // When
            var result = this.bootstrapper.GetModule(typeof(FakeNancyModuleWithDependency), new NancyContext()) as FakeNancyModuleWithDependency;

            // Then
            result.FooDependency.ShouldNotBeNull();
            result.FooDependency.ShouldBeSameAs(result.Dependency.FooDependency);
        }

        [Fact]
        public void Should_not_return_the_same_instance_when_getmodule_is_called_multiple_times_with_different_context()
        {
            // Given
            this.bootstrapper.GetEngine();
            var context1 = new NancyContext();
            var context2 = new NancyContext();

            // When
            var result = this.bootstrapper.GetModule(typeof(FakeNancyModuleWithDependency), context1) as FakeNancyModuleWithDependency;
            var result2 = this.bootstrapper.GetModule(typeof(FakeNancyModuleWithDependency), context2) as FakeNancyModuleWithDependency;

            // Then
            result.FooDependency.ShouldNotBeNull();
            result2.FooDependency.ShouldNotBeNull();
            result.FooDependency.ShouldNotBeSameAs(result2.FooDependency);
        }
    }
}
