using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public interface IProperty {

        string Name { get; }
        Type ValueType { get; }
        IPropertyValue Value { get; }

        bool Inherited { get; }
        bool IsShorthand { get; }
        bool IsVariable { get; }

        IPropertyDefinition Definition { get; }

    }

    public interface IProperty<T> :
        IProperty {

        new T Value { get; }

    }

}