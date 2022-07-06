using Gsemac.Forms.Styles.Renderers2;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2 {

    internal class UserPaintStyleApplicator<T> :
        ControlStyleApplicatorBase<T> where T : Control {

        // Public members

        public UserPaintStyleApplicator() :
            this(ControlRendererFactory.Default) {
        }
        public UserPaintStyleApplicator(IStyleRendererFactory styleRendererFactory) {

            if (styleRendererFactory is null)
                throw new ArgumentNullException(nameof(styleRendererFactory));

            this.styleRendererFactory = styleRendererFactory;

        }

        public override void InitializeStyle(T obj) {

            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (IsSupportedControl(obj)) {

                ControlUtilities.SetDoubleBuffered(obj, true);
                ControlUtilities.SetStyle(obj, ControlStyles.UserPaint, true);

                obj.Paint += PaintEventHandler;

            }

            base.InitializeStyle(obj);

        }
        public override void DeinitializeStyle(T obj) {

            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            obj.Paint -= PaintEventHandler;

            styles.Remove(obj);

            base.DeinitializeStyle(obj);

        }

        public override void ApplyStyle(T obj, IRuleset ruleset) {

            styles[obj] = ruleset;

        }

        // Private members

        private readonly IStyleRendererFactory styleRendererFactory;
        private readonly IDictionary<T, IRuleset> styles = new Dictionary<T, IRuleset>();

        private bool IsSupportedControl(Control control) {

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

            IStyleRenderer renderer = styleRendererFactory.Create(sender.GetType());

            if (renderer is object && sender is Control control && styles.TryGetValue((T)control, out IRuleset style)) {

                IRenderContext context = new RenderContext(e.Graphics, control.ClientRectangle, style);

                renderer.Render(sender, context);

            }

        }

    }

}