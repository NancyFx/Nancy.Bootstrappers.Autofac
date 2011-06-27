using Autofac;
using Nancy.Bootstrapper;

namespace Nancy.Bootstrappers.Autofac
{
    using System;
    using System.Collections.Generic;

    public class AutofacNancyBootstrapper : NancyBootstrapperWithRequestContainerBase<ILifetimeScope>
    {
        /// <summary>
        /// Gets all registered startup tasks
        /// </summary>
        /// <returns>An <see cref="System.Collections.Generic.IEnumerable{T}"/> instance containing <see cref="IStartup"/> instances. </returns>
        protected override IEnumerable<IStartup> GetStartupTasks()
        {
            return ApplicationContainer.Resolve<IEnumerable<IStartup>>();
        }

        /// <summary>
        /// Get INancyEngine
        /// </summary>
        /// <returns>INancyEngine implementation</returns>
        protected override INancyEngine GetEngineInternal()
        {
            return ApplicationContainer.Resolve<INancyEngine>();
        }

        /// <summary>
        /// Get the moduleKey generator
        /// </summary>
        /// <returns>IModuleKeyGenerator instance</returns>
        protected override IModuleKeyGenerator GetModuleKeyGenerator()
        {
            return ApplicationContainer.Resolve<IModuleKeyGenerator>();
        }

        /// <summary>
        /// Create a default, unconfigured, container
        /// </summary>
        /// <returns>Container instance</returns>
        protected override ILifetimeScope GetApplicationContainer()
        {
            var builder = new ContainerBuilder();
            return builder.Build();
        }

        /// <summary>
        /// Bind the bootstrapper's implemented types into the container.
        /// This is necessary so a user can pass in a populated container but not have
        /// to take the responsibility of registering things like INancyModuleCatalog manually.
        /// </summary>
        /// <param name="applicationContainer">Application container to register into</param>
        protected override void RegisterBootstrapperTypes(ILifetimeScope applicationContainer)
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(this).As<INancyModuleCatalog>();
            builder.Update(ApplicationContainer.ComponentRegistry);
        }

        /// <summary>
        /// Bind the default implementations of internally used types into the container as singletons
        /// </summary>
        /// <param name="container">Container to register into</param>
        /// <param name="typeRegistrations">Type registrations to register</param>
        protected override void RegisterTypes(ILifetimeScope container, IEnumerable<TypeRegistration> typeRegistrations)
        {
            var builder = new ContainerBuilder();
            foreach(var typeRegistration in typeRegistrations)
            {
                builder.RegisterType(typeRegistration.ImplementationType).As(typeRegistration.RegistrationType).SingleInstance();
            }
            builder.Update(ApplicationContainer.ComponentRegistry);
        }

        /// <summary>
        /// Bind the various collections into the container as singletons to later be resolved
        /// by IEnumerable{Type} constructor dependencies.
        /// </summary>
        /// <param name="container">Container to register into</param>
        /// <param name="collectionTypeRegistrations">Collection type registrations to register</param>
        protected override void RegisterCollectionTypes(ILifetimeScope container, IEnumerable<CollectionTypeRegistration> collectionTypeRegistrations)
        {
            var builder = new ContainerBuilder();
            foreach (CollectionTypeRegistration collectionTypeRegistration in collectionTypeRegistrations)
            {
                foreach (Type implementationType in collectionTypeRegistration.ImplementationTypes)
                {
                    builder.RegisterType(implementationType).As(collectionTypeRegistration.RegistrationType).SingleInstance();
                }
            }
            builder.Update(container.ComponentRegistry);
        }

        /// <summary>
        /// Bind the given instances into the container
        /// </summary>
        /// <param name="container">Container to register into</param>
        /// <param name="instanceRegistrations">Instance registration types</param>
        protected override void RegisterInstances(ILifetimeScope container, IEnumerable<InstanceRegistration> instanceRegistrations)
        {
            var builder = new ContainerBuilder();
            foreach (InstanceRegistration instanceRegistration in instanceRegistrations)
            {
                builder.RegisterInstance(instanceRegistration.Implementation).As(instanceRegistration.RegistrationType);
            }
            builder.Update(container.ComponentRegistry);
        }

        /// <summary>
        /// Creates a per request child/nested container
        /// </summary>
        /// <returns>Request container instance</returns>
        protected override ILifetimeScope CreateRequestContainer()
        {
            return ApplicationContainer.BeginLifetimeScope();
        }

        /// <summary>
        /// Bind the given module types into the container
        /// </summary>
        /// <param name="container">Container to register into</param>
        /// <param name="moduleRegistrationTypes">NancyModule types</param>
        protected override void RegisterRequestContainerModules(ILifetimeScope container, IEnumerable<ModuleRegistration> moduleRegistrationTypes)
        {
            var builder = new ContainerBuilder();
            foreach (ModuleRegistration moduleRegistrationType in moduleRegistrationTypes)
            {
                builder.RegisterType(moduleRegistrationType.ModuleType).As(typeof(NancyModule)).Named<NancyModule>(moduleRegistrationType.ModuleKey);
            }
            builder.Update(container.ComponentRegistry);
        }

        /// <summary>
        /// Retrieve all module instances from the container
        /// </summary>
        /// <param name="container">Container to use</param>
        /// <returns>Collection of NancyModule instances</returns>
        protected override IEnumerable<NancyModule> GetAllModules(ILifetimeScope container)
        {
            //using (var child = ApplicationContainer.BeginLifetimeScope())
            //{
            //    return child.Resolve<IEnumerable<NancyModule>>();
            //}
            return container.Resolve<IEnumerable<NancyModule>>();
        }

        /// <summary>
        /// Retreive a specific module instance from the container by its key
        /// </summary>
        /// <param name="container">Container to use</param>
        /// <param name="moduleKey">Module key of the module</param>
        /// <returns>NancyModule instance</returns>
        protected override NancyModule GetModuleByKey(ILifetimeScope container, string moduleKey)
        {
            return container.ResolveNamed(moduleKey, typeof(NancyModule)) as NancyModule;
        }
    }
}