using Gsemac.Forms.Styles.StyleSheets.Properties.Extensions;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public sealed class BorderRadiusProperty :
        PropertyBase<BorderRadii> {

        // Public members

        public BorderRadiusProperty() :
            this(new BorderRadii()) {
        }
        public BorderRadiusProperty(BorderRadii value) :
            base(PropertyName.BorderRadius, value, inherited: false) {
        }

        //public override IEnumerable<IProperty> GetLonghands(IPropertyFactory propertyFactory) {

        //    yield return propertyFactory.Create(PropertyName.BorderTopLeftRadius, PropertyValue.Create(Value.TopLeft));
        //    yield return propertyFactory.Create(PropertyName.BorderTopRightRadius, PropertyValue.Create(Value.TopRight));
        //    yield return propertyFactory.Create(PropertyName.BorderBottomRightRadius, PropertyValue.Create(Value.BottomRight));
        //    yield return propertyFactory.Create(PropertyName.BorderBottomLeftRadius, PropertyValue.Create(Value.BottomLeft));

        //}

    }

}