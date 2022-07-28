using Gsemac.Data.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class StringToSideOrCornerConverter :
        ValueConverterBase<string, SideOrCorner> {

        public override bool TryConvert(string value, out SideOrCorner result) {

            result = default;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            value = value.Trim().ToLowerInvariant();

            // We will have the word "to", followed by up to two directions.
            // The order of the directions doesn't matter.

            if (!value.StartsWith("to", StringComparison.OrdinalIgnoreCase))
                return false;

            IEnumerable<string> keywords = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Select(keyword => keyword.Trim())
                .OrderBy(keyword => keyword);

            string joinedKeywords = string.Join(" ", keywords);

            switch (joinedKeywords) {

                case "bottom right":
                    result = SideOrCorner.BottomRight;
                    return true;

                case "bottom left":
                    result = SideOrCorner.BottomLeft;
                    return true;

                case "right top":
                    result = SideOrCorner.TopRight;
                    return true;

                case "left top":
                    result = SideOrCorner.TopLeft;
                    return true;

                case "bottom":
                    result = SideOrCorner.Bottom;
                    return true;

                case "top":
                    result = SideOrCorner.Top;
                    return true;

                case "left":
                    result = SideOrCorner.Left;
                    return true;

                case "right":
                    result = SideOrCorner.Right;
                    return true;

                default:
                    return false;

            }

        }

    }

}