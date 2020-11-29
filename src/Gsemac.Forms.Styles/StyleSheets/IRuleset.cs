using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets {

    public interface IRuleset :
        IEnumerable<IProperty> {

        ISelector Selector { get; }

        ColorProperty BackgroundColor { get; }
        BackgroundImageProperty BackgroundImage { get; }
        ColorProperty BorderBottomColor { get; }
        NumberProperty BorderBottomLeftRadius { get; }
        NumberProperty BorderBottomRightRadius { get; }
        BorderStyleProperty BorderBottomStyle { get; }
        NumberProperty BorderBottomWidth { get; }
        ColorProperty BorderLeftColor { get; }
        BorderStyleProperty BorderLeftStyle { get; }
        NumberProperty BorderLeftWidth { get; }
        ColorProperty BorderRightColor { get; }
        BorderStyleProperty BorderRightStyle { get; }
        NumberProperty BorderRightWidth { get; }
        ColorProperty BorderTopColor { get; }
        NumberProperty BorderTopLeftRadius { get; }
        NumberProperty BorderTopRightRadius { get; }
        BorderStyleProperty BorderTopStyle { get; }
        NumberProperty BorderTopWidth { get; }
        ColorProperty Color { get; }
        NumberProperty Opacity { get; }

        void AddProperty(IProperty property);
        void AddProperties(IEnumerable<IProperty> properties);
        void InheritProperties(IEnumerable<IProperty> properties);
        IProperty GetProperty(PropertyType propertyType);
        bool HasProperty(PropertyType propertyType);

    }

}