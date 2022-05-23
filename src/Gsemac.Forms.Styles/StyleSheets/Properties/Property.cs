namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class Property :
        PropertyBase {

        // Public members

        public Property(string name, IPropertyValue value) :
            base(name, value, isInheritable: false) {
        }
        public Property(string name, IPropertyValue value, bool isInheritable) :
            base(name, value, isInheritable) {
        }

        public static Property<T> Create<T>(string name, T value) {

            return Create(name, value, isInheritable: false);

        }
        public static Property<T> Create<T>(string name, T value, bool isInheritable) {

            return new Property<T>(name, value, isInheritable);

        }

    }

    public class Property<T> :
        PropertyBase<T> {

        // Public members

        public Property(string name, T value, bool isInheritable) :
            base(name, value, isInheritable) {

        }

    }

}