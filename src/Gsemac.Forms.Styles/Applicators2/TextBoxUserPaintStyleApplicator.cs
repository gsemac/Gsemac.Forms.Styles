using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2 {

    internal class TextBoxUserPaintStyleApplicator :
        ParentUserPaintStyleApplicator<TextBox> {

        // Public members

        public override void InitializeStyle(TextBox textBox) {

            if (textBox is null)
                throw new ArgumentNullException(nameof(textBox));

            base.InitializeStyle(textBox);

        }

        // Protected members

        protected override BorderControl WrapControl(TextBox textBox) {

            Size originalSize = textBox.Size;

            textBox.BorderStyle = BorderStyle.None;

            BorderControl borderControl = base.WrapControl(textBox);

            borderControl.Padding = new Padding(0, 0, 1, 0);

            ControlUtilities2.Resize(borderControl, originalSize);

            return borderControl;

        }

    }

}