using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public interface ILinearGradient :
        IGradient {

        Angle Direction { get; }
        IEnumerable<ColorStop> ColorStops { get; }

    }

}