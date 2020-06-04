using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public interface IImage {

        void DrawImage(Graphics graphics, Rectangle rect);

    }

}