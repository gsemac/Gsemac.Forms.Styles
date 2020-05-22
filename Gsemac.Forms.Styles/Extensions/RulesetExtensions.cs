using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.Extensions {

    public static class RulesetExtensions {

        public static Borders GetBorders(this IRuleset ruleset) {

            Border top = new Border(ruleset.BorderTopWidth?.Value ?? 0, ruleset.BorderTopStyle?.Value ?? BorderStyle.Solid, ruleset.BorderTopColor?.Value ?? default);
            Border right = new Border(ruleset.BorderRightWidth?.Value ?? 0, ruleset.BorderRightStyle?.Value ?? BorderStyle.Solid, ruleset.BorderRightColor?.Value ?? default);
            Border bottom = new Border(ruleset.BorderBottomWidth?.Value ?? 0, ruleset.BorderBottomStyle?.Value ?? BorderStyle.Solid, ruleset.BorderBottomColor?.Value ?? default);
            Border left = new Border(ruleset.BorderLeftWidth?.Value ?? 0, ruleset.BorderLeftStyle?.Value ?? BorderStyle.Solid, ruleset.BorderLeftColor?.Value ?? default);

            return new Borders(top, right, bottom, left);

        }
        public static BorderRadius GetBorderRadii(this IRuleset ruleset) {

            double topLeft = ruleset.BorderTopLeftRadius?.Value ?? 0;
            double topRight = ruleset.BorderTopRightRadius?.Value ?? 0;
            double bottomRight = ruleset.BorderBottomRightRadius?.Value ?? 0;
            double bottomLeft = ruleset.BorderBottomLeftRadius?.Value ?? 0;

            return new BorderRadius(topLeft, topRight, bottomRight, bottomLeft);

        }

    }

}