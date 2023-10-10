using Gsemac.Drawing.Imaging.Filters;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2 {

    internal sealed class ListBoxUserPaintStyleApplicator :
        WrapperControlUserPaintStyleApplicator<ListBox> {

        // Public members

        public ListBoxUserPaintStyleApplicator() :
            base(forwardPaintEventsToChildControl: true) {
        }

        public override void InitializeStyle(ListBox listBox) {

            if (listBox is null)
                throw new ArgumentNullException(nameof(listBox));

            listBox.DrawMode = DrawMode.OwnerDrawFixed;

            base.InitializeStyle(listBox);

        }
        public override void DeinitializeStyle(ListBox listBox) {

            if (listBox is null)
                throw new ArgumentNullException(nameof(listBox));

            base.DeinitializeStyle(listBox);

        }

        // Protected members

        protected override WrapperControl WrapControl(ListBox listBox) {

            if (listBox is null)
                throw new ArgumentNullException(nameof(listBox));

            Size originalSize = listBox.Size;

            listBox.BorderStyle = BorderStyle.None;

            WrapperControl borderControl = base.WrapControl(listBox);

            borderControl.Padding = new Padding(2, 2, 2, 2);

            ControlUtilities2.Resize(borderControl, originalSize);

            return borderControl;

        }

    }

}