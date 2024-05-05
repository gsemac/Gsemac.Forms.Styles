using Gsemac.Forms.Styles.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2.UserPaint
{

    internal sealed class NumericUpDownUserPaintStyleApplicator :
        WrapperControlUserPaintStyleApplicator<NumericUpDown> {

        // Public members

        public override void InitializeStyle(NumericUpDown numericUpDown) {

            if (numericUpDown is null)
                throw new ArgumentNullException(nameof(numericUpDown));

            base.InitializeStyle(numericUpDown);

        }

        // Protected members

        protected override WrapperControl WrapControl(NumericUpDown numericUpDown) {

            if (numericUpDown is null)
                throw new ArgumentNullException(nameof(numericUpDown));

            Size originalSize = numericUpDown.Size;

            numericUpDown.BorderStyle = BorderStyle.None;

            WrapperControl borderControl = base.WrapControl(numericUpDown);

            borderControl.Padding = new Padding(3, 2, 2, 0);

            ControlUtilities2.Resize(borderControl, originalSize);

            return borderControl;

        }

    }

}