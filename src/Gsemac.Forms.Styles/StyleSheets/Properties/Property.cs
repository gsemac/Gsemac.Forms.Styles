using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class Property :
        PropertyBase {

        // Public members

        public Property(string name, IPropertyValue value) :
            base(name, value, inherited: false) {
        }
        public Property(string name, IPropertyValue value, bool inherited) :
            base(name, value, inherited) {
        }

        public static Property<T> Create<T>(string name, T value) {

            return Create(name, value, inherited: false);

        }
        public static Property<T> Create<T>(string name, T value, bool inherited) {

            return new Property<T>(name, value, inherited);

        }

        public static Property Create(string name, IPropertyValue value) {

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            return new Property(name, value);

        }
        public static Property Create(string name, IPropertyValue value, bool inherited) {

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            return new Property(name, value, inherited);

        }

    }

    public class Property<T> :
        PropertyBase<T> {

        // Public members

        public Property(string name, T value, bool inherited) :
            base(name, value, inherited) {
        }

    }

}