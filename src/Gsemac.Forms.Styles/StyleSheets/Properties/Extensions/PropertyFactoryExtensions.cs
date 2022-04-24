using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.Extensions {

    public static class PropertyFactoryExtensions {

        // Public members

        public static IProperty Create(this IPropertyFactory factory, string propertyName) {

            return Create(factory, propertyName, Ruleset.Empty);

        }
        public static IProperty Create(this IPropertyFactory factory, string propertyName, IRuleset ruleset) {

            if (factory is null)
                throw new ArgumentNullException(nameof(factory));

            return factory.Create(propertyName, null, ruleset);

        }
        public static IProperty Create(this IPropertyFactory factory, string propertyName, IPropertyValue argument) {

            return Create(factory, propertyName, argument, Ruleset.Empty);

        }
        public static IProperty Create(this IPropertyFactory factory, string propertyName, IPropertyValue argument, IRuleset ruleset) {

            if (factory is null)
                throw new ArgumentNullException(nameof(factory));

            if (argument is null)
                throw new ArgumentNullException(nameof(argument));

            return factory.Create(propertyName, new[] { argument }, ruleset);

        }

    }

}