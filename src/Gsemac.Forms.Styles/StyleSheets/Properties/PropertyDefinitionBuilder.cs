using Gsemac.Forms.Styles.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class PropertyDefinitionBuilder :
        IPropertyDefinitionBuilder {

        // Public members

        public PropertyDefinitionBuilder() { }
        public PropertyDefinitionBuilder(string name) {

            WithName(name);

        }

        public IPropertyDefinitionBuilder WithName(string value) {

            value = FormatPropertyName(value);

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(ExceptionMessages.PropertyNameCannotBeEmpty, nameof(value));

            definition.Name = value;

            return this;

        }
        public IPropertyDefinitionBuilder WithInherited(bool value) {

            definition.Inherited = value;

            return this;

        }
        public IPropertyDefinitionBuilder WithType(Type value) {

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            definition.ValueType = value;

            setValueType = true;

            return this;

        }
        public IPropertyDefinitionBuilder WithInitial(IPropertyValue value) {

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            definition.InitialValue = value;

            if (!setValueType)
                WithType(value.Type);

            return this;

        }
        public IPropertyDefinitionBuilder WithLonghand(string name, LonghandPropertyValueFactory longhandValueFactory) {

            if (longhandValueFactory is null)
                throw new ArgumentNullException(nameof(longhandValueFactory));

            // We will return a new builder for building the longhand property.

            PropertyDefinitionBuilder longhandBuilder = new PropertyDefinitionBuilder(this);

            longhandBuilder.WithName(name);

            longhands.Add(new LonghandPropertyBuilderInfo(longhandBuilder, longhandValueFactory));

            return longhandBuilder;

        }
        public IPropertyDefinitionBuilder EndProperty() {

            if (parentBuilder is object)
                return parentBuilder;

            return this;

        }

        public IPropertyDefinition Build() {

            if (parentBuilder is object)
                return parentBuilder.Build();

            return Build(buildParent: false);

        }

        // Private members

        private class LonghandPropertyBuilderInfo {

            // Public members

            public PropertyDefinitionBuilder Builder { get; }
            public LonghandPropertyValueFactory ValueFactory { get; }

            public LonghandPropertyBuilderInfo(PropertyDefinitionBuilder builder, LonghandPropertyValueFactory valueFactory) {

                Builder = builder;
                ValueFactory = valueFactory;

            }

        }

        private class PropertyDefinition :
            IPropertyDefinition {

            // Public members

            public string Name {
                get => name;
                set => name = FormatPropertyName(value);
            }
            public Type ValueType { get; set; } = typeof(object);
            public IPropertyValue InitialValue { get; set; } = PropertyValue.Initial;

            public bool Inherited { get; set; } = false;
            public bool IsShorthand => longhands.Any();
            public bool IsVariable => PropertyUtilities.IsVariableName(Name);

            public IEnumerable<ILonghandPropertyDefinition> Longhands => longhands.ToArray();

            public PropertyDefinition() { }
            public PropertyDefinition(IPropertyDefinition definition) {

                if (definition is null)
                    throw new ArgumentNullException(nameof(definition));

                Name = definition.Name;
                ValueType = definition.ValueType;
                InitialValue = definition.InitialValue;
                Inherited = definition.Inherited;

                foreach (ILonghandPropertyDefinition longhand in definition.Longhands)
                    longhands.Add(longhand);

            }

            public void AddLonghand(IPropertyDefinition definition, LonghandPropertyValueFactory valueFactory) {

                if (definition is null)
                    throw new ArgumentNullException(nameof(definition));

                if (valueFactory is null)
                    throw new ArgumentNullException(nameof(valueFactory));

                longhands.Add(new LonghandPropertyDefinition(definition, valueFactory));

            }
            public void AddLonghand(ILonghandPropertyDefinition definition) {

                if (definition is null)
                    throw new ArgumentNullException(nameof(definition));

                longhands.Add(definition);

            }

            // Private members

            private class LonghandPropertyDefinition :
                PropertyDefinition,
                ILonghandPropertyDefinition {

                // Public members

                public LonghandPropertyValueFactory ValueFactory { get; }

                public LonghandPropertyDefinition(IPropertyDefinition definition, LonghandPropertyValueFactory valueFactory) :
                    base(definition) {

                    if (definition is null)
                        throw new ArgumentNullException(nameof(definition));

                    if (valueFactory is null)
                        throw new ArgumentNullException(nameof(valueFactory));

                    ValueFactory = valueFactory;

                }
            }

            private readonly IList<ILonghandPropertyDefinition> longhands = new List<ILonghandPropertyDefinition>();
            private string name = string.Empty;



        }

        private PropertyDefinitionBuilder(IPropertyDefinitionBuilder parentBuilder) {

            this.parentBuilder = parentBuilder;

        }

        private IPropertyDefinition Build(bool buildParent) {

            if (buildParent)
                return Build();

            foreach (LonghandPropertyBuilderInfo longhand in longhands)
                definition.AddLonghand(longhand.Builder.Build(buildParent: false), longhand.ValueFactory);

            longhands.Clear();

            return definition;

        }

        private readonly IPropertyDefinitionBuilder parentBuilder;
        private readonly PropertyDefinition definition = new PropertyDefinition();
        private readonly List<LonghandPropertyBuilderInfo> longhands = new List<LonghandPropertyBuilderInfo>();
        private bool setValueType = false;

        private static string FormatPropertyName(string value) {

            return (value ?? string.Empty)?.Trim();

        }

    }

}