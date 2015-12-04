A [bootstrapper](https://github.com/NancyFx/Nancy/wiki/Bootstrapper) implementation, for the [Nancy](http://nancyfx.org) framework, based on the Autofac inversion of control container.

## Usage

When Nancy detects that the `AutofacNancyBootstrapper` type is available in the AppDomain of your application, it will assume you want to use it, rather than the default one.

The easiest way to get the latest version of `AutofacNancyBootstrapper` into your application is to install the `Nancy.Bootstrappers.Autofac` nuget.

```
public class Bootstrapper : AutofacNancyBootstrapper
{
    protected override void ConfigureApplicationContainer(ILifetimeScope existingContainer)
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<User>()
               .As<IUser>()
               .SingleInstance();

        builder.Update(existingContainer.ComponentRegistry);          
    }
}
```

### Customizing

By inheriting from `AutofacNancyBootstrapper` you will gain access to the `ILifetimeScope` of the application and request containers and can perform what ever reqistations that your application requires.

```c#
public class Bootstrapper : AutofacNancyBootstrapper
{
    protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
    {
        // No registrations should be performed in here, however you may
        // resolve things that are needed during application startup.
    }

    protected override void ConfigureApplicationContainer(ILifetimeScope existingContainer)
    {
        // Perform registration that should have an application lifetime
    }

    protected override void ConfigureRequestContainer(ILifetimeScope container, NancyContext context)
    {
        // Perform registrations that should have a request lifetime
    }

    protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
    {
        // No registrations should be performed in here, however you may
        // resolve things that are needed during request startup.
    }
}
```

You can also override the `GetApplicationContainer` method and return a pre-existing container instance, instead of having Nancy create one for you. This is useful if Nancy is co-existing with another application and you want them to share a single container.

```c#
protected override ILifetimeScope GetApplicationContainer()
{
    // Return application container instance
}
```

## Contributors

* [Andreas Håkansson](http://github.com/thecodejunkie)
* [Andy Pike](http://github.com/andypike)
* [Nathan Palmer](http://github.com/nathanpalmer)
* [Steven Robbins](http://github.com/grumpydev)

## Copyright

Copyright © 2010 Andreas Håkansson, Steven Robbins and contributors

## License

Nancy.Bootstrappers.Autofac is licensed under [MIT](http://www.opensource.org/licenses/mit-license.php "Read more about the MIT license form"). Refer to license.txt for more information.
