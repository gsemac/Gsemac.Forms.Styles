﻿using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public interface IControlRenderer {

        void RenderControl(Graphics graphics, Control control);

    }

    public interface IControlRenderer<T> :
      IControlRenderer where T : Control {

        void RenderControl(Graphics graphics, T control);

    }

}