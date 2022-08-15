using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles {

    public static class ControlUtilities2 {

        // Public members

        public static bool FocusCuesShown(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (!control.Focused)
                return false;

            PropertyInfo showFocusCuesProperty = typeof(Control).GetProperty("ShowFocusCues", BindingFlags.Instance | BindingFlags.NonPublic);

            if (showFocusCuesProperty is null || !showFocusCuesProperty.PropertyType.Equals(typeof(bool)))
                return false;

            return (bool)showFocusCuesProperty.GetValue(control, null);

        }
        public static bool IsChecked(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (control is CheckBox checkBox) {

                return checkBox.Checked;

            }
            else if (control is RadioButton radioButton) {

                return radioButton.Checked;

            }

            return false;

        }

        public static void Inflate(Control control, int width, int height) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            BeginRepositioning(control);

            AnchorStyles anchors = control.Anchor;

            control.Anchor = AnchorStyles.None;

            control.Location = new Point(control.Location.X - width, control.Location.Y - height);
            control.Width += width * 2;
            control.Height += height * 2;

            control.Anchor = anchors;

            EndRepositioning(control);

        }
        public static void Inflate(Control control, Size size) {

            Inflate(control, size.Width, size.Height);

        }
        public static void Offset(Control control, int xOffset, int yOffset) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            BeginRepositioning(control);

            AnchorStyles anchors = control.Anchor;

            control.Anchor = AnchorStyles.None;

            control.Location = new Point(control.Location.X + xOffset, control.Location.Y + yOffset);

            control.Anchor = anchors;

            EndRepositioning(control);

        }
        public static void Resize(Control control, int width, int height) {

            Resize(control, new Size(width, height));

        }
        public static void Resize(Control control, Size size) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            BeginRepositioning(control);

            AnchorStyles anchors = control.Anchor;

            control.Anchor = AnchorStyles.None;

            control.Size = size;

            control.Anchor = anchors;

            EndRepositioning(control);

        }

        // Private members

        private static void BeginRepositioning(Control control) {

            if (control.Parent is object)
                control.Parent.SuspendLayout();

            control.SuspendLayout();

        }
        private static void EndRepositioning(Control control) {

            control.ResumeLayout();

            if (control.Parent is object)
                control.Parent.ResumeLayout();

        }

    }

}