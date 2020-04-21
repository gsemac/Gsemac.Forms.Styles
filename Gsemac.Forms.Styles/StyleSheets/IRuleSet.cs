using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public interface IRuleset :
        IEnumerable<IProperty> {

        ISelector Selector { get; }

        ColorProperty BackgroundColor { get; }
        ColorProperty BorderColor { get; }
        NumberProperty BorderWidth { get; }
        BorderRadiusProperty BorderRadius { get; }
        ColorProperty Color { get; }

        void AddProperty(IProperty property);
        void AddProperties(IEnumerable<IProperty> properties);
        void InheritProperties(IEnumerable<IProperty> properties);
        IProperty GetProperty(PropertyType propertyType);
        bool HasProperty(PropertyType propertyType);

    }

}