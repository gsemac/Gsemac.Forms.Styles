using Gsemac.Forms.Styles.Dom;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Controls {

    [DomHidden]
    internal class WrapperControl :
        UserControl {

        // Public members

        public WrapperControl(Control childControl) {

            if (childControl is null)
                throw new ArgumentNullException(nameof(childControl));

            this.childControl = childControl;

            // Copy properties from the child control.

            Location = childControl.Location;
            Width = childControl.Width;
            Height = childControl.Height;
            Anchor = childControl.Anchor;

            // Configure the child control.

            childControl.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left;
            childControl.Location = new Point(0, 0);
            childControl.Parent = this;

            Controls.Add(childControl);

            // Enable user paint so that the control can be custom painted.

            SetStyle(ControlStyles.UserPaint, true);

            DoubleBuffered = true;

        }

        public Control ChildControl {
            get => childControl;
        }
        public override string Text {
            get => childControl.Text;
            set => childControl.Text = value;
        }

        // Private members

        private readonly Control childControl;

    }

}