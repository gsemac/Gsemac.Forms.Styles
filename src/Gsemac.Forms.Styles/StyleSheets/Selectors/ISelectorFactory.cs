using System.IO;

namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public interface ISelectorFactory {

        ISelector FromStream(Stream stream);

    }

}