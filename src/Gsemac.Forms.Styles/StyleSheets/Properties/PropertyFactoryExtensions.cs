using System;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public static class PropertyFactoryExtensions {

        // Public members

        public static IProperty Create(this IPropertyFactory factory, string propertyName) {

            if (factory is null)
                throw new ArgumentNullException(nameof(factory));

            if (propertyName is null)
                throw new ArgumentNullException(nameof(propertyName));

            return factory.Create(propertyName, Enumerable.Empty<IPropertyValue>().ToArray());

        }
        public static IProperty Create(this IPropertyFactory factory, string propertyName, IPropertyValue argument) {

            if (factory is null)
                throw new ArgumentNullException(nameof(factory));

            if (argument is null)
                throw new ArgumentNullException(nameof(argument));

            return factory.Create(propertyName, new[] { argument });

        }
        public static IProperty Create<TArgument>(this IPropertyFactory factory, string propertyName, TArgument argument) {

            if (factory is null)
                throw new ArgumentNullException(nameof(factory));

            if (propertyName is null)
                throw new ArgumentNullException(nameof(propertyName));

            if (argument is IPropertyValue propertyValue)
                return factory.Create(propertyName, propertyValue);
            else
                return factory.Create(propertyName, (IPropertyValue)PropertyValue.Create(argument));

        }

    }

}