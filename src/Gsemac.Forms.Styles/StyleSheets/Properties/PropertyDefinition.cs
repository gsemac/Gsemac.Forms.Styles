using Gsemac.Forms.Styles.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    internal class PropertyDefinition :
        IPropertyDefinition {

        // Public members

        public string Name { get; set; }
        public Type ValueType { get; set; } = typeof(object);
        public IPropertyValue InitialValue { get; set; } = PropertyValue.Initial;
        public bool Inherited { get; set; } = false;
        public bool IsShorthand => longhands.Any();

        public IProperty Create(IPropertyValue[] arguments) {

            string propertyName = FormatPropertyName(Name);

            IPropertyValue propertyValue;

            // We need to convert the given arguments into an object of type ValueType.

            if (arguments.Length <= 0) {

                // If we haven't been given any arguments, just set the value to the property's initial value.

                propertyValue = InitialValue;

            }
            else if (arguments.Length == 1 && (arguments[0].Type.Equals(ValueType) || arguments[0].IsVariableReference || arguments[0].IsKeyword)) {

                // We have a single argument that is the exact type we need, so simply use the argument directly.

                propertyValue = arguments[0];

            }
            else if (ValueType.Equals(typeof(BackgroundImage))) {

                propertyValue = PropertyValue.Create(CreateBackgroundImage(arguments));

            }
            else if (ValueType.Equals(typeof(Border))) {

                propertyValue = PropertyValue.Create(CreateBorder(arguments));

            }
            else if (ValueType.Equals(typeof(BorderRadii))) {

                propertyValue = PropertyValue.Create(CreateBorderRadius(arguments));

            }
            else if (ValueType.Equals(typeof(BorderWidths))) {

                propertyValue = PropertyValue.Create(CreateBorderWidths(arguments));

            }
            else if (arguments.Length == 1) {

                // We have a single argument, but its type is not the one we need.
                // Try to convert it to the desired type using the ValueConverter implementations.

                propertyValue = arguments[0].As(ValueType);

            }
            else {

                // We have multiple arguments but no meaningful way to interpret them.

                throw new ArgumentException(string.Format(ExceptionMessages.InvalidPropertyArguments, propertyName), nameof(arguments));

            }

            // Create the new property.

            return new Property(this, propertyName, propertyValue);

        }

        public IEnumerable<IPropertyDefinition> GetLonghands() {

            return longhands.Select(item => item.Definition);

        }

        public void AddLonghand(IPropertyDefinition definition, LonghandPropertyValueFactory valueFactory) {

            if (definition is null)
                throw new ArgumentNullException(nameof(definition));

            if (valueFactory is null)
                throw new ArgumentNullException(nameof(valueFactory));

            longhands.Add(new LonghandPropertyInfo(definition, valueFactory));

        }

        // Private members

        private class LonghandPropertyInfo {

            // Public members

            public IPropertyDefinition Definition { get; }
            public LonghandPropertyValueFactory ValueFactory { get; }

            public LonghandPropertyInfo(IPropertyDefinition definition, LonghandPropertyValueFactory valueFactory) {

                Definition = definition;
                ValueFactory = valueFactory;

            }

        }

        private class Property :
            PropertyBase {

            // Public members

            public Property(PropertyDefinition definition, string name, IPropertyValue value) :
                base(name, value, definition.Inherited) {

                this.definition = definition;

            }

            public override IEnumerable<IProperty> GetLonghands() {

                foreach (LonghandPropertyInfo longhand in definition.longhands) {

                    yield return longhand.Definition.Create(longhand.ValueFactory(Value));

                }

            }

            // Private members

            private readonly PropertyDefinition definition;

        }

        private readonly List<LonghandPropertyInfo> longhands = new List<LonghandPropertyInfo>();

        private static string FormatPropertyName(string value) {

            return (value ?? string.Empty)?.Trim();

        }

        private static BackgroundImage CreateBackgroundImage(IPropertyValue[] arguments) {

            return new BackgroundImage(arguments.Select(v => v.As<IImage>()));

        }
        private static Border CreateBorder(IPropertyValue[] arguments) {

            // TODO: Implement this

            throw new NotImplementedException();

            //// At least a border style MUST be specified.
            //// Arguments are allowed to occur in any order.

            //BorderStyle? borderStyle = values.Select(value => value.GetString())
            //    .Select(value => PropertyUtilities.TryParseBorderStyle(value, out BorderStyle result) ? (BorderStyle?)result : null)
            //    .Where(result => result != null)
            //    .FirstOrDefault();

            //if (!borderStyle.HasValue)
            //    throw new ArgumentException(nameof(values));

            //double borderWidth = values.Select(value => value.GetString())
            //    .Select(value => PropertyUtilities.TryParseNumber(value, out double result) ? (double?)result : null)
            //    .Where(result => result != null)
            //    .FirstOrDefault() ?? 0.0;

            //Color borderColor = values.Select(value => value.GetString())
            //   .Select(value => PropertyUtilities.TryParseColor(value, out Color result) ? (Color?)result : null)
            //   .Where(result => result != null)
            //   .FirstOrDefault() ?? default;

            //return new Border(borderWidth, borderStyle.Value, borderColor);

        }
        private static BorderRadii CreateBorderRadius(IPropertyValue[] arguments) {

            Length[] measurments = arguments
                .Select(arg => arg.As<Length>())
                .ToArray();

            if (measurments.Count() == 4)
                return new BorderRadii(measurments[0], measurments[1], measurments[2], measurments[3]);
            else if (measurments.Count() == 3)
                return new BorderRadii(measurments[0], measurments[1], measurments[2]);
            else if (measurments.Count() == 2)
                return new BorderRadii(measurments[0], measurments[1]);
            else
                return new BorderRadii(measurments[0]);

        }
        private static BorderWidths CreateBorderWidths(IPropertyValue[] arguments) {

            LineWidth[] measurments = arguments
                .Select(arg => arg.As<LineWidth>())
                .ToArray();

            if (measurments.Count() == 4)
                return new BorderWidths(measurments[0], measurments[1], measurments[2], measurments[3]);
            else if (measurments.Count() == 3)
                return new BorderWidths(measurments[0], measurments[1], measurments[2]);
            else if (measurments.Count() == 2)
                return new BorderWidths(measurments[0], measurments[1]);
            else
                return new BorderWidths(measurments[0]);

        }

    }

}