using System;
using Autofac;

namespace Nancy.Bootstrappers.Autofac
{
    public static class ComponentContextExtensions
    {
        /// <summary>
        /// Updates the specified component context using the specified <paramref name="builderAction"/>.
        /// </summary>
        /// <typeparam name="T">The component context type.</typeparam>
        /// <param name="context">The component context to update.</param>
        /// <param name="builderAction">The builder action.</param>
        /// <returns>The updated component context.</returns>
        /// <exception cref="System.ArgumentNullException">If <paramref name="context"/> or <paramref name="builderAction"/> is <c>null</c>.</exception>
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