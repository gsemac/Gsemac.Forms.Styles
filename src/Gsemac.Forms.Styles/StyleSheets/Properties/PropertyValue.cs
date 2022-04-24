using Gsemac.Core;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class PropertyValue :
        IPropertyValue {

        // Public members

        public Type Type { get; }
        public object Value { get; }

        public static PropertyValue Null => new PropertyValue(typeof(object), null);

        public static PropertyValue Initial => Create("initial");

        public static PropertyValue<T> Create<T>(T value) {

            return new PropertyValue<T>(value);

        }

        public override bool Equals(object obj) {

            if (obj is IPropertyValue other) {

                return other.Type.Equals(Type) &&
                    other.Value.Equals(Value);

            }

            return false;

        }
        public override int GetHashCode() {

            return new HashCodeBuilder()
                .Add(Type)
                .Add(Value)
                .GetHashCode();

        }

        public override string ToString() {

            return Value.ToString();

        }

        // Internal members

        internal PropertyValue(Type type, object value) {

            if (type is null)
                throw new ArgumentNullException(nameof(type));

            Type = type;
            Value = value;

        }

    }

    public class PropertyValue<T> :
        PropertyValue,
        IPropertyValue<T> {

        // Public members

        public new T Value { get; }

        public PropertyValue(T value) :
            base(typeof(T), value) {

            Value = value;

        }

    }

}