using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public static class PropertyDefinitionBuilderExtensions {

        // Public members

        public static IPropertyDefinitionBuilder WithType<T>(this IPropertyDefinitionBuilder builder) {

            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            return builder.WithType(typeof(T));

        }
        public static IPropertyDefinitionBuilder WithInitial<T>(this IPropertyDefinitionBuilder builder, T value) {

            return builder.WithInitial(PropertyValue.Create(value));

        }

        public static IPropertyDefinitionBuilder WithLonghand<TSource, TDestination>(this IPropertyDefinitionBuilder builder, string name, Func<TSource, TDestination> longhandValueFactory) {

            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (longhandValueFactory is null)
                throw new ArgumentNullException(nameof(longhandValueFactory));

            return builder.WithLonghand(name, CreateLonghandValueFactoryWrapper(longhandValueFactory))
                .WithType<TDestination>();

        }

        // Private members

        private static LonghandPropertyValueFactory CreateLonghandValueFactoryWrapper<TSource, TDestination>(Func<TSource, TDestination> longhandValueFactory) {

            // Create a wrapper function around the argument types.

            return (IPropertyValue value) => {

                TSource sourceObject = value.As<TSource>();
                TDestination destinationObject = longhandValueFactory(sourceObject);

                return PropertyValue.Create(destinationObject);

            };

        }

    }

}