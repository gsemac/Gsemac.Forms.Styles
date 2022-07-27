using Gsemac.Core;
using Gsemac.Forms.Styles.Properties;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class PropertyValue :
        IPropertyValue {

        // Public members

        public Type Type { get; }
        public object Value { get; }

        public static PropertyValue Inherit => Create(Keyword.Inherit);
        public static PropertyValue Initial => Create(Keyword.Initial);
        public static PropertyValue Revert => Create(Keyword.Revert);
        public static PropertyValue RevertLayer => Create(Keyword.RevertLayer);
        public static PropertyValue Unset => Create(Keyword.Unset);

        public static PropertyValue Auto => Create(Keyword.Auto);
        public static PropertyValue None => Create(Keyword.None);

        public static PropertyValue CanvasText => Create(Keyword.CanvasText);
        public static PropertyValue CurrentColor => Create(Keyword.CurrentColor);

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
                .WithValue(Type)
                .WithValue(Value)
                .GetHashCode();

        }

        public override string ToString() {

            return Value.ToString();

        }

        // Internal members

        internal PropertyValue(Type type, object value) {

            if (type is null)
                throw new ArgumentNullException(nameof(type));

            if (!type.IsAssignableFrom(value.GetType()))
                throw new ArgumentException(string.Format(ExceptionMessages.GivenTypeDoesNotMatchValueType, type, value.GetType()), nameof(type));

            Type = type;
            Value = value;

        }

        internal static PropertyValue Create(Type type, object value) {

            return new PropertyValue(type, value);

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