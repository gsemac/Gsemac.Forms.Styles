using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class PropertyValue :
        IPropertyValue {

        // Public members

        public Type Type { get; }
        public object Value { get; }

        public static PropertyValue Null { get; } = new PropertyValue(typeof(object), null);

        public static PropertyValue<T> Create<T>(T value) {

            return new PropertyValue<T>(value);

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