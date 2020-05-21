using Gsemac.Forms.Styles.Renderers;
using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators {

    public class UserPaintStyleApplicator :
        StyleSheetStyleApplicatorBase {

        // Public members

        public UserPaintStyleApplicator(IStyleSheet styleSheet) :
            base(styleSheet) {

            controlRenderer = new ControlRenderer();

        }

        // Protected members

        protected override void OnApplyStyles(Control control) {

            ControlInfo info = GetControlInfo(control);

            if (ControlSupportsUserPaint(control)) {

                SetStyle(control, ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

                control.Paint += PaintEventHandler;

                info.ResetControl += (c) => {
                    control.Paint -= PaintEventHandler;
                };

            }

            switch (control) {

                case ListBox listBox:

                    ApplyStyles(listBox, info);

                    break;

                case NumericUpDown numericUpDown:

                    ApplyStyles(numericUpDown, info);

                    break;

                case Panel panel:

                    ApplyStyles(panel, info);

                    break;

                case TextBox textBox:

                    ApplyStyles(textBox, info);

                    break;

                case ToolStrip toolStrip:

                    ApplyStyles(toolStrip, info);

                    break;

            }

        }

        // Private members

        private readonly ControlRenderer controlRenderer;
        private readonly IStyleRenderer styleRenderer = new StyleRenderer();

        private bool ControlSupportsUserPaint(Control control) {

            // Controls that use TextBoxes generally need special treatment to render correctly.
            // ToolStrips and MenuStrips (which inherit from ToolStrip) are drawn through the custom ToolStripRenderers supplied to the "Renderer" property.

            if (control is TextBox || control is NumericUpDown || control is ToolStrip)
                return false;

            // Only ComboBoxes with the DropDownList style do not use a TextBox, and can be fully painted.

            if (control is ComboBox comboBox && comboBox.DropDownStyle != ComboBoxStyle.DropDownList)
                return false;

            return true;

        }

        private void PaintEventHandler(object sender, PaintEventArgs e) {

            if (sender is Control control)
                controlRenderer.PaintControl(control, CreateControlPaintArgs(control, e));

        }
        private void InvalidateHandler(object sender, EventArgs e) {

            if (sender is Control control)
                control.Invalidate();

        }
        private void InvalidateParentHandler(object sender, EventArgs e) {

            if (sender is Control control && control.Parent != null)
                control.Parent.Invalidate();

        }

        private void AddParentPaintHandler(Control control, ControlInfo info) {

            if (control.Parent != null) {

                ControlInfo parentControlInfo = GetControlInfo(control.Parent);

                if (parentControlInfo != null) {

                    // Add event handlers to the parent control.

                    void paintHandler(object sender, PaintEventArgs e) {

                        e.Graphics.TranslateTransform(control.Location.X, control.Location.Y);

                        controlRenderer.PaintControl(control, CreateControlPaintArgs(control, e));

                        e.Graphics.TranslateTransform(-control.Location.X, -control.Location.Y);

                    }

                    control.Parent.Paint += paintHandler;

                    info.ResetControl += (c) => {
                        control.Parent.Paint -= paintHandler;
                    };

                }

            }

        }
        private void AddTextBoxInvalidateParentHandlers(Control control, ControlInfo info) {

            control.MouseMove += InvalidateParentHandler; // required for :hover
            control.MouseEnter += InvalidateParentHandler; // required for :hover
            control.MouseLeave += InvalidateParentHandler; // required for :hover
            control.MouseDown += InvalidateParentHandler; // required for :active
            control.GotFocus += InvalidateParentHandler; // required for :focus
            control.LostFocus += InvalidateParentHandler; // required for :focus

            info.ResetControl += (c) => {

                control.MouseMove -= InvalidateParentHandler;
                control.MouseEnter -= InvalidateParentHandler;
                control.MouseLeave -= InvalidateParentHandler;
                control.MouseDown -= InvalidateParentHandler;
                control.GotFocus -= InvalidateParentHandler;
                control.LostFocus -= InvalidateParentHandler;

            };

        }

        private void ApplyStyles(ListBox control, ControlInfo info) {

            control.DrawMode = DrawMode.OwnerDrawFixed;
            control.BorderStyle = System.Windows.Forms.BorderStyle.None;

            control.MouseMove += InvalidateHandler; // required for :hover
            control.MouseEnter += InvalidateHandler; // required for :hover
            control.MouseLeave += InvalidateHandler; // required for :hover
            control.SelectedIndexChanged += InvalidateHandler; // required for item selection
            control.MouseDown += InvalidateHandler; // required for item selection

            info.ResetControl += (c) => {

                control.MouseMove -= InvalidateHandler;
                control.MouseEnter -= InvalidateHandler;
                control.MouseLeave -= InvalidateHandler;
                control.SelectedIndexChanged -= InvalidateHandler;
                control.MouseDown -= InvalidateHandler;


            };

        }
        private void ApplyStyles(NumericUpDown control, ControlInfo info) {

            AddParentPaintHandler(control, info);
            AddTextBoxInvalidateParentHandlers(control, info);

            Control upDownButtonsControl = control.Controls.Cast<Control>()
                .Where(c => c.GetType().FullName.Equals("System.Windows.Forms.UpDownBase+UpDownButtons"))
                .FirstOrDefault();

            if (upDownButtonsControl != null) {

                void invalidateUpDownButtons(object sender, EventArgs e) {
                    upDownButtonsControl.Invalidate();
                }

                control.MouseMove += invalidateUpDownButtons;
                control.MouseEnter += invalidateUpDownButtons;
                control.MouseLeave += invalidateUpDownButtons;
                control.MouseDown += invalidateUpDownButtons;
                control.GotFocus += invalidateUpDownButtons;
                control.LostFocus += invalidateUpDownButtons;

                info.ResetControl += (c) => {

                    control.MouseMove -= invalidateUpDownButtons;
                    control.MouseEnter -= invalidateUpDownButtons;
                    control.MouseLeave -= invalidateUpDownButtons;
                    control.MouseDown -= invalidateUpDownButtons;
                    control.GotFocus -= invalidateUpDownButtons;
                    control.LostFocus -= invalidateUpDownButtons;

                };

            }

            info.Location = control.Location;
            info.Width = control.Width;
            info.Height = control.Height;

            control.BorderStyle = System.Windows.Forms.BorderStyle.None;

            control.Location = new Point(control.Location.X + 2, control.Location.Y + 2);
            control.Width -= 3;

        }
        private void ApplyStyles(Panel control, ControlInfo info) {

            // ResizeRedraw needs to be set to true to prevent smearing.
            // https://stackoverflow.com/a/39419274/5383169

            TryGetResizeRedraw(control, out bool oldResetRedraw);

            TrySetResizeRedraw(control, true);

            info.ResetControl += (c) => {
                TrySetResizeRedraw(control, oldResetRedraw);
            };

        }
        private void ApplyStyles(TextBox control, ControlInfo info) {

            // We need the TextBox to have a parent control so we can draw the TextBox in the parent's OnPaint.

            AddParentPaintHandler(control, info);
            AddTextBoxInvalidateParentHandlers(control, info);

            // Set up properties for the TextBox.
            // Borderless TextBoxes don't have the same offset/size as regular TextBoxes, so we need to adjust it.

            info.Location = control.Location;
            info.Width = control.Width;
            info.Height = control.Height;

            control.BorderStyle = System.Windows.Forms.BorderStyle.None;

            control.Location = new Point(control.Location.X + 3, control.Location.Y + 4);
            control.Width -= 6;

            if (control.Multiline)
                control.Height -= 6;

        }
        private void ApplyStyles(ToolStrip control, ControlInfo info) {

            System.Windows.Forms.ToolStripRenderer originalRenderer = control.Renderer;

            control.Renderer = new Renderers.ToolStripRenderer(StyleSheet, styleRenderer);

            info.ResetControl += (c) => {

                (c as ToolStrip).Renderer = originalRenderer;

            };

        }

        private bool TryGetResizeRedraw(Control control, out bool value) {

            PropertyInfo drawModeProperty = control.GetType().GetProperty("ResizeRedraw", BindingFlags.NonPublic | BindingFlags.Instance);

            if (drawModeProperty != null) {

                value = (bool)drawModeProperty.GetValue(control, null);

                return true;

            }
            else {

                value = false;

                return false;

            }

        }
        private bool TrySetResizeRedraw(Control control, bool value) {

            PropertyInfo drawModeProperty = control.GetType().GetProperty("ResizeRedraw", BindingFlags.NonPublic | BindingFlags.Instance);

            if (drawModeProperty != null) {

                drawModeProperty.SetValue(control, value, null);

                return true;

            }
            else {

                return false;

            }

        }

        private ControlPaintArgs CreateControlPaintArgs(Control control, PaintEventArgs paintEventArgs) {

            return new ControlPaintArgs(control, paintEventArgs.Graphics, StyleSheet, styleRenderer);

        }

    }

}