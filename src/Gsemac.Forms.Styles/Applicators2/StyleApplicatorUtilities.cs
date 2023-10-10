using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2 {

    internal static class StyleApplicatorUtilities {

        // Public members

        public static bool ControlSupportsUserPaint(Control control) {

            if (control is null)
                return false;

            // - Controls that use TextBoxes generally need special treatment to render correctly.
            // - ToolStrips and MenuStrips (which inherit from ToolStrip) are drawn through the custom ToolStripRenderers supplied to the "Renderer" property.
            // - ListViews and DataGridViews are drawn through event handlers.
            // - ScrollBars are drawn by the operating system (but this can be worked around: https://stackoverflow.com/a/4656361/5383169).

            if (control is DataGridView || control is ListView || control is NumericUpDown || control is RichTextBox || control is TextBox || control is ScrollBar || control is ToolStrip)
                return false;

            // Only ComboBoxes with the DropDownList style do not use a TextBox, and can be fully painted.

            if (control is ComboBox comboBox && comboBox.DropDownStyle != ComboBoxStyle.DropDownList)
                return false;

            return true;

        }

    }

}