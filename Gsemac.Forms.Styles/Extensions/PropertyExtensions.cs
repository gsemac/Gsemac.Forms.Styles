using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.Extensions {

    public static class PropertyExtensions {

        public static bool HasValue(this IProperty property) {

            return property != null;

        }

    }

}
