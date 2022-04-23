namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class Property {

        // Public members

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