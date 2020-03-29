using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Controls {

    internal static class ControlUtilities {

        public static bool GetStyle(Control control, ControlStyles styles) {

            return (bool)control.GetType()
                .GetMethod("GetStyle", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(control, new object[] { styles });

        }
        public static void SetStyle(Control control, ControlStyles styles, bool value) {

            control.GetType()
                .GetMethod("SetStyle", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(control, new object[] { styles, value });

        }

    }

}