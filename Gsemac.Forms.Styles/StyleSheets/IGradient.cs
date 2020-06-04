using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public interface IGradient :
        IImage {

        void DrawGradient(Graphics graphics, Rectangle rect);

    }

}