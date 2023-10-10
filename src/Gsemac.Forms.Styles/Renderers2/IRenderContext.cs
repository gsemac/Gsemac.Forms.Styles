using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System.Drawing;

namespace Gsemac.Forms.Styles.Renderers2 {

    public interface IRenderContext {

        Rectangle ClientRectangle { get; }
        Graphics Graphics { get; }
        IRuleset Style { get; }
        bool IsRenderingBackground { get; }

    }

}