using Gsemac.Collections;
using Gsemac.Collections.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Properties;
using Gsemac.Forms.Styles.StyleSheets.Properties.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Selectors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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

        public ISelector Selector { get; }

        public Color BackgroundColor => GetPropertyValueOrDefault<Color>(PropertyName.BackgroundColor);
        public BackgroundImage BackgroundImage => GetPropertyValueOrDefault<BackgroundImage>(PropertyName.BackgroundImage);
        public Borders Border => GetBorder();
        public Border BorderBottom => GetBorderBottom();
        public Color BorderBottomColor => GetPropertyValueOrDefault<Color>(PropertyName.BorderBottomColor);
        public IMeasurement BorderBottomLeftRadius => GetPropertyValueOrDefault<IMeasurement>(PropertyName.BorderBottomLeftRadius);
        public IMeasurement BorderBottomRightRadius => GetPropertyValueOrDefault<IMeasurement>(PropertyName.BorderBottomRightRadius);
        public BorderStyle BorderBottomStyle => GetPropertyValueOrDefault<BorderStyle>(PropertyName.BorderBottomStyle);
        public IMeasurement BorderBottomWidth => GetPropertyValueOrDefault<IMeasurement>(PropertyName.BorderBottomWidth);
        public Border BorderLeft => GetBorderLeft();
        public Color BorderLeftColor => GetPropertyValueOrDefault<Color>(PropertyName.BorderLeftColor);
        public BorderStyle BorderLeftStyle => GetPropertyValueOrDefault<BorderStyle>(PropertyName.BorderLeftStyle);
        public IMeasurement BorderLeftWidth => GetPropertyValueOrDefault<IMeasurement>(PropertyName.BorderLeftWidth);
        public BorderRadius BorderRadius => GetBorderRadius();
        public Border BorderRight => GetBorderRight();
        public Color BorderRightColor => GetPropertyValueOrDefault<Color>(PropertyName.BorderRightColor);
        public BorderStyle BorderRightStyle => GetPropertyValueOrDefault<BorderStyle>(PropertyName.BorderRightStyle);
        public IMeasurement BorderRightWidth => GetPropertyValueOrDefault<IMeasurement>(PropertyName.BorderRightWidth);
        public Border BorderTop => GetBorderTop();
        public Color BorderTopColor => GetPropertyValueOrDefault<Color>(PropertyName.BorderTopColor);
        public IMeasurement BorderTopLeftRadius => GetPropertyValueOrDefault<IMeasurement>(PropertyName.BorderTopLeftRadius);
        public IMeasurement BorderTopRightRadius => GetPropertyValueOrDefault<IMeasurement>(PropertyName.BorderTopRightRadius);
        public BorderStyle BorderTopStyle => GetPropertyValueOrDefault<BorderStyle>(PropertyName.BorderTopStyle);
        public IMeasurement BorderTopWidth => GetPropertyValueOrDefault<IMeasurement>(PropertyName.BorderTopWidth);
        public Color Color => GetPropertyValueOrDefault<Color>(PropertyName.Color);
        public double Opacity => GetPropertyValueOrDefault<double>(PropertyName.Opacity);

        public IProperty Get(string propertyName) {

            return GetPropertyOrDefault(propertyName);

        }
        public bool Contains(string propertyName) {

            return properties.ContainsKey(propertyName);

        }
        public bool Remove(string propertyName) {

            return properties.Remove(propertyName);

        }

        public void Add(IProperty property) {

            if (property is null)
                throw new ArgumentNullException(nameof(property));

            // Remove any existing property with the same name so that the property is added to end of the dictionary.

            Remove(property.Name);

            properties.Add(property.Name, property);

            foreach (IProperty childProperty in property.GetChildProperties(propertyFactory))
                Add(childProperty);

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
                return properties.Remove(property.Name);

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

        private readonly IDictionary<string, IProperty> properties = new OrderedDictionary<string, IProperty>();

        private T GetPropertyValueOrDefault<T>(string propertyName) {

            if (properties.TryGetValue(propertyName, out IProperty property) && property.ValueType.Equals(typeof(T)))
                return (T)property.Value;

            return PropertyUtilities.GetInitialValue<T>(propertyName, this);

        }
        private IProperty GetPropertyOrDefault(string propertyName) {

            if (properties.TryGetValue(propertyName, out IProperty property))
                return property;

            return propertyFactory.Create(propertyName);

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
        private BorderRadius GetBorderRadius() {

            return new BorderRadius(BorderTopLeftRadius, BorderTopRightRadius, BorderBottomRightRadius, BorderBottomLeftRadius);

        }

        // Private members

        private readonly IPropertyFactory propertyFactory;

    }

}