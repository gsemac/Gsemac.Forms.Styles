using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class ControlPaintArgs {

        // Public members

        public const TextFormatFlags DefaultTextFormatFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter;

        public Control Control { get; }
        public Graphics Graphics { get; }
        public IStyleSheet StyleSheet { get; }
        public IStyleRenderer StyleRenderer { get; }
        public bool ParentDraw { get; }

        public ControlPaintArgs(Control control, Graphics graphics, IStyleSheet styleSheet, IStyleRenderer styleRenderer, bool parentDraw) {

            this.Control = control;
            this.Graphics = graphics;
            this.StyleSheet = styleSheet;
            this.StyleRenderer = styleRenderer;
            this.ParentDraw = parentDraw;

        }

        public void Clear() {

            using (Brush brush = new SolidBrush(GetClearColor(Control)))
                Graphics.FillRectangle(brush, Control.ClientRectangle);

        }

        public void PaintBackground() {

            Clear();

            PaintBackground(Control.ClientRectangle);

        }
        public void PaintBackground(Rectangle drawRect) {

            StyleRenderer.PaintBackground(Graphics, drawRect, StyleSheet.GetRuleset(Control));

        }
        public void PaintText(TextFormatFlags textFormatFlags = DefaultTextFormatFlags) {

            PaintText(Control.ClientRectangle, textFormatFlags);

        }
        public void PaintText(Rectangle textRect, TextFormatFlags textFormatFlags = DefaultTextFormatFlags) {

            StyleRenderer.PaintText(Graphics, textRect, StyleSheet.GetRuleset(Control), Control.Text, Control.Font, textFormatFlags);

        }
        public void PaintBorder() {

            PaintBorder(Control.ClientRectangle);

        }
        public void PaintBorder(Rectangle drawRect) {

            StyleRenderer.PaintBorder(Graphics, drawRect, StyleSheet.GetRuleset(Control));

        }

        public void PaintControl() {

            PaintBackground();
            PaintText();
            PaintBorder();

        }

        public void ClipToBorder() {

            ClipToBorder(Control.ClientRectangle);

        }
        public void ClipToBorder(Rectangle clippingRect) {

            IRuleset ruleset = StyleSheet.GetRuleset(Control);

            StyleRenderer.ClipToBorder(Graphics, clippingRect, ruleset);

        }

        // Private members

        private Color GetClearColor(Control control) {

            Color clearColor = Color.Transparent;

            if (control != null && control.Parent != null) {

                ColorProperty parentBackgroundColor = StyleSheet.GetRuleset(control.Parent).BackgroundColor;

                if (parentBackgroundColor.HasValue())
                    clearColor = parentBackgroundColor.Value;
                else
                    clearColor = control.Parent.BackColor;

                if (clearColor == Color.Transparent)
                    clearColor = GetClearColor(control.Parent);

            }

            return clearColor;

        }

    }

}