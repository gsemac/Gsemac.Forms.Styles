using Gsemac.Forms.Styles.StyleSheets;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class ToolTipRenderer :
        IToolTipRenderer {

        // Public members

        public ToolTipRenderer(IStyleSheet styleSheet, IStyleRenderer styleRenderer) {

            this.styleSheet = styleSheet;
            this.styleRenderer = styleRenderer;

        }

        public void Draw(object sender, DrawToolTipEventArgs e) {

            INode node = new UserNode(string.Empty, new[] { "ToolTip" });
            IRuleset ruleset = styleSheet.GetRuleset(node);

            styleRenderer.PaintBackground(e.Graphics, e.Bounds, ruleset);
            styleRenderer.PaintText(e.Graphics, e.Bounds, ruleset, e.ToolTipText, e.Font, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            styleRenderer.PaintBorder(e.Graphics, e.Bounds, ruleset);

        }

        // Private members

        private readonly IStyleSheet styleSheet;
        private readonly IStyleRenderer styleRenderer;

    }

}