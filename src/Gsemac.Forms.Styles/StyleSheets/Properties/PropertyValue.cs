using Gsemac.Core;
using Gsemac.Forms.Styles.Properties;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class PropertyValue :
        IPropertyValue {

        // Public members

        public Type Type { get; }
        public object Value { get; }

        public bool IsKeyword { get; private set; }
        public bool IsVariableReference => Type.Equals(typeof(VariableReference));

        public static PropertyValue Null => Create(typeof(object), null);

        public static PropertyValue Inherit => Create("inherit", isKeyword: true);
        public static PropertyValue Initial => Create("initial", isKeyword: true);
        public static PropertyValue Revert => Create("revert", isKeyword: true);
        public static PropertyValue RevertLayer => Create("revert-layer", isKeyword: true);
        public static PropertyValue Unset => Create("unset", isKeyword: true);

        public static PropertyValue Auto => Create("auto", isKeyword: true);
        public static PropertyValue CurrentColor => Create("currentcolor", isKeyword: true);
        public static PropertyValue None => Create("none", isKeyword: true);

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

            if (!type.IsAssignableFrom(value.GetType()))
                throw new ArgumentException(string.Format(ExceptionMessages.GivenTypeDoesNotMatchValueType, type, value.GetType()), nameof(type));

            Type = type;
            Value = value;

        }

        internal static PropertyValue Create(Type type, object value) {

            return new PropertyValue(type, value);

        }

        // Private members

        private static PropertyValue Create<T>(T value, bool isKeyword) {

            return new PropertyValue(typeof(T), value) {
                IsKeyword = isKeyword,
            };

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