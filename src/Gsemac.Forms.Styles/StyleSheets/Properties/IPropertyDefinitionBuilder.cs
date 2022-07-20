using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public delegate IPropertyValue LonghandPropertyValueFactory(IPropertyValue propertyValue);

    public interface IPropertyDefinitionBuilder {

        IPropertyDefinitionBuilder WithName(string value);
        IPropertyDefinitionBuilder WithInherited(bool value);
        IPropertyDefinitionBuilder WithType(Type value);
        IPropertyDefinitionBuilder WithInitial(IPropertyValue value);

        IPropertyDefinitionBuilder WithLonghand(string name, LonghandPropertyValueFactory longhandValueFactory);
        IPropertyDefinitionBuilder EndProperty();

        IPropertyDefinition Build();

    }

}