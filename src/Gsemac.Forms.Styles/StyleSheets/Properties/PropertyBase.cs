using Gsemac.Forms.Styles.Properties;
using Gsemac.Forms.Styles.StyleSheets.Properties.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public abstract class PropertyBase<T> :
        IProperty<T> {

        // Public members

        public T Value => value.Value;
        public string Name { get; }

        public bool IsInheritable { get; }
        public bool IsVariable => Name.StartsWith("--");

        public Type ValueType => typeof(T);

        IPropertyValue IProperty.Value => value;

        public virtual IEnumerable<IProperty> GetChildProperties(IPropertyFactory propertyFactory) {

            return Enumerable.Empty<IProperty>();

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            sb.Append(Name);
            sb.Append(": ");

            if (ValueType.Equals(typeof(Color)))
                sb.Append(new ColorToStringConverter().Convert(value.As<Color>()));
            else if (ValueType.Equals(typeof(BorderStyle)))
                sb.Append(new BorderStyleToStringConverter().Convert(value.As<BorderStyle>()));
            else
                sb.Append(Value.ToString());

            return sb.ToString();

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
            IsInheritable = isInheritable;

            this.value = PropertyValue.Create(value);

        }

        // Private members

        private readonly IPropertyValue<T> value;

    }

}