using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public interface IStyleRenderer {

        void PaintBackground(Graphics graphics, Rectangle rectangle, IRuleset ruleset);
        void PaintBorder(Graphics graphics, Rectangle rectangle, IRuleset ruleset);
        void PaintText(Graphics graphics, Rectangle rectangle, IRuleset ruleset, string text, Font font, TextFormatFlags textFormatFlags = TextFormatFlags.Left);

    }

}