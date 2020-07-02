using Gsemac.Forms.Styles.StyleSheets;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators {

    public abstract class StyleSheetStyleApplicatorBase :
        StyleApplicatorBase {

        // Protected members

        protected IStyleSheet StyleSheet { get; }

        protected StyleSheetStyleApplicatorBase(IStyleSheet styleSheet) {

            this.StyleSheet = styleSheet;

        }

        protected override bool HasStyles(Control control) {
           
            return StyleSheet.GetRuleset(new ControlNode(control), false).Any();

        }

    }

}