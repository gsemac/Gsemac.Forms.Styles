using Gsemac.Forms.Styles.Controls;
using Gsemac.Forms.Styles.StyleSheets;
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

            AddPaintEventHandler(control);

        }

        // Private members

        private readonly ControlRenderer controlRenderer;

        private void AddPaintEventHandler(Control control) {

            ControlUtilities.SetStyle(control, ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            if (control is ListBox listBox)
                listBox.DrawMode = DrawMode.OwnerDrawFixed;

            ControlInfo info = GetControlInfo(control);

            info.PaintEventHandler = PaintEventHandler;

            control.Paint += info.PaintEventHandler;

        }

        private void PaintEventHandler(object sender, PaintEventArgs e) {

            if (sender is Control control)
                controlRenderer.RenderControl(e.Graphics, control);

        }

    }

}