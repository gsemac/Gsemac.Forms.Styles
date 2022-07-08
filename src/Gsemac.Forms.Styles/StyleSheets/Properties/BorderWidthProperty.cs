using Gsemac.Forms.Styles.StyleSheets.Properties.Extensions;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public sealed class BorderWidthProperty :
        PropertyBase<BorderWidths> {

        // Public members

        public BorderWidthProperty() :
            this(new BorderWidths()) {
        }
        public BorderWidthProperty(BorderWidths value) :
            base(PropertyName.BorderWidth, value, inherited: false) {
        }

        //public override IEnumerable<IProperty> GetLonghands(IPropertyFactory propertyFactory) {

        //    yield return propertyFactory.Create(PropertyName.BorderTopWidth, PropertyValue.Create(Value.Top));
        //    yield return propertyFactory.Create(PropertyName.BorderRightWidth, PropertyValue.Create(Value.Right));
        //    yield return propertyFactory.Create(PropertyName.BorderBottomWidth, PropertyValue.Create(Value.Bottom));
        //    yield return propertyFactory.Create(PropertyName.BorderLeftWidth, PropertyValue.Create(Value.Left));

        //}

    }

}