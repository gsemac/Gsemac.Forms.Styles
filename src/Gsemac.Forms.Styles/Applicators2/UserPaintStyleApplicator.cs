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

            if (StyleApplicatorUtilities.ControlSupportsUserPaint(obj)) {

                ControlUtilities.SetDoubleBuffered(obj, true);
                ControlUtilities.SetStyle(obj, ControlStyles.UserPaint, true);

                // Make sure we never add the PaintEventHandler more than once.
                // InitializeStyle should not be called more than once, but let's be safe!

                obj.Paint -= PaintEventHandler;
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

        private void PaintEventHandler(object sender, PaintEventArgs e) {

            IStyleRenderer renderer = styleRendererFactory.Create(sender.GetType());

            if (renderer is object && sender is Control control && styles.TryGetValue((T)control, out IRuleset style)) {

                IRenderContext context = new RenderContext(e.Graphics, control.ClientRectangle, style);

                renderer.Render(sender, context);

            }

        }

    }

}