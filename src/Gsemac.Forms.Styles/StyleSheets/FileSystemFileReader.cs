using System.IO;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class FileSystemFileReader :
        IFileReader {

        // Public members

        public string RootPath { get; set; }

        public Stream OpenFile(string filePath) {

            if (!Path.IsPathRooted(filePath) && !string.IsNullOrWhiteSpace(RootPath))
                filePath = Path.Combine(RootPath, filePath);

            return new FileStream(filePath, FileMode.Open, FileAccess.Read);

        }

    }

}