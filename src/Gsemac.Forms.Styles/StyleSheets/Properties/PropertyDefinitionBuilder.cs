using System;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class PropertyDefinitionBuilder :
        IPropertyDefinitionBuilder {

        // Public members

        public PropertyDefinitionBuilder() { }
        public PropertyDefinitionBuilder(string name) {

            WithName(name);

        }

        public IPropertyDefinitionBuilder WithName(string value) {

            value = (value ?? string.Empty).Trim();

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

            longhands.Add(new LonghandPropertyInfo(longhandBuilder, longhandValueFactory));

            return longhandBuilder;

        }
        public IPropertyDefinitionBuilder End() {

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

        private class LonghandPropertyInfo {

            // Public members

            public PropertyDefinitionBuilder Builder { get; }
            public LonghandPropertyValueFactory ValueFactory { get; }

            public LonghandPropertyInfo(PropertyDefinitionBuilder builder, LonghandPropertyValueFactory valueFactory) {

                Builder = builder;
                ValueFactory = valueFactory;

            }

        }

        private PropertyDefinitionBuilder(IPropertyDefinitionBuilder parentBuilder) {

            this.parentBuilder = parentBuilder;

        }

        private IPropertyDefinition Build(bool buildParent) {

            if (buildParent)
                return Build();

            foreach (LonghandPropertyInfo longhand in longhands)
                definition.AddLonghand(longhand.Builder.Build(buildParent: false), longhand.ValueFactory);

            longhands.Clear();

            return definition;

        }

        private readonly IPropertyDefinitionBuilder parentBuilder;
        private readonly PropertyDefinition definition = new PropertyDefinition();
        private readonly List<LonghandPropertyInfo> longhands = new List<LonghandPropertyInfo>();
        private bool setValueType = false;

    }

}