using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public sealed class ControlRenderer :
        ControlRendererBase {

        // Public members

        public override void PaintControl(Control control, ControlPaintArgs e) {

            IControlRenderer renderer = GetRenderer(control);

            if (renderer is null)
                PaintGenericControl(e);
            else
                renderer.PaintControl(control, e);

        }

        // Private members

        private IControlRenderer GetRenderer(Control control) {

            switch (control) {

                case Button _:
                    return new ButtonRenderer();

                case CheckBox _:
                    return new CheckBoxRenderer();

                case ComboBox _:
                    return new ComboBoxControlRenderer();

                case Label _:
                    return new LabelRenderer();

                case ListBox _:
                    return new ListBoxRenderer();

                case NumericUpDown _:
                    return new NumericUpDownRenderer();

                case RadioButton _:
                    return new RadioButtonRenderer();

                case TabControl _:
                    return new TabControlRenderer();

                case TextBox _:
                    return new TextBoxRenderer();

                case GroupBox _:
                    return new GroupBoxRenderer();

                case ProgressBar _:
                    return new ProgressBarRenderer();

                default:

                    // Some controls can't be checked for directly because they are internal types.

                    switch (control.GetType().FullName) {

                        case "System.Windows.Forms.UpDownBase+UpDownButtons":
                            return new UpDownButtonsRenderer();

                        default:
                            return null;

                    }

            }

        }

        private void PaintGenericControl(ControlPaintArgs e) {

            e.PaintBackground();
            e.PaintBorder();

        }

    }

}