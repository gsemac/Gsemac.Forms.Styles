using Gsemac.Forms.Styles.Properties;
using Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public abstract class PropertyBase :
        IProperty {

        // Public members

        public string Name => definition.Name;
        public IPropertyValue Value { get; }

        public bool Inherited => definition.Inherited || Value.Equals(PropertyValue.Inherit);
        public bool IsShorthand => definition.IsShorthand;
        public bool IsVariable => PropertyUtilities.IsVariableName(Name);

        public Type ValueType => definition.ValueType;

        public virtual IEnumerable<IProperty> GetLonghands() {

            foreach (ILonghandPropertyDefinition longhand in definition.GetLonghands()) {

                yield return propertyFactory.Create(longhand.Name, longhand.ValueFactory(Value));

            }

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            sb.Append(Name);
            sb.Append(": ");

            object propertyValue = Value.Value;

            string propertyValueStr = StyleValueConverterFactory.Default
                .Create(propertyValue.GetType(), typeof(string))
                .Convert(propertyValue)
                .ToString();

            sb.Append(propertyValueStr);

            return sb.ToString();

        }

        // Protected members

        protected PropertyBase(string name, IPropertyValue value, bool inherited) {

            if (name is null)
                throw new ArgumentNullException(nameof(name));

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(ExceptionMessages.PropertyNameCannotBeEmpty, nameof(name));

            if (value is object obj && obj is null)
                throw new ArgumentNullException(nameof(value));

            definition = new PropertyDefinitionBuilder(name)
                .WithInherited(inherited)
                .WithType(value.Type)
                .Build();

            Value = value;

        }
        protected PropertyBase(IPropertyDefinition definition, IPropertyValue value, IPropertyFactory propertyFactory) {

            if (definition is null)
                throw new ArgumentNullException(nameof(definition));

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (propertyFactory is null)
                throw new ArgumentNullException(nameof(propertyFactory));

            this.definition = definition;
            Value = value;

        }

        // Private members

        private readonly IPropertyDefinition definition;
        private readonly IPropertyFactory propertyFactory = PropertyFactory.Default;

    }

}