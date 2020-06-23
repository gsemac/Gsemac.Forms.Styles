namespace Gsemac.Forms.Styles.StyleSheets {

    public abstract class PropertyBase<T> :
        IProperty<T> {

        // Public members

        public T Value { get; }
        public string Name => Property.GetName(Type);
        public PropertyType Type { get; }
        public bool Inheritable { get; }

        object IProperty.Value => Value;

        public override string ToString() {

            return ToString(Value.ToString());

        }

        // Protected members

        protected PropertyBase(PropertyType type, T value, bool inheritable = true) {

            this.Type = type;
            this.Value = value;
            this.Inheritable = inheritable;

        }

        protected string ToString(string value) {

            return string.Format("{0}: {1}", Name, value);

        }

    }

}