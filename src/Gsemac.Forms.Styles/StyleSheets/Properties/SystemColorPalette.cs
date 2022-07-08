using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class SystemColorPalette :
        ISystemColorPalette {

        // Public members

        public Color CanvasText => SystemColors.WindowText;

        public static SystemColorPalette Default => new SystemColorPalette();

    }

}