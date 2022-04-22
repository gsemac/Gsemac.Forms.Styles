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

        public override IEnumerable<IProperty> GetChildProperties(IPropertyFactory propertyFactory) {

            yield return propertyFactory.Create(PropertyName.BorderTopWidth, Value.Width);
            yield return propertyFactory.Create(PropertyName.BorderTopStyle, Value.Style);
            yield return propertyFactory.Create(PropertyName.BorderTopColor, Value.Color);

            yield return propertyFactory.Create(PropertyName.BorderRightWidth, Value.Width);
            yield return propertyFactory.Create(PropertyName.BorderRightStyle, Value.Style);
            yield return propertyFactory.Create(PropertyName.BorderRightColor, Value.Color);

            yield return propertyFactory.Create(PropertyName.BorderBottomWidth, Value.Width);
            yield return propertyFactory.Create(PropertyName.BorderBottomStyle, Value.Style);
            yield return propertyFactory.Create(PropertyName.BorderBottomColor, Value.Color);

            yield return propertyFactory.Create(PropertyName.BorderLeftWidth, Value.Width);
            yield return propertyFactory.Create(PropertyName.BorderLeftStyle, Value.Style);
            yield return propertyFactory.Create(PropertyName.BorderLeftColor, Value.Color);

        }

    }

}