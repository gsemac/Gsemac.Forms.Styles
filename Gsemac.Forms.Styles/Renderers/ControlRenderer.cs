using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public sealed class ControlRenderer :
        ControlRendererBase {

        // Public members

        public override void PaintControl(Control control, ControlPaintArgs e) {

            IControlRenderer renderer = GetControlRenderer(control);

            if (renderer is null)
                PaintGenericControl(e);
            else
                renderer.PaintControl(control, e);

        }

        // Private members

        private IControlRenderer GetControlRenderer(Control control) {

            switch (control) {

                case Button _:
                    return new ButtonControlRenderer();

                case CheckBox _:
                    return new CheckBoxControlRenderer();

                case ComboBox _:
                    return new ComboBoxControlRenderer();

                case Label _:
                    return new LabelControlRenderer();

                case ListBox _:
                    return new ListBoxControlRenderer();

                case NumericUpDown _:
                    return new NumericUpDownControlRenderer();

                case RadioButton _:
                    return new RadioButtonControlRenderer();

                case TabControl _:
                    return new TabControlControlRenderer();

                case TextBox _:
                    return new TextBoxControlRenderer();

                case GroupBox _:
                    return new GroupBoxControlRenderer();

                default:

                    // Some controls can't be checked for directly because they are internal types.

                    switch (control.GetType().FullName) {

                        case "System.Windows.Forms.UpDownBase+UpDownButtons":
                            return new UpDownButtonsControlRenderer();

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