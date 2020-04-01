using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        protected PropertyBase(PropertyType propertyType, T propertyValue, bool inheritable = true) {

            this.Type = propertyType;
            this.Value = propertyValue;
            this.Inheritable = inheritable;

        }

        protected string ToString(string propertyValue) {

            return string.Format("{0}: {1}", Name, propertyValue);

        }

    }

}