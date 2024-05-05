using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System.Linq;
using System.Windows.Forms;
using BorderStyle = Gsemac.Forms.Styles.StyleSheets.Properties.BorderStyle;

namespace Gsemac.Forms.Styles.Applicators2.Properties {

    internal sealed class ListBoxPropertyStyleApplicator :
         ControlPropertyStyleApplicator<ListBox> {

        // Public members

        public override void ApplyStyle(ListBox listBox, IRuleset style) {

            base.ApplyStyle(listBox, style);

            BorderStyle borderStyle = style.BorderStyle.Where(s => s != BorderStyle.None).LastOrDefault();
            double borderWidth = borderStyle == BorderStyle.None ? 0 : style.BorderWidth.Max(width => width.Value);

            if (borderWidth <= 0)
                listBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            else
                listBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

        }

    }

}