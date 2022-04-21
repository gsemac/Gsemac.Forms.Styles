using Gsemac.Forms.Styles.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public abstract class PropertyBase<T> :
        IProperty<T> {

        // Public members

        public T Value { get; }
        public string Name { get; }

        public bool IsInheritable { get; }

        public Type ValueType => typeof(T);

        object IProperty.Value => Value;

        public virtual IEnumerable<IProperty> GetChildProperties(IPropertyFactory propertyFactory) {

            return Enumerable.Empty<IProperty>();

        }

        public override string ToString() {

            return string.Format("{0}: {1}", Name, PropertyUtilities.SerializePropertyValue(Value));

        }

        // Protected members

        protected PropertyBase(string name, T value, bool isInheritable) {

            if (name is null)
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(ExceptionMessages.PropertyNameCannotBeEmpty);

            if (value is object obj && obj is null)
                throw new ArgumentNullException(nameof(value));

            Name = name;
            Value = value;
            IsInheritable = isInheritable;

        }

    }

}