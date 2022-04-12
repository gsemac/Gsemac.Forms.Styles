using Gsemac.Forms.Styles.Dom;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.StyleSheets.Extensions {

    public static class StyleSheetExtensions {

        // Public members

        public static IRuleset GetRuleset(this IStyleSheet styleSheet, Control control) {

            return styleSheet.GetRuleset(control, true);

        }
        public static IRuleset GetRuleset(this IStyleSheet styleSheet, Control control, bool inherit) {

            return styleSheet.GetRuleset(new ControlNode2(control));

        }
        public static IRuleset GetRuleset(this IStyleSheet styleSheet, INode2 node, Control parent) {

            return styleSheet.GetRuleset(node, styleSheet.GetRuleset(parent));

        }
        public static IRuleset GetRuleset(this IStyleSheet styleSheet, INode2 node) {

            // TODO: Get rid of this

            return styleSheet.GetRulesets(node).First();

        }
        public static IRuleset GetRuleset(this IStyleSheet styleSheet, INode2 node, bool inherit) {

            // TODO: Get rid of this

            return styleSheet.GetRuleset(node);

        }
        public static IRuleset GetRuleset(this IStyleSheet styleSheet, INode2 node, IRuleset parentRuleset) {

            IRuleset ruleset = new Ruleset();

            ruleset.InheritProperties(parentRuleset);
            ruleset.AddProperties(styleSheet.GetRuleset(node));

            return ruleset;

        }

    }

}