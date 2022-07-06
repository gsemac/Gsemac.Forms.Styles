using System;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public interface IProperty {

        string Name { get; }
        IPropertyValue Value { get; }

        bool IsInheritable { get; }
        bool IsShorthand { get; }
        bool IsVariable { get; }

        Type ValueType { get; }

        IEnumerable<IProperty> GetLonghandProperties(IPropertyFactory propertyFactory);

    }

    public interface IProperty<T> :
        IProperty {

        new T Value { get; }

    }

}