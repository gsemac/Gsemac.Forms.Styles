using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    internal sealed class ToolTipRenderer :
        IToolTipRenderer {

        // Public members

        public IRuleset Ruleset { get; set; }

        public void Draw(object sender, DrawToolTipEventArgs e) {

            if (sender is null)
                throw new ArgumentNullException(nameof(sender));

            if (e is null)
                throw new ArgumentNullException(nameof(e));

            IRenderContext context = new RenderContext(e.Graphics, e.Bounds, Ruleset);

            context.DrawBackground();
            context.DrawText(e.ToolTipText, e.Font, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            context.DrawBorder();

        }

    }

}