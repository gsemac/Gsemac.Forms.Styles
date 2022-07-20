using System;
using System.IO;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public static class SelectorFactoryExtensions {

        // Public members

        public static ISelector Parse(this ISelectorFactory selectorFactory, string value) {

            if (selectorFactory is null)
                throw new ArgumentNullException(nameof(selectorFactory));

            if (value is null)
                return Selector.Empty;

            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(value)))
                return selectorFactory.FromStream(stream);

        }

    }

}
