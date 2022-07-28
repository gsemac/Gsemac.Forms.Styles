using System;
using System.Reflection;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles {

    public static class ControlUtilities2 {

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

    }

}