using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2 {

    internal class ToolStripStyleApplicator :
        ControlStyleApplicatorBase<ToolStrip> {

        // Public members

        public override void InitializeStyle(ToolStrip control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            control.Renderer = new Renderers2.ToolStripRenderer();

        }
        public override void ApplyStyle(ToolStrip control, IRuleset ruleset) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (ruleset is null)
                throw new ArgumentNullException(nameof(ruleset));

            if (control.Renderer is Renderers2.ToolStripRenderer renderer)
                renderer.Ruleset = ruleset;

        }

    }

}