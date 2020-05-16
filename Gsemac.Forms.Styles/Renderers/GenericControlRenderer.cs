using Gsemac.Forms.Styles.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class GenericControlRenderer :
        IControlRenderer {

        // Public members

        public GenericControlRenderer(IStyleSheetControlRenderer baseRenderer) {

            this.baseRenderer = baseRenderer;

        }

        public void RenderControl(Graphics graphics, Control control) {

            baseRenderer.PaintBackground(graphics, control);

        }

        // Private members

        private readonly IStyleSheetControlRenderer baseRenderer;

    }

}