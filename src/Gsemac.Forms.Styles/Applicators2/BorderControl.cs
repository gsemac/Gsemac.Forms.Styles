﻿using Gsemac.Forms.Styles.Dom;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2 {

    [DomHidden]
    internal class BorderControl :
        UserControl {

        // Public members

        public BorderControl(Control childControl) {

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

            Controls.Add(childControl);

            // Enable user paint so that the control can be custom painted.

            SetStyle(ControlStyles.UserPaint, true);

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