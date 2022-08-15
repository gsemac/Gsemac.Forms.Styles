using Gsemac.Forms.Styles.Renderers2;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2 {

    internal class ParentUserPaintStyleApplicator<T> :
        ControlStyleApplicatorBase<T> where T : Control {

        // Public members

        public ParentUserPaintStyleApplicator() :
            this(ControlRendererFactory.Default) {
        }
        public ParentUserPaintStyleApplicator(IStyleRendererFactory styleRendererFactory) {

            if (styleRendererFactory is null)
                throw new ArgumentNullException(nameof(styleRendererFactory));

            this.styleRendererFactory = styleRendererFactory;

        }

        public override void InitializeStyle(T control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (control.Parent is null)
                return;

            // Wrap the control in a new control that allows us to paint on it.

            BorderControl borderControl = WrapControl(control);

            base.InitializeStyle(control);

            control.Invalidated += InvalidatedHandler;
            control.Parent.Paint += PaintEventHandler;

        }
        public override void DeinitializeStyle(T control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            control.Invalidated -= InvalidatedHandler;
            control.Parent.Paint -= PaintEventHandler;

            styles.Remove(control);

            UnwrapControl(control);

            base.DeinitializeStyle(control);

        }

        public override void ApplyStyle(T control, IRuleset ruleset) {

            styles[control] = ruleset;

            base.ApplyStyle(control, ruleset);

        }

        // Protected members

        protected virtual BorderControl WrapControl(T control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            Control parent = control.Parent;
            bool hasParent = parent is object;

            if (hasParent) {

                parent.SuspendLayout();

                parent.Controls.Remove(control);

            }

            BorderControl wrappedControl = new BorderControl(control);

            if (hasParent) {

                parent.Controls.Add(wrappedControl);

                parent.ResumeLayout();

            }

            return wrappedControl;

        }
        protected virtual T UnwrapControl(T control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (control.Parent is BorderControl borderControl && borderControl.Parent is object) {

                Control parent = borderControl.Parent;

                parent.SuspendLayout();

                parent.Controls.Remove(borderControl);
                borderControl.Controls.Clear();

                parent.Controls.Add(control);

                parent.ResumeLayout();

                borderControl.Dispose();

            }

            return control;

        }

        // Private members

        private readonly IStyleRendererFactory styleRendererFactory;
        private readonly IDictionary<T, IRuleset> styles = new Dictionary<T, IRuleset>();

        private void InvalidatedHandler(object sender, EventArgs e) {

            if (!(sender is T control) || control.Parent is null)
                return;

            // When the wrapped control is invalidated, the parent control should be invalidated too.

            control.Parent.Invalidate(control.ClientRectangle, invalidateChildren: false);

        }
        private void PaintEventHandler(object sender, PaintEventArgs e) {

            // This event handler is called by the parent control.

            if (sender is BorderControl borderControl && borderControl.ChildControl is T childControl) {

                IStyleRenderer renderer = styleRendererFactory.Create(childControl.GetType());

                if (renderer is object && styles.TryGetValue(childControl, out IRuleset style)) {

                    IRenderContext context = new RenderContext(e.Graphics, borderControl.ClientRectangle, style);

                    renderer.Render(childControl, context);

                }

            }

        }

    }

}