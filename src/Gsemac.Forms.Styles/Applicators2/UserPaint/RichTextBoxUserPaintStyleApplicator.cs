using Gsemac.Forms.Styles.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2.UserPaint
{

    internal sealed class RichTextBoxUserPaintStyleApplicator :
        WrapperControlUserPaintStyleApplicator<RichTextBox> {

        // Public members

        public override void InitializeStyle(RichTextBox richTextBox) {

            if (richTextBox is null)
                throw new ArgumentNullException(nameof(richTextBox));

            base.InitializeStyle(richTextBox);

        }

        // Protected members

        protected override WrapperControl WrapControl(RichTextBox richTextBox) {

            if (richTextBox is null)
                throw new ArgumentNullException(nameof(richTextBox));

            Size originalSize = richTextBox.Size;

            richTextBox.BorderStyle = BorderStyle.None;

            WrapperControl borderControl = base.WrapControl(richTextBox);

            borderControl.Padding = new Padding(3, 3, 3, 3);

            ControlUtilities2.Resize(borderControl, originalSize);

            return borderControl;

        }

    }

}