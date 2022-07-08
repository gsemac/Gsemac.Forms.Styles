using System;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public interface IProperty {

        string Name { get; }
        IPropertyValue Value { get; }
        Type ValueType { get; }

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