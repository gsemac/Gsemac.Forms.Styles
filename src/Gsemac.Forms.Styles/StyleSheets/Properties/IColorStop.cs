using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public interface IColorStop {

        Color Color { get; }
        Measurement StopPosition { get; }

    }

}