using Gsemac.Forms.Styles.Renderers2;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2.Properties {

    internal sealed class ToolTipPropertyStyleApplicator :
        StyleApplicatorBase<ToolTip> {

        // Public members

        public override void InitializeStyle(ToolTip toolTip) {

            if (toolTip is null)
                throw new ArgumentNullException(nameof(toolTip));

            toolTip.OwnerDraw = true;

            toolTip.Draw += renderer.Draw;

        }
        public override void DeinitializeStyle(ToolTip toolTip) {

            if (toolTip is null)
                throw new ArgumentNullException(nameof(toolTip));

            toolTip.OwnerDraw = false;

            toolTip.Draw -= renderer.Draw;

        }
        public override void ApplyStyle(ToolTip toolTip, IRuleset ruleset) {

            if (toolTip is null)
                throw new ArgumentNullException(nameof(toolTip));

            if (ruleset is null)
                throw new ArgumentNullException(nameof(ruleset));

            renderer.Ruleset = ruleset;

        }

        // Private members

        private readonly ToolTipRenderer renderer = new ToolTipRenderer();

    }

}