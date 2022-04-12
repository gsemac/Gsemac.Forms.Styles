using System.IO;

namespace Gsemac.Forms.Styles.StyleSheets {

    public interface IStyleSheetFactory {

        IStyleSheet FromStream(Stream stream, IStyleSheetOptions options);

    }

}