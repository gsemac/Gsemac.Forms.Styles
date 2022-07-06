using Gsemac.Collections;
using Gsemac.Collections.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Properties;
using Gsemac.Forms.Styles.StyleSheets.Properties.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Selectors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets.Rulesets {

    public abstract class RulesetBase :
        IRuleset {

        // Public members

        public IProperty this[string propertyName] {
            get => Get(propertyName);
        }

        public int Count => properties.Count;
        public bool IsReadOnly => properties.IsReadOnly;

        public StyleOrigin Origin { get; } = StyleOrigin.User;
        public ISelector Selector { get; }

        public Color AccentColor => GetPropertyValueOrDefault<Color>(PropertyName.AccentColor);
        public Color BackgroundColor => GetPropertyValueOrDefault<Color>(PropertyName.BackgroundColor);
        public BackgroundImage BackgroundImage => GetPropertyValueOrDefault<BackgroundImage>(PropertyName.BackgroundImage);
        public Borders Border => GetBorder();
        public Border BorderBottom => GetBorderBottom();
        public Color BorderBottomColor => GetPropertyValueOrDefault<Color>(PropertyName.BorderBottomColor);
        public ILengthPercentage BorderBottomLeftRadius => GetPropertyValueOrDefault<ILengthPercentage>(PropertyName.BorderBottomLeftRadius);
        public ILengthPercentage BorderBottomRightRadius => GetPropertyValueOrDefault<ILengthPercentage>(PropertyName.BorderBottomRightRadius);
        public BorderStyle BorderBottomStyle => GetPropertyValueOrDefault<BorderStyle>(PropertyName.BorderBottomStyle);
        public LineWidth BorderBottomWidth => GetPropertyValueOrDefault<LineWidth>(PropertyName.BorderBottomWidth);
        public Border BorderLeft => GetBorderLeft();
        public Color BorderLeftColor => GetPropertyValueOrDefault<Color>(PropertyName.BorderLeftColor);
        public BorderStyle BorderLeftStyle => GetPropertyValueOrDefault<BorderStyle>(PropertyName.BorderLeftStyle);
        public LineWidth BorderLeftWidth => GetPropertyValueOrDefault<LineWidth>(PropertyName.BorderLeftWidth);
        public BorderRadii BorderRadius => GetBorderRadius();
        public Border BorderRight => GetBorderRight();
        public Color BorderRightColor => GetPropertyValueOrDefault<Color>(PropertyName.BorderRightColor);
        public BorderStyle BorderRightStyle => GetPropertyValueOrDefault<BorderStyle>(PropertyName.BorderRightStyle);
        public LineWidth BorderRightWidth => GetPropertyValueOrDefault<LineWidth>(PropertyName.BorderRightWidth);
        public Border BorderTop => GetBorderTop();
        public Color BorderTopColor => GetPropertyValueOrDefault<Color>(PropertyName.BorderTopColor);
        public ILengthPercentage BorderTopLeftRadius => GetPropertyValueOrDefault<ILengthPercentage>(PropertyName.BorderTopLeftRadius);
        public ILengthPercentage BorderTopRightRadius => GetPropertyValueOrDefault<ILengthPercentage>(PropertyName.BorderTopRightRadius);
        public BorderStyle BorderTopStyle => GetPropertyValueOrDefault<BorderStyle>(PropertyName.BorderTopStyle);
        public LineWidth BorderTopWidth => GetPropertyValueOrDefault<LineWidth>(PropertyName.BorderTopWidth);
        public BorderWidths BorderWidth => GetBorderWidth();
        public Color Color => GetPropertyValueOrDefault<Color>(PropertyName.Color);
        public double Opacity => GetPropertyValueOrDefault<double>(PropertyName.Opacity);

        public IProperty Get(string propertyName) {

            return GetPropertyOrDefault(propertyName);

        }
        public bool Contains(string propertyName) {

            return properties.ContainsKey(propertyName);

        }
        public bool Remove(string propertyName) {

            variableReferencingProperties.Remove(propertyName);

            return properties.Remove(propertyName);

        }

        public void Add(IProperty property) {

            if (property is null)
                throw new ArgumentNullException(nameof(property));

            // If the property has longhand properties (e.g. is a shorthand property), add those properties instead.
            // This way, when the user queries for the shorthand property, they always get the most up-to-date values.

            IEnumerable<IProperty> longhandProperties = property.GetLonghandProperties(propertyFactory);

            if (longhandProperties.Any()) {

                foreach (IProperty longhandProperty in longhandProperties)
                    Add(longhandProperty);

            }
            else if (property.IsVariable) {

                // We need to update all properties that reference this variable.
                // Don't remove the properties, only update them-- this preserves their position in the ruleset.

                properties.Add(property.Name, property);

                foreach (IProperty propertyToUpdate in variableReferencingProperties.Values.Where(p => p.Value.As<VariableReference>().Name.Equals(property.Name)).ToArray()) {

                    variableReferencingProperties.Remove(propertyToUpdate.Name);

                    Add(propertyToUpdate);

                }

            }
            else if (property.Value.Is<VariableReference>()) {

                // If the property value is a variable reference, we will resolve the reference immediately.

                variableReferencingProperties.Add(property.Name, property);

                string variableName = property.Value.As<VariableReference>().Name;

                IProperty updatedProperty;

                if (properties.TryGetValue(variableName, out IProperty referencedVariableProperty)) {

                    updatedProperty = propertyFactory.Create(property.Name, referencedVariableProperty.Value, this);

                }
                else {

                    // The variable hasn't been defined yet, so use the default value.

                    updatedProperty = propertyFactory.Create(property.Name, this);

                }

                // Update the property if it exists, preserving its position in the ruleset.

                properties[property.Name] = updatedProperty;

            }
            else {

                // Remove any existing property with the same name so that the property is added to end of the dictionary.

                Remove(property.Name);

                properties.Add(property.Name, property);

            }

        }
        public void Clear() {

            properties.Clear();

        }
        public bool Contains(IProperty property) {

            if (property is null)
                return false;

            if (properties.TryGetValue(property.Name, out IProperty value))
                return property.Equals(value);

            return false;

        }
        public void CopyTo(IProperty[] array, int arrayIndex) {

            properties.Values.CopyTo(array, arrayIndex);

        }
        public bool Remove(IProperty property) {

            if (property is null)
                return false;

            if (properties.TryGetValue(property.Name, out IProperty value) && property.Equals(value))
                return Remove(property.Name);

            return false;

        }

        public IEnumerator<IProperty> GetEnumerator() {

            return properties.Values.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            sb.Append(Selector?.ToString() ?? "");
            sb.AppendLine(" {");

            foreach (IProperty property in this) {

                sb.Append('\t');
                sb.Append(property.ToString());
                sb.AppendLine(";");

            }

            sb.Append("}");

            return sb.ToString();

        }

        // Protected members

        protected RulesetBase() :
            this(PropertyFactory.Default) {
        }
        protected RulesetBase(IPropertyFactory propertyFactory) {

            if (propertyFactory is null)
                throw new ArgumentNullException(nameof(propertyFactory));

            this.propertyFactory = propertyFactory;

        }
        protected RulesetBase(ISelector selector) :
            this(selector, PropertyFactory.Default) {
        }
        protected RulesetBase(ISelector selector, StyleOrigin origin) :
          this(selector, PropertyFactory.Default) {

            Origin = origin;

        }
        protected RulesetBase(ISelector selector, IPropertyFactory propertyFactory) :
            this(propertyFactory) {

            Selector = selector;

        }
        protected RulesetBase(IRuleset other) :
            this(other, PropertyFactory.Default) {
        }
        protected RulesetBase(IRuleset other, IPropertyFactory propertyFactory) :
             this(propertyFactory) {

            if (other is null)
                throw new ArgumentNullException(nameof(other));

            Selector = other.Selector;

            this.AddRange(other);

        }

        // Private members

        // TODO: The property dictionary should be case-insensitive

        private readonly IPropertyFactory propertyFactory;
        private readonly IDictionary<string, IProperty> properties = new OrderedDictionary<string, IProperty>();
        private readonly IDictionary<string, IProperty> variableReferencingProperties = new Dictionary<string, IProperty>();

        private T GetPropertyValueOrDefault<T>(string propertyName) {

            return GetPropertyOrDefault(propertyName).Value.As<T>();

        }
        private IProperty GetPropertyOrDefault(string propertyName) {

            if (properties.TryGetValue(propertyName, out IProperty property))
                return property;

            return propertyFactory.Create(propertyName, this);

        }

        private Borders GetBorder() {

            return new Borders(BorderTop, BorderRight, BorderBottom, BorderLeft);

        }
        private Border GetBorderBottom() {

            return new Border(BorderBottomWidth, BorderBottomStyle, BorderBottomColor);

        }
        private Border GetBorderLeft() {

            return new Border(BorderLeftWidth, BorderLeftStyle, BorderLeftColor);

        }
        private Border GetBorderRight() {

            return new Border(BorderRightWidth, BorderRightStyle, BorderRightColor);

        }
        private Border GetBorderTop() {

            return new Border(BorderTopWidth, BorderTopStyle, BorderTopColor);

        }
        private BorderRadii GetBorderRadius() {

            return new BorderRadii(BorderTopLeftRadius, BorderTopRightRadius, BorderBottomRightRadius, BorderBottomLeftRadius);

        }
        private BorderWidths GetBorderWidth() {

            return new BorderWidths(BorderTopWidth, BorderRightWidth, BorderBottomWidth, BorderLeftWidth);
        }

    }

}