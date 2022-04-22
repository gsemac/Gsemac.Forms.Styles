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
        IMeasurement BorderBottomLeftRadius { get; }
        IMeasurement BorderBottomRightRadius { get; }
        BorderStyle BorderBottomStyle { get; }
        IMeasurement BorderBottomWidth { get; }
        Color BorderLeftColor { get; }
        BorderStyle BorderLeftStyle { get; }
        IMeasurement BorderLeftWidth { get; }
        BorderRadius BorderRadius { get; }
        Color BorderRightColor { get; }
        BorderStyle BorderRightStyle { get; }
        IMeasurement BorderRightWidth { get; }
        Color BorderTopColor { get; }
        IMeasurement BorderTopLeftRadius { get; }
        IMeasurement BorderTopRightRadius { get; }
        BorderStyle BorderTopStyle { get; }
        IMeasurement BorderTopWidth { get; }
        Color Color { get; }
        double Opacity { get; }

        IProperty Get(string propertyName);
        bool Contains(string propertyName);
        bool Remove(string propertyName);

    }

}