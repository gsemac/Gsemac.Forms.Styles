using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.Extensions {

    public static class PropertyFactoryExtensions {

        // Public members

        public static IProperty Create(this IPropertyFactory factory, string propertyName) {

            if (factory is null)
                throw new ArgumentNullException(nameof(factory));

            return factory.Create(propertyName, null);

        }
        public static IProperty Create(this IPropertyFactory factory, string propertyName, IPropertyValue argument) {

            if (factory is null)
                throw new ArgumentNullException(nameof(factory));

            return factory.Create(propertyName, new[] { argument });

        }
        public static IProperty Create<T>(this IPropertyFactory factory, string propertyName, T argument) {

            return factory.Create(propertyName, (IPropertyValue)PropertyValue.Create(argument));

        }

    }

}