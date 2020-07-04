using Gsemac.Forms.Styles.Renderers;
using Gsemac.Forms.Styles.StyleSheets;
using Gsemac.Forms.Utilities;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators {

    public class UserPaintStyleApplicator :
        StyleSheetStyleApplicatorBase {

        // Public members

        public UserPaintStyleApplicator(IStyleSheet styleSheet) :
            base(styleSheet) {

            controlRenderer = new ControlRenderer();

        }

        // Protected members

        protected override void OnApplyStyles(Control control) {

            ControlInfo info = GetControlInfo(control);

            if (ControlSupportsUserPaint(control)) {

                ControlUtilities.SetDoubleBuffered(control, true);

                ControlUtilities.SetStyle(control, ControlStyles.UserPaint, true);

                control.Paint += PaintEventHandler;

                // The following handlers are required in order for the ":focus-within" pseudo-class to work.
                // "Enter" and "Exit" are important, as they are triggered for parent controls when child controls gain or lose focus.

                control.GotFocus += InvalidateEventHandler;
                control.LostFocus += InvalidateEventHandler;

                control.Enter += InvalidateEventHandler;
                control.Leave += InvalidateEventHandler;

                info.ResetControl += (c) => {

                    control.Paint -= PaintEventHandler;

                    control.GotFocus -= InvalidateEventHandler;
                    control.LostFocus -= InvalidateEventHandler;

                    control.Enter -= InvalidateEventHandler;
                    control.Leave -= InvalidateEventHandler;

                };

            }

            switch (control) {

                case Button button:

                    ApplyStyles(button, info);

                    break;

                case CheckBox checkBox:

                    ApplyStyles(checkBox, info);

                    break;

                case DataGridView dataGridView:

                    ApplyStyles(dataGridView, info);

                    break;

                case ListBox listBox:

                    ApplyStyles(listBox, info);

                    break;

                case ListView listView:

                    ApplyStyles(listView, info);

                    break;

                case NumericUpDown numericUpDown:

                    ApplyStyles(numericUpDown, info);

                    break;

                case Panel panel:

                    ApplyStyles(panel);

                    break;

                case RadioButton radioButton:

                    ApplyStyles(radioButton, info);

                    break;

                case RichTextBox richTextBox:

                    ApplyStyles(richTextBox, info);

                    break;

                case TextBox textBox:

                    ApplyStyles(textBox, info);

                    break;

                case ToolStrip toolStrip:

                    ApplyStyles(toolStrip);

                    break;

                case TreeView treeView:

                    ApplyStyles(treeView, info);

                    break;

            }

            controlRenderer.InitializeControl(control);

            if (info.ParentDraw) {

                AddParentPaintHandler(control, info);
                AddTextBoxInvalidateParentHandlers(control, info);

            }

        }
        protected override void OnClearStyles(Control control) {

            ControlNode.ClearControlState(control);

        }

        // Private members

        private readonly ControlRenderer controlRenderer;
        private readonly IStyleRenderer styleRenderer = new StyleRenderer();

        private bool ControlSupportsUserPaint(Control control) {

            // - Controls that use TextBoxes generally need special treatment to render correctly.
            // - ToolStrips and MenuStrips (which inherit from ToolStrip) are drawn through the custom ToolStripRenderers supplied to the "Renderer" property.
            // - ListViews and DataGridViews are drawn through event handlers.
            // - ScrollBars are drawn by the operating system (but this can be worked around: https://stackoverflow.com/a/4656361/5383169).

            if (control is DataGridView || control is ListView || control is NumericUpDown || control is RichTextBox || control is ScrollBar || control is TextBox || control is ToolStrip)
                return false;

            // Only ComboBoxes with the DropDownList style do not use a TextBox, and can be fully painted.

            if (control is ComboBox comboBox && comboBox.DropDownStyle != ComboBoxStyle.DropDownList)
                return false;

            return true;

        }

        private void PaintEventHandler(object sender, PaintEventArgs e) {

            if (sender is Control control) {

                ControlInfo controlInfo = GetControlInfo(control);

                if (controlInfo?.ParentDraw ?? false) {

                    // We'll paint the control twice with different clip settings.
                    // The first paint will be on the control itself in this event handler, and it will be painted again by the parent control.

                    e.Graphics.SetClip(control.ClientRectangle);

                }

                controlRenderer.PaintControl(control, CreateControlPaintArgs(control, e, false));

            }

        }
        private void InvalidateEventHandler(object sender, EventArgs e) {

            if (sender is Control control)
                control.Invalidate();

        }
        private void ParentInvalidateEventHandler(object sender, EventArgs e) {

            if (sender is Control control && control.Parent != null)
                control.Parent.Invalidate();

        }
        private void SpacebarKeyDownEventHandler(object sender, KeyEventArgs e) {

            if (sender is Control control) {

                if (e.KeyCode == Keys.Space) {

                    ControlNode.AddControlState(control, NodeStates.Active);

                    control.Invalidate();

                }

            }

        }
        private void SpacebarKeyUpEventHandler(object sender, KeyEventArgs e) {

            if (sender is Control control) {

                if (e.KeyCode == Keys.Space) {

                    ControlNode.RemoveControlState(sender as Control, NodeStates.Active);

                    control.Invalidate();

                }

            }

        }

        private void AddParentPaintHandler(Control control, ControlInfo info) {

            Control parentControl = control.Parent;

            if (parentControl != null) {

                ControlInfo parentControlInfo = GetControlInfo(parentControl);

                if (parentControlInfo != null) {

                    // Add event handlers to the parent control.

                    void paintHandler(object sender, PaintEventArgs e) {

                        GraphicsState graphicsState = e.Graphics.Save();

                        e.Graphics.TranslateTransform(control.Location.X, control.Location.Y);

                        Region region = new Region(parentControl.ClientRectangle);

                        region.Exclude(control.ClientRectangle);

                        controlRenderer.PaintControl(control, CreateControlPaintArgs(control, e, true));

                        e.Graphics.Restore(graphicsState);

                    }

                    control.Parent.Paint += paintHandler;

                    info.ResetControl += (c) => {

                        control.Parent.Paint -= paintHandler;

                    };

                }

            }

        }
        private void AddTextBoxInvalidateParentHandlers(Control control, ControlInfo info) {

            control.MouseEnter += ParentInvalidateEventHandler; // required for :hover
            control.MouseLeave += ParentInvalidateEventHandler; // required for :hover
            control.MouseDown += ParentInvalidateEventHandler; // required for :active
            control.GotFocus += ParentInvalidateEventHandler; // required for :focus
            control.LostFocus += ParentInvalidateEventHandler; // required for :focus
            control.SizeChanged += ParentInvalidateEventHandler;
            control.LocationChanged += ParentInvalidateEventHandler;

            info.ResetControl += (c) => {

                control.MouseEnter -= ParentInvalidateEventHandler;
                control.MouseLeave -= ParentInvalidateEventHandler;
                control.MouseDown -= ParentInvalidateEventHandler;
                control.GotFocus -= ParentInvalidateEventHandler;
                control.LostFocus -= ParentInvalidateEventHandler;
                control.SizeChanged -= ParentInvalidateEventHandler;
                control.LocationChanged -= ParentInvalidateEventHandler;

            };

        }
        private void AddSpacebarEventHandlers(Control control, ControlInfo info) {

            control.KeyDown += SpacebarKeyDownEventHandler;
            control.KeyUp += SpacebarKeyUpEventHandler;

            info.ResetControl += (c) => {

                control.KeyDown -= SpacebarKeyDownEventHandler;
                control.KeyUp -= SpacebarKeyUpEventHandler;

            };

        }

        private void ApplyStyles(Button control, ControlInfo info) {

            AddSpacebarEventHandlers(control, info);

        }
        private void ApplyStyles(CheckBox control, ControlInfo info) {

            // By default, the entire check region is not invalidated when the control is activated (?).
            // When the check has a border that changes when :active, the top border is not overwritten without this.

            control.MouseDown += InvalidateEventHandler;
            control.KeyDown += InvalidateEventHandler;

            info.ResetControl += (c) => {

                control.MouseDown -= InvalidateEventHandler;
                control.KeyDown -= InvalidateEventHandler;

            };

            AddSpacebarEventHandlers(control, info);

        }
        private void ApplyStyles(DataGridView control, ControlInfo info) {

            info.ParentDraw = true;

            DataGridViewRenderer renderer = new DataGridViewRenderer(StyleSheet, styleRenderer);

            control.BorderStyle = System.Windows.Forms.BorderStyle.None;
            control.EnableHeadersVisualStyles = false;

            ControlUtilities.SetDoubleBuffered(control, true);

            control.RowPrePaint += renderer.RowPrePaint;
            control.RowPostPaint += renderer.RowPostPaint;
            control.CellPainting += renderer.CellPainting;
            control.Paint += renderer.Paint;

            // Only the current cell is invalidated when the DataGridView loses focus, so we need to invalidate the entire thing to ensure all cells appear correctly.

            control.Scroll += InvalidateEventHandler;
            control.LostFocus += InvalidateEventHandler;

            info.ResetControl += (c) => {

                control.RowPrePaint -= renderer.RowPrePaint;
                control.RowPostPaint -= renderer.RowPostPaint;
                control.CellPainting -= renderer.CellPainting;
                control.Paint -= renderer.Paint;

                control.Scroll -= InvalidateEventHandler;
                control.LostFocus -= InvalidateEventHandler;

            };

        }
        private void ApplyStyles(ListBox control, ControlInfo info) {

            info.ParentDraw = true;

            control.DrawMode = DrawMode.OwnerDrawFixed;
            control.BorderStyle = System.Windows.Forms.BorderStyle.None;

            control.MouseMove += InvalidateEventHandler; // required for :hover
            control.MouseEnter += InvalidateEventHandler; // required for :hover
            control.MouseLeave += InvalidateEventHandler; // required for :hover
            control.SelectedIndexChanged += InvalidateEventHandler; // required for item selection
            control.MouseDown += InvalidateEventHandler; // required for item selection

            info.ResetControl += (c) => {

                control.MouseMove -= InvalidateEventHandler;
                control.MouseEnter -= InvalidateEventHandler;
                control.MouseLeave -= InvalidateEventHandler;
                control.SelectedIndexChanged -= InvalidateEventHandler;
                control.MouseDown -= InvalidateEventHandler;

            };

        }
        private void ApplyStyles(ListView control, ControlInfo info) {

            info.ParentDraw = true;

            ListViewRenderer renderer = new ListViewRenderer(StyleSheet, styleRenderer);

            bool ownerDraw = control.OwnerDraw;
            bool gridLines = control.GridLines;

            control.OwnerDraw = true;
            control.BorderStyle = System.Windows.Forms.BorderStyle.None;
            control.GridLines = false;

            ControlUtilities.SetDoubleBuffered(control, true);

            control.DrawColumnHeader += renderer.DrawColumnHeader;
            control.DrawItem += renderer.DrawItem;
            control.DrawSubItem += renderer.DrawSubItem;

            info.ResetControl += (c) => {

                control.DrawColumnHeader -= renderer.DrawColumnHeader;
                control.DrawItem -= renderer.DrawItem;
                control.DrawSubItem -= renderer.DrawSubItem;

            };

        }
        private void ApplyStyles(NumericUpDown control, ControlInfo info) {

            AddParentPaintHandler(control, info);
            AddTextBoxInvalidateParentHandlers(control, info);

            Control upDownButtonsControl = control.Controls.Cast<Control>()
                .Where(c => c.GetType().FullName.Equals("System.Windows.Forms.UpDownBase+UpDownButtons"))
                .FirstOrDefault();

            Control textBoxControl = control.Controls.Cast<Control>()
                .Where(c => c is TextBox)
                .FirstOrDefault();

            if (upDownButtonsControl != null) {

                void invalidateUpDownButtons(object sender, EventArgs e) {

                    upDownButtonsControl.Invalidate();

                }

                control.MouseEnter += invalidateUpDownButtons;
                control.MouseLeave += invalidateUpDownButtons;
                control.MouseDown += invalidateUpDownButtons;
                control.GotFocus += invalidateUpDownButtons;
                control.LostFocus += invalidateUpDownButtons;

                if (textBoxControl != null) {

                    // This is necessary so that the UpDownButtons are invalidated when the cursor moves from the button into the TextBox.
                    // Using MouseEnter does not seem to work, presumably because the cursor is already in the TextBox when over the UpDownButtons.

                    textBoxControl.MouseMove += invalidateUpDownButtons;

                }

                info.ResetControl += (c) => {

                    control.MouseEnter -= invalidateUpDownButtons;
                    control.MouseLeave -= invalidateUpDownButtons;
                    control.MouseDown -= invalidateUpDownButtons;
                    control.GotFocus -= invalidateUpDownButtons;
                    control.LostFocus -= invalidateUpDownButtons;

                    if (textBoxControl != null) {

                        textBoxControl.MouseMove -= invalidateUpDownButtons;

                    }

                };

            }

            control.BorderStyle = System.Windows.Forms.BorderStyle.None;
            control.Location = new Point(control.Location.X + 2, control.Location.Y + 2);
            control.Width -= 3;

        }
        private void ApplyStyles(Panel control) {

            // ResizeRedraw needs to be set to true to prevent smearing.
            // https://stackoverflow.com/a/39419274/5383169

            TrySetResizeRedraw(control, true);

        }
        private void ApplyStyles(RadioButton control, ControlInfo info) {

            AddSpacebarEventHandlers(control, info);

        }
        private void ApplyStyles(RichTextBox control, ControlInfo info) {

            control.BorderStyle = System.Windows.Forms.BorderStyle.None;
            control.Location = new Point(control.Location.X + 3, control.Location.Y + 3);
            control.Width -= 6;
            control.Height -= 6;

            info.ParentDraw = true;

        }
        private void ApplyStyles(TextBox control, ControlInfo info) {

            // We need the TextBox to have a parent control so we can draw the TextBox in the parent's OnPaint.

            AddParentPaintHandler(control, info);
            AddTextBoxInvalidateParentHandlers(control, info);

            // Set up properties for the TextBox.
            // Borderless TextBoxes don't have the same offset/size as regular TextBoxes, so we need to adjust it.

            control.BorderStyle = System.Windows.Forms.BorderStyle.None;
            control.Location = new Point(control.Location.X + 3, control.Location.Y + 4);
            control.Width -= 6;

            if (control.Multiline)
                control.Height -= 6;

        }
        private void ApplyStyles(ToolStrip control) {

            control.Renderer = new Renderers.ToolStripRenderer(StyleSheet, styleRenderer);

        }
        private void ApplyStyles(TreeView control, ControlInfo info) {

            info.ParentDraw = true;

            control.DrawMode = TreeViewDrawMode.OwnerDrawAll;

            control.AfterExpand += InvalidateEventHandler;
            control.AfterCollapse += InvalidateEventHandler;

            info.ResetControl += (c) => {

                control.AfterExpand -= InvalidateEventHandler;
                control.AfterCollapse -= InvalidateEventHandler;

            };

        }

        private bool TrySetResizeRedraw(Control control, bool value) {

            PropertyInfo drawModeProperty = control.GetType().GetProperty("ResizeRedraw", BindingFlags.NonPublic | BindingFlags.Instance);

            if (drawModeProperty != null) {

                drawModeProperty.SetValue(control, value, null);

                return true;

            }
            else {

                return false;

            }

        }

        private ControlPaintArgs CreateControlPaintArgs(Control control, PaintEventArgs paintEventArgs, bool parentDraw) {

            return new ControlPaintArgs(control, paintEventArgs.Graphics, StyleSheet, styleRenderer, parentDraw);

        }

    }

}