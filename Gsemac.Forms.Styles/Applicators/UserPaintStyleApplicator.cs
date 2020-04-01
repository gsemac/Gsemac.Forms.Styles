using Gsemac.Forms.Styles.Controls;
using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators {

    public class UserPaintStyleApplicator :
        StyleApplicatorBase {

        // Public members

        public UserPaintStyleApplicator(IStyleSheet styleSheet) {

            controlRenderer = new ControlRenderer(styleSheet);

        }

        // Protected members

        protected override bool HasStyles(Control control) {

            return controlRenderer.HasStyles(control);

        }
        protected override void OnApplyStyles(Control control) {

            SetStyle(control, ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            ControlInfo info = GetControlInfo(control);

            control.Paint += PaintEventHandler;

            info.ResetControl += (c) => {
                control.Paint -= PaintEventHandler;
            };

            switch (control) {

                case ListBox listBox:
                    ApplyStyles(listBox, info);
                    break;

                case Panel panel:
                    ApplyStyles(panel, info);
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
        private void ApplyStyles(Panel control, ControlInfo info) {

            // ResizeRedraw needs to be set to true to prevent smearing.
            // https://stackoverflow.com/a/39419274/5383169

            TryGetResizeRedraw(control, out bool oldResetRedraw);

            TrySetResizeRedraw(control, true);

            info.ResetControl += (c) => {

                TrySetResizeRedraw(control, oldResetRedraw);

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