using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public interface ILinearGradient :
        IGradient {

        IMeasurement Direction { get; }
        IEnumerable<ColorStop> ColorStops { get; }

    }

}