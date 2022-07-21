using System;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public interface IPropertyDefinition {

        string Name { get; }
        Type ValueType { get; }
        IPropertyValue InitialValue { get; }

        bool Inherited { get; }
        bool IsShorthand { get; }

        IEnumerable<ILonghandPropertyDefinition> GetLonghands();

    }

}