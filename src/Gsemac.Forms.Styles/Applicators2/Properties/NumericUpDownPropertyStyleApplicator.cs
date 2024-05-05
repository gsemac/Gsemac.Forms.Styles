using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Linq;
using System.Windows.Forms;
using BorderStyle = Gsemac.Forms.Styles.StyleSheets.Properties.BorderStyle;

namespace Gsemac.Forms.Styles.Applicators2.Properties {

    internal sealed class NumericUpDownPropertyStyleApplicator :
         ControlPropertyStyleApplicator<NumericUpDown> {

        // Public members

        public override void ApplyStyle(NumericUpDown numericUpDown, IRuleset style) {

            if (numericUpDown is null)
                throw new ArgumentNullException(nameof(numericUpDown));

            if (style is null)
                throw new ArgumentNullException(nameof(style));

            base.ApplyStyle(numericUpDown, style);

            BorderStyle borderStyle = style.BorderStyle.Where(s => s != BorderStyle.None).LastOrDefault();
            double borderWidth = borderStyle == BorderStyle.None ? 0 : style.BorderWidth.Max(width => width.Value);

            if (borderWidth <= 0)
                numericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.None;
            else
                numericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

        }

    }

}