using System.IO;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets.Extensions {

    public static class StyleSheetFactoryExtensions {

        // Public members

        public static IStyleSheet Parse(this IStyleSheetFactory styleSheetFactory, string input) {

            return styleSheetFactory.Parse(input, StyleSheetOptions.Default);

        }
        public static IStyleSheet Parse(this IStyleSheetFactory styleSheetFactory, string input, IStyleSheetOptions options) {

            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(input)))
                return styleSheetFactory.FromStream(stream, options);

        }
        public static IStyleSheet FromStream(this IStyleSheetFactory styleSheetFactory, Stream stream) {

            return styleSheetFactory.FromStream(stream, StyleSheetOptions.Default);

        }
        public static IStyleSheet FromFile(this IStyleSheetFactory styleSheetFactory, string filePath) {

            // The FileReader is set such that files are read relative to the stylesheet.

            return styleSheetFactory.FromFile(filePath, new StyleSheetOptions() {
                FileReader = new FileSystemFileReader() {
                    RootPath = Path.GetDirectoryName(filePath)
                }
            });

        }
        public static IStyleSheet FromFile(this IStyleSheetFactory styleSheetFactory, string filePath, IStyleSheetOptions options) {

            using (FileStream fstream = new FileStream(filePath, FileMode.Open))
                return styleSheetFactory.FromStream(fstream, options);

        }

    }

}