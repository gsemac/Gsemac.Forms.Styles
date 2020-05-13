using Gsemac.Forms.Styles.StyleSheets;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public interface IStyleSheetControlRenderer :
        IControlRenderer {

        IRuleset GetRuleset(Control control, bool inherit);
        IRuleset GetRuleset(INode node, bool inherit);

        void PaintBackground(Graphics graphics, Rectangle rectangle, IRuleset ruleset);

        void PaintForeground(Graphics graphics, string text, Font font, Rectangle rectangle, IRuleset ruleset, TextFormatFlags textFormatFlags);

    }

}