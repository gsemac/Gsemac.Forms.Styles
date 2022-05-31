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
        ILengthPercentage BorderBottomLeftRadius { get; }
        ILengthPercentage BorderBottomRightRadius { get; }
        BorderStyle BorderBottomStyle { get; }
        LineWidth BorderBottomWidth { get; }
        Color BorderLeftColor { get; }
        BorderStyle BorderLeftStyle { get; }
        LineWidth BorderLeftWidth { get; }
        BorderRadii BorderRadius { get; }
        Color BorderRightColor { get; }
        BorderStyle BorderRightStyle { get; }
        LineWidth BorderRightWidth { get; }
        Color BorderTopColor { get; }
        ILengthPercentage BorderTopLeftRadius { get; }
        ILengthPercentage BorderTopRightRadius { get; }
        BorderStyle BorderTopStyle { get; }
        LineWidth BorderTopWidth { get; }
        BorderWidths BorderWidth { get; }
        Color Color { get; }
        double Opacity { get; }

        IProperty Get(string propertyName);
        bool Contains(string propertyName);
        bool Remove(string propertyName);

    }

}