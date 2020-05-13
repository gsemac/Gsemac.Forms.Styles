using Gsemac.Forms.Styles.Renderers;
using Gsemac.Forms.Styles.StyleSheets;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Extensions {

    public static class StyleSheetControlRendererExtensions {

        public const TextFormatFlags DefaultTextFormatFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter;

        public static IRuleset GetRuleset(this IStyleSheetControlRenderer renderer, Control control) {

            return renderer.GetRuleset(control, true);

        }
        public static IRuleset GetRuleset(this IStyleSheetControlRenderer renderer, INode node) {

            return renderer.GetRuleset(node, true);

        }
        public static IRuleset GetRuleset(this IStyleSheetControlRenderer renderer, Control parentControl, INode node) {

            return renderer.GetRuleset(renderer.GetRuleset(parentControl), node);

        }
        public static IRuleset GetRuleset(this IStyleSheetControlRenderer renderer, IRuleset parentRuleset, INode node) {

            IRuleset ruleset = new Ruleset();

            ruleset.InheritProperties(parentRuleset);
            ruleset.AddProperties(renderer.GetRuleset(node));

            return ruleset;

        }

        public static void ClearBackground(this IStyleSheetControlRenderer renderer, Graphics graphics, Control control) {

            ColorProperty parentBackgroundColor = null;

            if (control.Parent != null)
                parentBackgroundColor = renderer.GetRuleset(control.Parent).BackgroundColor;

            graphics.Clear(parentBackgroundColor?.Value ?? control.Parent?.BackColor ?? Color.Transparent);

        }

        public static void PaintBackground(this IStyleSheetControlRenderer renderer, Graphics graphics, Control control) {

            renderer.PaintBackground(graphics, control, renderer.GetRuleset(control, true));

        }
        public static void PaintBackground(this IStyleSheetControlRenderer renderer, Graphics graphics, Control control, IRuleset ruleset) {

            renderer.ClearBackground(graphics, control);

            renderer.PaintBackground(graphics, control.ClientRectangle, ruleset);

        }

        public static void PaintForeground(this IStyleSheetControlRenderer renderer, Graphics graphics, Control control) {

            renderer.PaintForeground(graphics, control, control.ClientRectangle);

        }
        public static void PaintForeground(this IStyleSheetControlRenderer renderer, Graphics graphics, Control control, Rectangle rectangle) {

            renderer.PaintForeground(graphics, control, rectangle, DefaultTextFormatFlags);

        }
        public static void PaintForeground(this IStyleSheetControlRenderer renderer, Graphics graphics, Control control, TextFormatFlags textFormatFlags) {

            IRuleset ruleset = renderer.GetRuleset(control);

            // Paint the foreground text.

            renderer.PaintForeground(graphics, control.Text, control.Font, control.ClientRectangle, ruleset, textFormatFlags);

        }
        public static void PaintForeground(this IStyleSheetControlRenderer renderer, Graphics graphics, Control control, Rectangle rectangle, TextFormatFlags textFormatFlags) {

            renderer.PaintForeground(graphics, control.Text, control.Font, rectangle, renderer.GetRuleset(control), textFormatFlags);

        }
        public static void PaintForeground(this IStyleSheetControlRenderer renderer, Graphics graphics, string text, Font font, Rectangle rectangle, IRuleset ruleset) {

            renderer.PaintForeground(graphics, text, font, rectangle, ruleset, DefaultTextFormatFlags);

        }

    }

}