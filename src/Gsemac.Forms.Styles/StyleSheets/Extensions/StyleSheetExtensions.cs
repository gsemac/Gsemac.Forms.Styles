using System.Windows.Forms;

namespace Gsemac.Forms.Styles.StyleSheets.Extensions {

    public static class StyleSheetExtensions {

        // Public members

        public static IRuleset GetRuleset(this IStyleSheet styleSheet, Control control) {

            return styleSheet.GetRuleset(control, true);

        }
        public static IRuleset GetRuleset(this IStyleSheet styleSheet, Control control, bool inherit) {

            return styleSheet.GetRuleset(new ControlNode(control), inherit);

        }
        public static IRuleset GetRuleset(this IStyleSheet styleSheet, INode node, Control parent) {

            return styleSheet.GetRuleset(node, styleSheet.GetRuleset(parent));

        }
        public static IRuleset GetRuleset(this IStyleSheet styleSheet, INode node, IRuleset parentRuleset) {

            IRuleset ruleset = new Ruleset();

            ruleset.InheritProperties(parentRuleset);
            ruleset.AddProperties(styleSheet.GetRuleset(node));

            return ruleset;

        }

    }

}