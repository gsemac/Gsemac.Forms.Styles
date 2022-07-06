using Gsemac.Forms.Styles.Renderers2;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2 {

    public abstract class ControlStyleApplicatorBase<T> :
        StyleApplicatorBase<T> where T : Control {

        // Public members

        public override void InitializeStyle(T obj) {

            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            Control control = obj;

            ControlUtilities.SetDoubleBuffered(control, true);

        }

        public override void ApplyStyle(T obj, IRuleset ruleset) {

            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (ruleset is null)
                throw new ArgumentNullException(nameof(ruleset));

            Control control = obj;

            ControlRenderUtilities.ApplyColorProperties(control, ruleset);

        }

    }

}