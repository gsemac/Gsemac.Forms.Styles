using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public static class Selector {

        // Public members

        public static ISelector Empty => new CompositeSelector(Enumerable.Empty<ISelector>());

    }

}