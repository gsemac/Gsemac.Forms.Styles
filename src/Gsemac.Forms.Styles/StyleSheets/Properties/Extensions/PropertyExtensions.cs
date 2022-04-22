using System;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.Extensions {

    public static class PropertyExtensions {

        // Public members

        public static IEnumerable<IProperty> GetChildProperties(this IProperty property) {

            if (property is null)
                throw new ArgumentNullException(nameof(property));

            return property.GetChildProperties(PropertyFactory.Default);

        }

    }

}