using Gsemac.Forms.Styles.StyleSheets.Properties;
using Gsemac.Forms.Styles.StyleSheets.Selectors;
using System.Collections.Generic;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Rulesets {

    public interface IRuleset :
        ICollection<IProperty> {

        IProperty this[string propertyName] { get; }

        ISelector Selector { get; }

        Color BackgroundColor { get; }
        BackgroundImage BackgroundImage { get; }
        Borders Border { get; }
        Color BorderBottomColor { get; }
        ILengthOrPercentage BorderBottomLeftRadius { get; }
        ILengthOrPercentage BorderBottomRightRadius { get; }
        BorderStyle BorderBottomStyle { get; }
        Length BorderBottomWidth { get; }
        Color BorderLeftColor { get; }
        BorderStyle BorderLeftStyle { get; }
        Length BorderLeftWidth { get; }
        BorderRadius BorderRadius { get; }
        Color BorderRightColor { get; }
        BorderStyle BorderRightStyle { get; }
        Length BorderRightWidth { get; }
        Color BorderTopColor { get; }
        ILengthOrPercentage BorderTopLeftRadius { get; }
        ILengthOrPercentage BorderTopRightRadius { get; }
        BorderStyle BorderTopStyle { get; }
        Length BorderTopWidth { get; }
        Color Color { get; }
        double Opacity { get; }

        IProperty Get(string propertyName);
        bool Contains(string propertyName);
        bool Remove(string propertyName);

    }

}