using Gsemac.Forms.Styles.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class Ruleset :
        RulesetBase {

        // Public members

        public Ruleset() {
        }
        public Ruleset(ISelector selector) :
            base(selector) {
        }

        public override void AddProperty(IProperty property) {

            switch (property.Type) {

                case PropertyType.BorderRadius:
                    AddBorderRadiusProperty(property);
                    break;

                case PropertyType.Border:
                    AddBorderProperties(property);
                    break;

                case PropertyType.BorderWidth:
                    AddBorderWidthProperties(property);
                    break;

                case PropertyType.BorderStyle:
                    AddBorderStyleProperties(property);
                    break;

                case PropertyType.BorderColor:
                    AddBorderColorProperties(property);
                    break;

            }

            properties[property.Type] = property;

        }
        public override IProperty GetProperty(PropertyType propertyType) {

            if (properties.TryGetValue(propertyType, out IProperty property))
                return property;
            else
                return null;

        }

        public override IEnumerator<IProperty> GetEnumerator() {

            return properties.Values.GetEnumerator();

        }

        // Private members

        private readonly IDictionary<PropertyType, IProperty> properties = new OrderedDictionary<PropertyType, IProperty>();

        private void AddBorderProperties(IProperty property) {

            BorderProperty borderProperty = property as BorderProperty;

            AddProperty(Property.Create(PropertyType.BorderTopWidth, borderProperty.Value.Width));
            AddProperty(Property.Create(PropertyType.BorderTopStyle, borderProperty.Value.Style));
            AddProperty(Property.Create(PropertyType.BorderTopColor, borderProperty.Value.Color));

            AddProperty(Property.Create(PropertyType.BorderRightWidth, borderProperty.Value.Width));
            AddProperty(Property.Create(PropertyType.BorderRightStyle, borderProperty.Value.Style));
            AddProperty(Property.Create(PropertyType.BorderRightColor, borderProperty.Value.Color));

            AddProperty(Property.Create(PropertyType.BorderBottomWidth, borderProperty.Value.Width));
            AddProperty(Property.Create(PropertyType.BorderBottomStyle, borderProperty.Value.Style));
            AddProperty(Property.Create(PropertyType.BorderBottomColor, borderProperty.Value.Color));

            AddProperty(Property.Create(PropertyType.BorderLeftWidth, borderProperty.Value.Width));
            AddProperty(Property.Create(PropertyType.BorderLeftStyle, borderProperty.Value.Style));
            AddProperty(Property.Create(PropertyType.BorderLeftColor, borderProperty.Value.Color));

        }
        private void AddBorderWidthProperties(IProperty property) {

            AddProperty(Property.Create(PropertyType.BorderTopWidth, property.Value));
            AddProperty(Property.Create(PropertyType.BorderRightWidth, property.Value));
            AddProperty(Property.Create(PropertyType.BorderBottomWidth, property.Value));
            AddProperty(Property.Create(PropertyType.BorderLeftWidth, property.Value));

        }
        private void AddBorderStyleProperties(IProperty property) {

            AddProperty(Property.Create(PropertyType.BorderTopStyle, property.Value));
            AddProperty(Property.Create(PropertyType.BorderRightStyle, property.Value));
            AddProperty(Property.Create(PropertyType.BorderBottomStyle, property.Value));
            AddProperty(Property.Create(PropertyType.BorderLeftStyle, property.Value));

        }
        private void AddBorderColorProperties(IProperty property) {

            AddProperty(Property.Create(PropertyType.BorderTopColor, property.Value));
            AddProperty(Property.Create(PropertyType.BorderRightColor, property.Value));
            AddProperty(Property.Create(PropertyType.BorderBottomColor, property.Value));
            AddProperty(Property.Create(PropertyType.BorderLeftColor, property.Value));

        }
        private void AddBorderRadiusProperty(IProperty property) {

            BorderRadiusProperty borderRadiusProperty = property as BorderRadiusProperty;

            AddProperty(Property.Create(PropertyType.BorderTopLeftRadius, borderRadiusProperty.Value.TopLeft));
            AddProperty(Property.Create(PropertyType.BorderTopRightRadius, borderRadiusProperty.Value.TopRight));
            AddProperty(Property.Create(PropertyType.BorderBottomLeftRadius, borderRadiusProperty.Value.BottomLeft));
            AddProperty(Property.Create(PropertyType.BorderBottomRightRadius, borderRadiusProperty.Value.BottomRight));

        }

    }

}