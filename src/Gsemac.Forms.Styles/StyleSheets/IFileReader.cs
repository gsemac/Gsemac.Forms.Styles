using System.IO;

namespace Gsemac.Forms.Styles.StyleSheets {

    public interface IFileReader {

        Stream OpenFile(string filePath);

    }

}