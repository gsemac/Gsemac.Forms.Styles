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
        public Color BorderBottomColor => GetPropertyValueOrDefault<Color>(PropertyName.BorderBottomColor);
        public Measurement BorderBottomLeftRadius => GetPropertyValueOrDefault<Measurement>(PropertyName.BorderBottomLeftRadius);
        public Measurement BorderBottomRightRadius => GetPropertyValueOrDefault<Measurement>(PropertyName.BorderBottomRightRadius);
        public BorderStyle BorderBottomStyle => GetPropertyValueOrDefault<BorderStyle>(PropertyName.BorderBottomStyle);
        public Measurement BorderBottomWidth => GetPropertyValueOrDefault<Measurement>(PropertyName.BorderBottomWidth);
        public Color BorderLeftColor => GetPropertyValueOrDefault<Color>(PropertyName.BorderLeftColor);
        public BorderStyle BorderLeftStyle => GetPropertyValueOrDefault<BorderStyle>(PropertyName.BorderLeftStyle);
        public Measurement BorderLeftWidth => GetPropertyValueOrDefault<Measurement>(PropertyName.BorderLeftWidth);
        public Color BorderRightColor => GetPropertyValueOrDefault<Color>(PropertyName.BorderRightColor);
        public BorderStyle BorderRightStyle => GetPropertyValueOrDefault<BorderStyle>(PropertyName.BorderRightStyle);
        public Measurement BorderRightWidth => GetPropertyValueOrDefault<Measurement>(PropertyName.BorderRightWidth);
        public Color BorderTopColor => GetPropertyValueOrDefault<Color>(PropertyName.BorderTopColor);
        public Measurement BorderTopLeftRadius => GetPropertyValueOrDefault<Measurement>(PropertyName.BorderTopLeftRadius);
        public Measurement BorderTopRightRadius => GetPropertyValueOrDefault<Measurement>(PropertyName.BorderTopRightRadius);
        public BorderStyle BorderTopStyle => GetPropertyValueOrDefault<BorderStyle>(PropertyName.BorderTopStyle);
        public Measurement BorderTopWidth => GetPropertyValueOrDefault<Measurement>(PropertyName.BorderTopWidth);
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

        // Private members

        private readonly IPropertyFactory propertyFactory;

    }

}