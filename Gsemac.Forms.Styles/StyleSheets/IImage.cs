using System;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets {

    public interface IImage :
        IDisposable {

        void DrawImage(Graphics graphics, Rectangle rect);

    }

}