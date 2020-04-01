using Gsemac.Forms.Styles.Controls;
using Gsemac.Forms.Styles.StyleSheets;
using System;
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

            ControlUtilities.SetStyle(control, ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            ControlInfo info = GetControlInfo(control);

            control.Paint += PaintEventHandler;

            info.ResetControl += (c) => {
                control.Paint -= PaintEventHandler;
            };

            if (control is ListBox listBox)
                ApplyStyles(listBox, info);

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

    }

}