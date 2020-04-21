using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Controls {

    public sealed class ControlRenderer :
        ControlRendererBase {

        // Public members

        public ControlRenderer(IStyleSheet styleSheet) :
            base(styleSheet) {
        }

        public override void RenderControl(Graphics graphics, Control control) {

            switch (control) {

                case Button button:

                    new ButtonControlRenderer(StyleSheet).RenderControl(graphics, button);

                    break;

                case CheckBox checkBox:

                    new CheckBoxControlRenderer(StyleSheet).RenderControl(graphics, checkBox);

                    break;

                case ComboBox comboBox:

                    new ComboBoxControlRenderer(StyleSheet).RenderControl(graphics, comboBox);

                    break;

                case Label label:

                    new LabelControlRenderer(StyleSheet).RenderControl(graphics, label);

                    break;

                case ListBox listBox:

                    new ListBoxControlRenderer(StyleSheet).RenderControl(graphics, listBox);

                    break;

                case NumericUpDown numericUpDown:

                    new NumericUpDownRenderer(StyleSheet).RenderControl(graphics, numericUpDown);

                    break;

                case RadioButton radioButton:

                    new RadioButtonControlRenderer(StyleSheet).RenderControl(graphics, radioButton);

                    break;

                case TabControl tabControl:

                    new TabControlRenderer(StyleSheet).RenderControl(graphics, tabControl);

                    break;

                case TextBox textBox:

                    new TextBoxControlRenderer(StyleSheet).RenderControl(graphics, textBox);

                    break;

                default:

                    // These controls can't be checked for directly because they are internal types.

                    switch (control.GetType().FullName) {

                        case "System.Windows.Forms.UpDownBase+UpDownButtons":

                            new UpDownButtonsControlRenderer(StyleSheet).RenderControl(graphics, control);

                            break;

                        default:

                            RenderGenericControl(graphics, control);

                            break;

                    }

                    break;

            }

        }

        // Private members

        private void RenderGenericControl(Graphics graphics, Control control) {

            PaintBackground(graphics, control);

        }

    }

}