using Gsemac.Forms.Styles.StyleSheets.Properties.Extensions;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public sealed class BorderRadiusProperty :
        PropertyBase<BorderRadius> {

        // Public members

        public BorderRadiusProperty() :
            this(new BorderRadius()) {
        }
        public BorderRadiusProperty(BorderRadius value) :
            base(PropertyName.BorderRadius, value, isInheritable: false) {
        }

        public override IEnumerable<IProperty> GetChildProperties(IPropertyFactory propertyFactory) {

            yield return propertyFactory.Create(PropertyName.BorderTopLeftRadius, PropertyValue.Create(Value.TopLeft));
            yield return propertyFactory.Create(PropertyName.BorderTopRightRadius, PropertyValue.Create(Value.TopRight));
            yield return propertyFactory.Create(PropertyName.BorderBottomRightRadius, PropertyValue.Create(Value.BottomRight));
            yield return propertyFactory.Create(PropertyName.BorderBottomLeftRadius, PropertyValue.Create(Value.BottomLeft));

        }

    }

}