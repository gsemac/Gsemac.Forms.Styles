using Gsemac.Forms.Styles.StyleSheets;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Controls {

    public sealed class ControlRenderer :
        ControlRendererBase {

        // Public members

        public IStyleSheet StyleSheet { get; }

        public ControlRenderer(IStyleSheet styleSheet) :
            base(styleSheet) {

            StyleSheet = styleSheet;

        }

        public void RenderControl(Graphics graphics, Control control) {

            switch (control) {

                case TabControl tabControl:
                    new TabControlRenderer(StyleSheet).RenderControl(graphics, tabControl);
                    break;

                case Label label:
                    new LabelControlRenderer(StyleSheet).RenderControl(graphics, label);
                    break;

                case Button button:
                    new ButtonControlRenderer(StyleSheet).RenderControl(graphics, button);
                    break;

                default:
                    RenderGenericControl(graphics, control);
                    break;

            }

        }
        public bool HasStyles(Control control) {

            return GetRuleset(control).Any();

        }

        // Private members

        private void RenderGenericControl(Graphics graphics, Control control) {

            PaintBackground(graphics, control);

        }

    }

}