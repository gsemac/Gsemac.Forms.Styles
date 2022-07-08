using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public static class PropertyFactoryExtensions {

        // Public members

        public static IProperty Create(this IPropertyFactory factory, string propertyName) {

            return factory.Create(propertyName, Ruleset.Empty);

        }
        public static IProperty Create(this IPropertyFactory factory, string propertyName, IRuleset ruleset) {

            if (factory is null)
                throw new ArgumentNullException(nameof(factory));

            return factory.Create(propertyName, new IPropertyValue[] { });

        }
        public static IProperty Create(this IPropertyFactory factory, string propertyName, IPropertyValue argument) {

            return factory.Create(propertyName, argument, Ruleset.Empty);

        }
        public static IProperty Create(this IPropertyFactory factory, string propertyName, IPropertyValue argument, IRuleset ruleset) {

            if (factory is null)
                throw new ArgumentNullException(nameof(factory));

            if (argument is null)
                throw new ArgumentNullException(nameof(argument));

            return factory.Create(propertyName, new[] { argument });

        }

    }

}