﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2 {

    internal class TextBoxUserPaintStyleApplicator :
        WrapperControlUserPaintStyleApplicator<TextBox> {

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

            int bottomPadding = textBox.Multiline ? 3 : 0;

            borderControl.Padding = new Padding(3, 3, 3, bottomPadding);

            ControlUtilities2.Resize(borderControl, originalSize);

            return borderControl;

        }

    }

}