
/* Unmerged change from project 'Gsemac.Forms.Styles (net461)'
Before:
using System.Drawing;
After:
using Gsemac;
using Gsemac.Forms;
using Gsemac.Forms.Styles;
using Gsemac.Forms.Styles.StyleSheets;
using Gsemac.Forms.Styles.StyleSheets;
using Gsemac.Forms.Styles.StyleSheets.Properties;
using System.Drawing;
*/
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class SystemColorPalette :
        ISystemColorPalette {

        // Public members

        public Color CanvasText => SystemColors.WindowText;

        public static SystemColorPalette Default => new SystemColorPalette();

    }

}