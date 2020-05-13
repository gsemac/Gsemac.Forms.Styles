using System;
using System.Collections;
using System.Collections.Generic;
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

                case PropertyType.BorderTopLeftRadius:
                case PropertyType.BorderTopRightRadius:
                case PropertyType.BorderBottomLeftRadius:
                case PropertyType.BorderBottomRightRadius:

                    AddBorderRadiusProperty(property);

                    break;

                default:

                    properties[property.Type] = property;

                    break;

            }

        }
        public override IProperty GetProperty(PropertyType propertyType) {

            IProperty result = null;

            switch (propertyType) {

                case PropertyType.BorderTopLeftRadius:
                case PropertyType.BorderTopRightRadius:
                case PropertyType.BorderBottomLeftRadius:
                case PropertyType.BorderBottomRightRadius:

                    result = GetBorderRadiusProperty(propertyType);

                    break;

                default:

                    if (properties.TryGetValue(propertyType, out IProperty value))
                        result = value;

                    break;

            }

            return result;

        }

        public override IEnumerator<IProperty> GetEnumerator() {

            return properties.Values.GetEnumerator();

        }

        // Private members

        private readonly Dictionary<PropertyType, IProperty> properties = new Dictionary<PropertyType, IProperty>();

        private void AddBorderRadiusProperty(IProperty property) {

            BorderRadiusProperty borderRadius = BorderRadius ?? new BorderRadiusProperty("0.0");

            switch (property.Type) {

                case PropertyType.BorderTopLeftRadius:

                    borderRadius.Value.TopLeft = (double)property.Value;

                    break;

                case PropertyType.BorderTopRightRadius:

                    borderRadius.Value.TopRight = (double)property.Value;

                    break;

                case PropertyType.BorderBottomLeftRadius:

                    borderRadius.Value.BottomLeft = (double)property.Value;

                    break;

                case PropertyType.BorderBottomRightRadius:

                    borderRadius.Value.BottomRight = (double)property.Value;

                    break;

            }

            AddProperty(borderRadius);

        }
        private IProperty GetBorderRadiusProperty(PropertyType propertyType) {

            IProperty result = null;

            if (BorderRadius != null) {

                double radiusValue = 0.0;

                switch (propertyType) {

                    case PropertyType.BorderTopLeftRadius:

                        radiusValue = BorderRadius.Value.TopLeft;

                        break;

                    case PropertyType.BorderTopRightRadius:

                        radiusValue = BorderRadius.Value.TopRight;

                        break;

                    case PropertyType.BorderBottomLeftRadius:

                        radiusValue = BorderRadius.Value.BottomLeft;

                        break;

                    case PropertyType.BorderBottomRightRadius:

                        radiusValue = BorderRadius.Value.BottomRight;

                        break;

                }

                result = new NumberProperty(propertyType, radiusValue, false);

            }

            return result;

        }

    }

}