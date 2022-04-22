using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.Extensions {

    public static class InitialValueFactoryExtensions {

        // Public members

        public static IPropertyValue GetInitialValue(this IInitialValueFactory factory, string propertyName) {

            if (factory is null)
                throw new ArgumentNullException(nameof(factory));

            return factory.GetInitialValue(propertyName, Ruleset.Empty);

        }
        public static T GetInitialValue<T>(this IInitialValueFactory factory, string propertyName) {

            if (factory is null)
                throw new ArgumentNullException(nameof(factory));

            return factory.GetInitialValue<T>(propertyName, Ruleset.Empty);

        }

    }

}