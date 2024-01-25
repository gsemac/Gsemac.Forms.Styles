﻿using Gsemac.Forms.Styles.Renderers2;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2 {

    internal class WrapperControlUserPaintStyleApplicator<T> :
        ControlStyleApplicatorBase<T> where T : Control {

        // Public members

        public WrapperControlUserPaintStyleApplicator() :
            this(ControlRendererFactory.Default) {
        }
        public WrapperControlUserPaintStyleApplicator(bool forwardPaintEventsToChildControl) :
            this(ControlRendererFactory.Default, forwardPaintEventsToChildControl) {
        }
        public WrapperControlUserPaintStyleApplicator(IStyleRendererFactory styleRendererFactory) :
            this(styleRendererFactory, forwardPaintEventsToChildControl: false) {
        }
        public WrapperControlUserPaintStyleApplicator(IStyleRendererFactory styleRendererFactory, bool forwardPaintEventsToChildControl) {

            if (styleRendererFactory is null)
                throw new ArgumentNullException(nameof(styleRendererFactory));

            this.styleRendererFactory = styleRendererFactory;
            this.forwardPaintEventsToChildControl = forwardPaintEventsToChildControl;

        }

        public override void InitializeStyle(T control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (control.Parent is null)
                return;

            // Wrap the control in a new control that allows us to paint on it.

            WrapperControl wrapperControl = WrapControl(control);

            base.InitializeStyle(control);

            control.Invalidated += InvalidatedHandler;
            control.Parent.Paint += PaintEventHandler;

            if (forwardPaintEventsToChildControl) {

                // This allows the child control to support custom rendering as well.
                // This is for controls that need the border provided by the wrapper control while still using custom rendering (e.g. ListBox).

                ControlUtilities.SetDoubleBuffered(control, true);
                ControlUtilities.SetStyle(control, ControlStyles.UserPaint, true);

                control.Paint += PaintEventHandler;

            }

        }
        public override void DeinitializeStyle(T control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            control.Invalidated -= InvalidatedHandler;
            control.Parent.Paint -= PaintEventHandler;

            control.Paint -= PaintEventHandler;

            styles.Remove(control);

            UnwrapControl(control);

            base.DeinitializeStyle(control);

        }

        public override void ApplyStyle(T control, IRuleset ruleset) {

            styles[control] = ruleset;

            base.ApplyStyle(control, ruleset);

        }

        // Protected members

        protected virtual WrapperControl WrapControl(T control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            Control parent = control.Parent;
            bool hasParent = parent is object;

            if (hasParent) {

                parent.SuspendLayout();

                parent.Controls.Remove(control);

            }

            WrapperControl wrapperControl = new WrapperControl(control);

            if (hasParent) {

                parent.Controls.Add(wrapperControl);

                parent.ResumeLayout();

            }

            return wrapperControl;

        }
        protected virtual T UnwrapControl(T control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (control.Parent is WrapperControl wrapperControl && wrapperControl.Parent is object) {

                Control parent = wrapperControl.Parent;

                parent.SuspendLayout();

                parent.Controls.Remove(wrapperControl);
                wrapperControl.Controls.Clear();

                parent.Controls.Add(control);

                parent.ResumeLayout();

                wrapperControl.Dispose();

            }

            return control;

        }

        // Private members

        private readonly IStyleRendererFactory styleRendererFactory;
        private readonly IDictionary<T, IRuleset> styles = new Dictionary<T, IRuleset>();
        private readonly bool forwardPaintEventsToChildControl = false;

        private void InvalidatedHandler(object sender, EventArgs e) {

            if (!(sender is T control) || control.Parent is null)
                return;

            // When the wrapped control is invalidated, the parent control should be invalidated too.

            control.Parent.Invalidate(invalidateChildren: false);

        }
        private void PaintEventHandler(object sender, PaintEventArgs e) {

            // This event handler is called by the parent control.

            if (sender is WrapperControl wrapperControl && wrapperControl.ChildControl is T childControl) {

                // We are rendering on the parent (wrapper) control.

                IStyleRenderer renderer = styleRendererFactory.Create(childControl.GetType());

                if (renderer is object && styles.TryGetValue(childControl, out IRuleset style)) {

                    IRenderContext context = new RenderContext(e.Graphics, wrapperControl.ClientRectangle, style) {
                        IsRenderingBackground = true,
                    };

                    renderer.Render(childControl, context);

                }

            }
            else if (sender is T control) {

                // We are rendering on the child control (only occurs if enabled).

                IStyleRenderer renderer = styleRendererFactory.Create(control.GetType());

                if (renderer is object && styles.TryGetValue(control, out IRuleset style)) {

                    IRenderContext context = new RenderContext(e.Graphics, control.ClientRectangle, style);

                    renderer.Render(control, context);

                }

            }

        }

    }

}