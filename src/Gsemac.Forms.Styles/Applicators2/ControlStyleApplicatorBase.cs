using Gsemac.Forms.Styles.Renderers2;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2 {

    internal abstract class ControlStyleApplicatorBase<T> :
        StyleApplicatorBase<T> where T : Control {

        // Public members

        public override void InitializeStyle(T control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (IsSupportedControl(control)) {

                ControlUtilities.SetDoubleBuffered(control, true);

            }

        }

        public override void ApplyStyle(T control, IRuleset ruleset) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (ruleset is null)
                throw new ArgumentNullException(nameof(ruleset));

            if (IsSupportedControl(control)) {

                RenderUtilities.ApplyColorProperties(control, ruleset);

            }

        }

        // Private members

        private static bool IsSupportedControl(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            // Do not apply styles directly to NumericUpDown's TextBox control.
            // Instead, its properties will be set via the NumericUpDown.

            if (control is TextBox && control.Parent is NumericUpDown)
                return false;

            return true;

        }

    }

}