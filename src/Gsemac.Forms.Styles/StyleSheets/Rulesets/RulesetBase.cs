using Gsemac.Collections;
using Gsemac.Collections.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Dom;
using Gsemac.Forms.Styles.StyleSheets.Properties;
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
            get => properties[propertyName];
            set => UpdateProperty(value);
        }

        public int Count => properties.Count;
        public bool IsReadOnly => properties.IsReadOnly;

        public StyleOrigin Origin { get; } = DefaultOrigin;
        public ISelector Selector { get; } = DefaultSelector;

        public Color AccentColor => GetPropertyValueOrDefault<Color>(PropertyName.AccentColor);
        public Color BackgroundColor => GetPropertyValueOrDefault<Color>(PropertyName.BackgroundColor);
        public BackgroundImage BackgroundImage => GetPropertyValueOrDefault<BackgroundImage>(PropertyName.BackgroundImage);
        public Borders Border => GetPropertyValueOrDefault<Borders>(PropertyName.Border);
        public Border BorderBottom => GetPropertyValueOrDefault<Border>(PropertyName.BorderBottom);
        public Color BorderBottomColor => GetPropertyValueOrDefault<Color>(PropertyName.BorderBottomColor);
        public ILengthPercentage BorderBottomLeftRadius => GetPropertyValueOrDefault<ILengthPercentage>(PropertyName.BorderBottomLeftRadius);
        public ILengthPercentage BorderBottomRightRadius => GetPropertyValueOrDefault<ILengthPercentage>(PropertyName.BorderBottomRightRadius);
        public BorderStyle BorderBottomStyle => GetPropertyValueOrDefault<BorderStyle>(PropertyName.BorderBottomStyle);
        public LineWidth BorderBottomWidth => GetPropertyValueOrDefault<LineWidth>(PropertyName.BorderBottomWidth);
        public BorderColors BorderColor => GetPropertyValueOrDefault<BorderColors>(PropertyName.BorderColor);
        public Border BorderLeft => GetPropertyValueOrDefault<Border>(PropertyName.BorderLeft);
        public Color BorderLeftColor => GetPropertyValueOrDefault<Color>(PropertyName.BorderLeftColor);
        public BorderStyle BorderLeftStyle => GetPropertyValueOrDefault<BorderStyle>(PropertyName.BorderLeftStyle);
        public LineWidth BorderLeftWidth => GetPropertyValueOrDefault<LineWidth>(PropertyName.BorderLeftWidth);
        public BorderRadii BorderRadius => GetPropertyValueOrDefault<BorderRadii>(PropertyName.BorderRadius);
        public Border BorderRight => GetPropertyValueOrDefault<Border>(PropertyName.BorderRight);
        public Color BorderRightColor => GetPropertyValueOrDefault<Color>(PropertyName.BorderRightColor);
        public BorderStyle BorderRightStyle => GetPropertyValueOrDefault<BorderStyle>(PropertyName.BorderRightStyle);
        public LineWidth BorderRightWidth => GetPropertyValueOrDefault<LineWidth>(PropertyName.BorderRightWidth);
        public BorderStyles BorderStyle => GetPropertyValueOrDefault<BorderStyles>(PropertyName.BorderStyle);
        public Border BorderTop => GetPropertyValueOrDefault<Border>(PropertyName.BorderTop);
        public Color BorderTopColor => GetPropertyValueOrDefault<Color>(PropertyName.BorderTopColor);
        public ILengthPercentage BorderTopLeftRadius => GetPropertyValueOrDefault<ILengthPercentage>(PropertyName.BorderTopLeftRadius);
        public ILengthPercentage BorderTopRightRadius => GetPropertyValueOrDefault<ILengthPercentage>(PropertyName.BorderTopRightRadius);
        public BorderStyle BorderTopStyle => GetPropertyValueOrDefault<BorderStyle>(PropertyName.BorderTopStyle);
        public LineWidth BorderTopWidth => GetPropertyValueOrDefault<LineWidth>(PropertyName.BorderTopWidth);
        public BorderWidths BorderWidth => GetPropertyValueOrDefault<BorderWidths>(PropertyName.BorderWidth);
        public Color Color => GetPropertyValueOrDefault<Color>(PropertyName.Color);
        public double Opacity => GetPropertyValueOrDefault<double>(PropertyName.Opacity);
        public ScrollBarColors ScrollbarColor => GetPropertyValueOrDefault<ScrollBarColors>(PropertyName.ScrollbarColor);

        public void Add(IProperty property) {

            AddProperty(property);

        }

        public bool TryGetValue(string propertyName, out IProperty value) {

            return properties.TryGetValue(propertyName, out value);

        }

        public bool Contains(IProperty property) {

            if (property is null)
                return false;

            if (properties.TryGetValue(property.Name, out IProperty value))
                return property.Equals(value);

            return false;

        }
        public bool ContainsKey(string propertyName) {

            return properties.ContainsKey(propertyName);

        }

        public bool Remove(IProperty property) {

            if (property is null)
                return false;

            // Find the property we're going to remove.
            // The property should exactly match one we already have in the ruleset.

            if (!(properties.TryGetValue(property.Name, out IProperty propertyToRemove) && property.Equals(propertyToRemove)))
                return false;

            // Remove the property.

            properties.Remove(propertyToRemove.Name);

            RemoveLonghands(propertyToRemove);

            return true;

        }
        public bool Remove(string propertyName) {

            if (properties.TryGetValue(propertyName, out IProperty property))
                return Remove(property);

            return false;

        }

        public void Clear() {

            properties.Clear();

        }

        public void CopyTo(IProperty[] array, int arrayIndex) {

            properties.Values.CopyTo(array, arrayIndex);

        }

        public IEnumerator<IProperty> GetEnumerator() {

            return properties.Values.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            string selectorStr = Selector?.ToString() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(selectorStr)) {

                sb.Append(selectorStr);
                sb.Append(' ');

            }

            sb.AppendLine("{");

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
            this(DefaultSelector) {
        }
        protected RulesetBase(ISelector selector) :
            this(selector, DefaultOrigin) {
        }
        protected RulesetBase(ISelector selector, StyleOrigin origin) {

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            Selector = selector;
            Origin = origin;

        }
        protected RulesetBase(IRuleset other) :
            this(other?.Selector ?? DefaultSelector, other?.Origin ?? DefaultOrigin) {

            if (other is null)
                throw new ArgumentNullException(nameof(other));

            this.AddRange(other);

        }

        // Private members

        private class LonghandInfo {

            // Public members

            public IProperty Parent { get; }
            public IProperty Property { get; }

            public LonghandInfo(IProperty property, IProperty parent) {

                Property = property;
                Parent = parent;

            }

        }

        private const StyleOrigin DefaultOrigin = StyleOrigin.User;
        private static readonly ISelector DefaultSelector = Selectors.Selector.Empty;

        private readonly IPropertyFactory propertyFactory = PropertyFactory.Default;
        private readonly IDictionary<string, IProperty> properties = new OrderedDictionary<string, IProperty>(StringComparer.OrdinalIgnoreCase);
        private readonly IDictionary<string, List<LonghandInfo>> longhandProperties = new Dictionary<string, List<LonghandInfo>>(StringComparer.OrdinalIgnoreCase);

        private T GetPropertyValueOrDefault<T>(string propertyName) {

            return GetPropertyValueOrDefault(propertyName).As<T>();

        }
        private IPropertyValue GetPropertyValueOrDefault(string propertyName) {

            IProperty property = GetPropertyOrDefault(propertyName);

            // If this is a shorthand property that has been added before, we'll construct a value using the most recent longhand values we have.

            if (property.IsShorthand) {

                IPropertyValue[] arguments = property.Definition.Longhands
                    .Select(longhand => longhand.Name)
                    .Select(name => GetPropertyValueOrDefault(name))
                    .ToArray();

                property = propertyFactory.Create(property.Name, arguments);

            }

            // Resolve the property to the best of our abilty according to the information available in the ruleset.

            IStyleComputationContext context = new StyleComputationContext();

            property = context.ComputeProperty(property, Node.Empty, new[] { this });

            return property.Value;

        }
        private IProperty GetPropertyOrDefault(string propertyName) {

            // We want to return the latest value of a property, whether it was set explicitly or through a shorthand property.

            if (longhandProperties.TryGetValue(propertyName, out var longhands) && longhands.Any())
                return longhands.Last().Property;

            if (properties.TryGetValue(propertyName, out IProperty property))
                return property;

            return propertyFactory.Create(propertyName);

        }

        private void AddProperty(IProperty property) {

            if (property is null)
                throw new ArgumentNullException(nameof(property));

            // Remove any existing property with the same name so that the property is added to end of the dictionary.

            Remove(property.Name);

            properties.Add(property.Name, property);

            AddLonghands(property);

        }
        private void UpdateProperty(IProperty property) {

            if (property is null)
                throw new ArgumentNullException(nameof(property));

            // Update the property without changing its position in the dictionary.

            if (TryGetValue(property.Name, out IProperty existingProperty)) {

                RemoveLonghands(existingProperty);

                properties[property.Name] = property;

                AddLonghands(property);

            }
            else {

                Add(property);

            }

        }

        private void AddLonghands(IProperty property) {

            if (property is null)
                throw new ArgumentNullException(nameof(property));

            // Convert the property to a set of longhand properties.
            // If the property is already a longhand property, we'll use it directly.

            IEnumerable<IProperty> longhands = CreateLonghands(property);

            if (!longhands.Any())
                longhands = new[] { property };

            foreach (IProperty longhand in longhands) {

                LonghandInfo longhandInfo = new LonghandInfo(longhand, parent: property);

                if (!longhandProperties.ContainsKey(longhand.Name))
                    longhandProperties.Add(longhand.Name, new List<LonghandInfo>());

                longhandProperties[longhand.Name].Add(longhandInfo);

            }

        }
        private IEnumerable<IProperty> CreateLonghands(IProperty property) {

            if (property is null)
                throw new ArgumentNullException(nameof(property));

            if (!property.IsShorthand)
                yield break;

            // Compute the final value of the current property before attempting to create its longhands, because they may dependent on its final value.
            // For example, the longhands may not be anticipating a keyword such as "initial".

            IStyleComputationContext context = new StyleComputationContext();

            IProperty baseProperty = context.ComputeProperty(property, Node.Empty, new[] { this });

            foreach (ILonghandPropertyDefinition longhand in property.Definition.Longhands) {

                yield return propertyFactory.Create(longhand.Name, longhand.ValueFactory(baseProperty.Value));

            }

        }
        private void RemoveLonghands(IProperty property) {

            if (property is null)
                throw new ArgumentNullException(nameof(property));

            // Remove all longhand properties that belong to this property.

            if (longhandProperties.TryGetValue(property.Name, out var longhands))
                longhands.RemoveAll(i => ReferenceEquals(i.Parent, property));

        }

    }

}