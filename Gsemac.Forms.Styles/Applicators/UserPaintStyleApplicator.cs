using Gsemac.Forms.Styles.Controls;
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

            controlRenderer = new ControlRenderer(styleSheet);

        }

        // Protected members

        protected override void OnApplyStyles(Control control) {

            ControlInfo info = GetControlInfo(control);

            if (!(control is TextBox || control is NumericUpDown)) {

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

            }

        }

        // Private members

        private readonly ControlRenderer controlRenderer;

        private void PaintEventHandler(object sender, PaintEventArgs e) {

            if (sender is Control control)
                controlRenderer.RenderControl(e.Graphics, control);

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

                        controlRenderer.RenderControl(e.Graphics, control);

                        e.Graphics.TranslateTransform(-control.Location.X, -control.Location.Y);

                    }

                    control.Parent.Paint += paintHandler;

                    info.ResetControl += (c) => {
                        control.Parent.Paint -= paintHandler;
                    };

                }

            }

        }

        private void ApplyStyles(ListBox control, ControlInfo info) {

            control.DrawMode = DrawMode.OwnerDrawFixed;
            control.BorderStyle = BorderStyle.None;

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

            control.BorderStyle = BorderStyle.None;

            Point originalLocation = control.Location;
            int originalWidth = control.Width;
            int originalHeight = control.Height;

            control.Location = new Point(control.Location.X + 2, control.Location.Y + 2);
            control.Width -= 3;

            control.MouseMove += InvalidateParentHandler; // required for :hover
            control.MouseEnter += InvalidateParentHandler; // required for :hover
            control.MouseLeave += InvalidateParentHandler; // required for :hover
            control.MouseDown += InvalidateParentHandler; // required for :active
            control.GotFocus += InvalidateParentHandler; // required for :focus
            control.LostFocus += InvalidateParentHandler; // required for :focus

            info.ResetControl += (c) => {

                c.Location = originalLocation;
                c.Width = originalWidth;
                c.Height = originalHeight;

                control.MouseMove -= InvalidateParentHandler;
                control.MouseEnter -= InvalidateParentHandler;
                control.MouseLeave -= InvalidateParentHandler;
                control.MouseDown -= InvalidateParentHandler;
                control.GotFocus -= InvalidateParentHandler;
                control.LostFocus -= InvalidateParentHandler;

            };

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

            // Set up properties for the TextBox.
            // Borderless TextBoxes don't have the same offset/size as regular TextBoxes, so we need to adjust it.

            control.BorderStyle = BorderStyle.None;

            Point originalLocation = control.Location;
            int originalWidth = control.Width;
            int originalHeight = control.Height;

            control.Location = new Point(control.Location.X + 3, control.Location.Y + 3);
            control.Width -= 6;

            if (control.Multiline)
                control.Height -= 6;

            // Add event handlers.

            control.MouseMove += InvalidateParentHandler; // required for :hover
            control.MouseEnter += InvalidateParentHandler; // required for :hover
            control.MouseLeave += InvalidateParentHandler; // required for :hover
            control.MouseDown += InvalidateParentHandler; // required for :active
            control.GotFocus += InvalidateParentHandler; // required for :focus
            control.LostFocus += InvalidateParentHandler; // required for :focus

            info.ResetControl += (c) => {

                c.Location = originalLocation;
                c.Width = originalWidth;
                c.Height = originalHeight;

                control.MouseMove -= InvalidateParentHandler;
                control.MouseEnter -= InvalidateParentHandler;
                control.MouseLeave -= InvalidateParentHandler;
                control.MouseDown -= InvalidateParentHandler;
                control.GotFocus -= InvalidateParentHandler;
                control.LostFocus -= InvalidateParentHandler;

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

    }

}