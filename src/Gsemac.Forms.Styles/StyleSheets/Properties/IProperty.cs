using System;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public interface IProperty {

        string Name { get; }
        Type ValueType { get; }
        IPropertyValue Value { get; }

        bool Inherited { get; }
        bool IsShorthand { get; }
        bool IsVariable { get; }

        IEnumerable<IProperty> GetLonghands();

    }

    public interface IProperty<T> :
        IProperty {

        new T Value { get; }

    }

}