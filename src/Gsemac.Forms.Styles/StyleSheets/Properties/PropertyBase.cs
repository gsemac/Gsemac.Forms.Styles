﻿using Gsemac.Forms.Styles.Properties;
using Gsemac.Forms.Styles.StyleSheets.Properties.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public abstract class PropertyBase :
      IProperty {

        // Public members

        public string Name { get; }
        public IPropertyValue Value { get; }

        public bool IsInheritable { get; }
        public bool IsShorthand => this.GetChildProperties().Any();
        public bool IsVariable => Name.StartsWith("--");

        public Type ValueType => Value.Type;

        public virtual IEnumerable<IProperty> GetLonghandProperties(IPropertyFactory propertyFactory) {

            return Enumerable.Empty<IProperty>();

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            sb.Append(Name);
            sb.Append(": ");

            if (ValueType.Equals(typeof(Color)))
                sb.Append(new ColorToStringConverter().Convert(Value.As<Color>()));
            else if (ValueType.Equals(typeof(BorderStyle)))
                sb.Append(new BorderStyleToStringConverter().Convert(Value.As<BorderStyle>()));
            else
                sb.Append(Value.ToString());

            return sb.ToString();

        }

        // Protected members

        protected PropertyBase(string name, IPropertyValue value, bool isInheritable) {

            if (name is null)
                throw new ArgumentNullException(nameof(name));

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(ExceptionMessages.PropertyNameCannotBeEmpty);

            if (value is object obj && obj is null)
                throw new ArgumentNullException(nameof(value));

            Name = name;
            Value = value;
            IsInheritable = isInheritable;

        }

    }

    public abstract class PropertyBase<T> :
        PropertyBase,
        IProperty<T> {

        // Public members

        public new T Value => base.Value.As<T>();

        // Protected members

        protected PropertyBase(string name, T value, bool isInheritable) :
            base(name, PropertyValue.Create(value), isInheritable) {
        }

    }

}