using System;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public interface IPropertyDefinition {

        string Name { get; }

        Type ValueType { get; }
        IPropertyValue InitialValue { get; }

        bool IsShorthand { get; }
        bool Inherited { get; }

        IProperty Create(IPropertyValue[] arguments);

        IEnumerable<IPropertyDefinition> GetLonghands();

    }

}