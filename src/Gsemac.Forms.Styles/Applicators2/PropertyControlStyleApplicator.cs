using Gsemac.Forms.Styles.Renderers2;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2 {

    public class PropertyControlStyleApplicator<T> :
        ControlStyleApplicatorBase<T> where T : Control {

        // Public members

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