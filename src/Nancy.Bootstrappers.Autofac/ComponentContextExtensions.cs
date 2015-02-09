using System;
using Autofac;

namespace Nancy.Bootstrappers.Autofac
{
    public static class ComponentContextExtensions
    {
        public static T Update<T>(this T context, Action<ContainerBuilder> builderAction) where T : IComponentContext
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (builderAction == null)
            {
                throw new ArgumentNullException("builderAction");
            }

            var builder = new ContainerBuilder();

            builderAction.Invoke(builder);

            builder.Update(context.ComponentRegistry);

            return context;
        }
    }
}