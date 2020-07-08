using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
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

        public override void InitializeControl(Control control) {

            IControlRenderer renderer = GetRenderer(control);

            if (renderer != null)
                renderer.InitializeControl(control);

        }

        public void SetRenderer<T>(IControlRenderer<T> renderer) where T : Control {

            customRenderers[typeof(T)] = renderer;

        }

        // Private members

        private readonly IDictionary<Type, IControlRenderer> customRenderers = new Dictionary<Type, IControlRenderer>();

        private IControlRenderer GetRenderer(Control control) {

            if (customRenderers.TryGetValue(control.GetType(), out IControlRenderer customRenderer))
                return customRenderer;

            switch (control) {

                case Button _:
                    return new ButtonRenderer();

                case CheckBox _:
                    return new CheckBoxRenderer();

                case ComboBox _:
                    return new ComboBoxRenderer();

                case DataGridView _:
                    return new DataGridViewRenderer(null, null);

                case GroupBox _:
                    return new GroupBoxRenderer();

                case Label _:
                    return new LabelRenderer();

                case ListBox _:
                    return new ListBoxRenderer();

                case ListView _:
                    return new ListViewRenderer(null, null);

                case PictureBox _:
                    return new PictureBoxRenderer();

                case ProgressBar _:
                    return new ProgressBarRenderer();

                case NumericUpDown _:
                    return new NumericUpDownRenderer();

                case RadioButton _:
                    return new RadioButtonRenderer();

                case RichTextBox _:
                    return new RichTextBoxRenderer();

                case TabControl _:
                    return new TabControlRenderer();

                case TextBox _:
                    return new TextBoxRenderer();

                case TreeView _:
                    return new TreeViewRenderer();

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

            IRuleset ruleset = e.StyleSheet.GetRuleset(new ControlNode(e.Control));

            RenderUtilities.ApplyColorProperties(e.Control, ruleset);

            e.PaintBackground();
            e.PaintBorder();

        }

    }

}