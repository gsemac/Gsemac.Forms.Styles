using System;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public static class PropertyDefinitionExtensions {

        // Public members

        public static IProperty Create(this IPropertyDefinition propertyDefinition) {

            if (propertyDefinition is null)
                throw new ArgumentNullException(nameof(propertyDefinition));

            return propertyDefinition.Create(Enumerable.Empty<IPropertyValue>().ToArray());

        }
        public static IProperty Create(this IPropertyDefinition propertyDefinition, IPropertyValue propertyValue) {

            if (propertyDefinition is null)
                throw new ArgumentNullException(nameof(propertyDefinition));

            if (propertyValue is null)
                throw new ArgumentNullException(nameof(propertyValue));

            return propertyDefinition.Create(new[] { propertyValue });

        }

    }

}