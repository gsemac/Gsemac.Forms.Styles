using Gsemac.Forms.Styles.StyleSheets.Properties;
using Gsemac.Forms.Styles.StyleSheets.Selectors;
using System.Collections.Generic;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Rulesets {

    public interface IRuleset :
        ICollection<IProperty> {

        IProperty this[string propertyName] { get; set; }

        StyleOrigin Origin { get; }
        ISelector Selector { get; }

        Color AccentColor { get; }
        Color BackgroundColor { get; }
        BackgroundImage BackgroundImage { get; }
        Borders Border { get; }
        Color BorderBottomColor { get; }
        ILengthPercentage BorderBottomLeftRadius { get; }
        ILengthPercentage BorderBottomRightRadius { get; }
        BorderStyle BorderBottomStyle { get; }
        LineWidth BorderBottomWidth { get; }
        BorderColors BorderColor { get; }
        Color BorderLeftColor { get; }
        BorderStyle BorderLeftStyle { get; }
        LineWidth BorderLeftWidth { get; }
        BorderRadii BorderRadius { get; }
        Color BorderRightColor { get; }
        BorderStyle BorderRightStyle { get; }
        LineWidth BorderRightWidth { get; }
        BorderStyles BorderStyle { get; }
        Color BorderTopColor { get; }
        ILengthPercentage BorderTopLeftRadius { get; }
        ILengthPercentage BorderTopRightRadius { get; }
        BorderStyle BorderTopStyle { get; }
        LineWidth BorderTopWidth { get; }
        BorderWidths BorderWidth { get; }
        Color Color { get; }
        double Opacity { get; }

        bool ContainsKey(string propertyName);
        bool Remove(string propertyName);
        bool TryGetValue(string propertyName, out IProperty value);

    }

}