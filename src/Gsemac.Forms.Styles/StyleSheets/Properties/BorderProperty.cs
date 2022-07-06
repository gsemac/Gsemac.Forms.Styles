using Gsemac.Forms.Styles.StyleSheets.Properties.Extensions;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public sealed class BorderProperty :
        PropertyBase<Border> {

        // Public members

        public BorderProperty() :
            this(new Border()) {
        }
        public BorderProperty(Border value) :
            base(PropertyName.Border, value, isInheritable: false) {
        }

        public override IEnumerable<IProperty> GetLonghandProperties(IPropertyFactory propertyFactory) {

            yield return propertyFactory.Create(PropertyName.BorderTopWidth, PropertyValue.Create(Value.Width));
            yield return propertyFactory.Create(PropertyName.BorderTopStyle, PropertyValue.Create(Value.Style));
            yield return propertyFactory.Create(PropertyName.BorderTopColor, PropertyValue.Create(Value.Color));

            yield return propertyFactory.Create(PropertyName.BorderRightWidth, PropertyValue.Create(Value.Width));
            yield return propertyFactory.Create(PropertyName.BorderRightStyle, PropertyValue.Create(Value.Style));
            yield return propertyFactory.Create(PropertyName.BorderRightColor, PropertyValue.Create(Value.Color));

            yield return propertyFactory.Create(PropertyName.BorderBottomWidth, PropertyValue.Create(Value.Width));
            yield return propertyFactory.Create(PropertyName.BorderBottomStyle, PropertyValue.Create(Value.Style));
            yield return propertyFactory.Create(PropertyName.BorderBottomColor, PropertyValue.Create(Value.Color));

            yield return propertyFactory.Create(PropertyName.BorderLeftWidth, PropertyValue.Create(Value.Width));
            yield return propertyFactory.Create(PropertyName.BorderLeftStyle, PropertyValue.Create(Value.Style));
            yield return propertyFactory.Create(PropertyName.BorderLeftColor, PropertyValue.Create(Value.Color));

        }

    }

}