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
        Color BorderBottomColor { get; }
        Measurement BorderBottomLeftRadius { get; }
        Measurement BorderBottomRightRadius { get; }
        BorderStyle BorderBottomStyle { get; }
        Measurement BorderBottomWidth { get; }
        Color BorderLeftColor { get; }
        BorderStyle BorderLeftStyle { get; }
        Measurement BorderLeftWidth { get; }
        Color BorderRightColor { get; }
        BorderStyle BorderRightStyle { get; }
        Measurement BorderRightWidth { get; }
        Color BorderTopColor { get; }
        Measurement BorderTopLeftRadius { get; }
        Measurement BorderTopRightRadius { get; }
        BorderStyle BorderTopStyle { get; }
        Measurement BorderTopWidth { get; }
        Color Color { get; }
        double Opacity { get; }

        IProperty Get(string propertyName);
        bool Contains(string propertyName);
        bool Remove(string propertyName);

    }

}