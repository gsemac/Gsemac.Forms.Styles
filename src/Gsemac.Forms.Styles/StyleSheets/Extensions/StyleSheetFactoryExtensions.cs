using System.IO;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets.Extensions {

    public static class StyleSheetFactoryExtensions {

        // Public members

        public static IStyleSheet Parse(this IStyleSheetFactory styleSheetFactory, string value) {

            return styleSheetFactory.Parse(value, StyleSheetOptions.Default);

        }
        public static IStyleSheet Parse(this IStyleSheetFactory styleSheetFactory, string value, IStyleSheetOptions options) {

            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(value)))
                return styleSheetFactory.FromStream(stream, options);

        }

        public static IStyleSheet FromStream(this IStyleSheetFactory styleSheetFactory, Stream stream) {

            return FromStream(styleSheetFactory, stream, StyleSheetOptions.Default);

        }
        public static IStyleSheet FromStream(this IStyleSheetFactory styleSheetFactory, Stream stream, IStyleSheetOptions options) {

            return styleSheetFactory.FromStream(stream, options);

        }

        public static IStyleSheet FromFile(this IStyleSheetFactory styleSheetFactory, string filePath) {

            // The FileReader is set such that files are read relative to the stylesheet.

            return styleSheetFactory.FromFile(filePath);

        }
        public static IStyleSheet FromFile(this IStyleSheetFactory styleSheetFactory, string filePath, IStyleSheetOptions options) {

            using (FileStream fstream = new FileStream(filePath, FileMode.Open))
                return styleSheetFactory.FromStream(fstream, options);

        }

    }

}