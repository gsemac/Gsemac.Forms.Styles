using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public interface IProperty {

        string Name { get; }
        PropertyType Type { get; }
        object Value { get; }
        bool IsInheritable { get; }

    }

    public interface IProperty<T> :
        IProperty {

        new T Value { get; }

    }

}