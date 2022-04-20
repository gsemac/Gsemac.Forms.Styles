using Gsemac.Forms.Styles.StyleSheets.Properties;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gsemac.Forms.Styles.StyleSheets {

    public static class PropertyUtilities {

        // Public members

        public static bool TryParseColor(string input, out Color result) {

            try {

                input = input.Replace("grey", "gray");

                result = ColorTranslator.FromHtml(input);

                return true;

            }
            catch (Exception) {

                result = default;

                return false;

            }

        }
        public static bool TryParseNumber(string input, out double result) {

            if (input.EndsWith("px", StringComparison.OrdinalIgnoreCase))
                input = input.Substring(0, input.IndexOf("px", StringComparison.OrdinalIgnoreCase));

            return double.TryParse(input, out result) || TryParseAngle(input, out result);

        }
        public static bool TryParseBorderStyle(string input, out BorderStyle result) {

            switch (input?.Trim()?.ToLowerInvariant()) {

                case "dotted":
                    result = BorderStyle.Dotted;
                    break;

                case "dashed":
                    result = BorderStyle.Dashed;
                    break;

                case "solid":
                    result = BorderStyle.Solid;
                    break;

                case "double":
                    result = BorderStyle.Double;
                    break;

                case "groove":
                    result = BorderStyle.Groove;
                    break;

                case "ridge":
                    result = BorderStyle.Ridge;
                    break;

                case "inset":
                    result = BorderStyle.Inset;
                    break;

                case "outset":
                    result = BorderStyle.Outset;
                    break;

                case "none":
                    result = BorderStyle.None;
                    break;

                case "hidden":
                    result = BorderStyle.Hidden;
                    break;

                default:
                    result = BorderStyle.None;
                    return false;

            }

            return true;

        }

        public static Color ParseColor(string input) {

            if (TryParseColor(input, out Color result))
                return result;
            else
                throw new FormatException($"Failed to parse \"{input}\" as a color.");

        }
        public static double ParseNumber(string input) {

            if (TryParseNumber(input, out double result))
                return result;
            else
                throw new FormatException($"Failed to parse \"{input}\" as a number.");

        }
        public static BorderStyle ParseBorderStyle(string input) {

            if (TryParseBorderStyle(input, out BorderStyle result))
                return result;
            else
                throw new FormatException($"Failed to parse \"{input}\" as a border style.");

        }

        public static Color Rgb(int r, int g, int b) {

            return Color.FromArgb(r, g, b);

        }
        public static Color Rgba(int r, int g, int b, float alpha) {

            return Color.FromArgb((int)Math.Round(byte.MaxValue * alpha), r, g, b);

        }
        public static ILinearGradient LinearGradient(double degrees, Color[] colorStops) {

            return new LinearGradient(degrees, colorStops);

        }
        public static StyleObject Url(string resourceFilePath) {

            return Url(resourceFilePath, new FileSystemFileReader());

        }
        public static StyleObject Url(string resourceFilePath, IFileReader fileReader) {

            if (fileReader is null)
                throw new ArgumentNullException(nameof(fileReader));

            string[] imageFileExtensions = new[] { ".bmp", ".gif", ".jpg", ".jpeg", ".jpe", ".jif", ".jfif", ".jfi", ".png", ".tiff", ".tif" };

            // Strip outer quotes from the path.

            resourceFilePath = resourceFilePath.Trim('"', '\'');

            string ext = Path.GetExtension(resourceFilePath);

            if (imageFileExtensions.Any(imageExt => ext.Equals(imageExt, StringComparison.OrdinalIgnoreCase))) {

                if (File.Exists(resourceFilePath)) {

                    using (Stream stream = fileReader.OpenFile(resourceFilePath))
                        return new StyleObject(new Image(System.Drawing.Image.FromStream(stream)));

                }
                else
                    return new StyleObject(Image.Empty);

            }
            else {

                throw new ArgumentException(nameof(resourceFilePath));

            }

        }

        public static StyleObject EvaluateFunction(string functionName, StyleObject[] functionArgs) {

            return EvaluateFunction(functionName, functionArgs, new FileSystemFileReader());

        }
        public static StyleObject EvaluateFunction(string functionName, StyleObject[] functionArgs, IFileReader fileReader) {

            functionName = functionName?.Trim().ToLowerInvariant();

            switch (functionName) {

                case "linear-gradient":
                    return new StyleObject(LinearGradient(functionArgs[0].GetNumber(), functionArgs.Skip(1).Select(obj => obj.GetColor()).ToArray()));

                case "rgb":
                    return new StyleObject(Rgb((int)functionArgs[0].GetNumber(), (int)functionArgs[1].GetNumber(), (int)functionArgs[2].GetNumber()));

                case "rgba":
                    return new StyleObject(Rgba((int)functionArgs[0].GetNumber(), (int)functionArgs[1].GetNumber(), (int)functionArgs[2].GetNumber(), (float)functionArgs[3].GetNumber()));

                case "url":
                    return new StyleObject(Url(functionArgs[0].GetString(), fileReader));

                default:
                    throw new InvalidFunctionException(functionName);

            }

        }

        public static string ToString(BorderStyle input) {

            switch (input) {

                case BorderStyle.Dotted:
                    return "dotted";

                case BorderStyle.Dashed:
                    return "dashed";

                case BorderStyle.Solid:
                    return "solid";

                case BorderStyle.Double:
                    return "double";

                case BorderStyle.Groove:
                    return "groove";

                case BorderStyle.Ridge:
                    return "ridge";

                case BorderStyle.Inset:
                    return "inset";

                case BorderStyle.Outset:
                    return "outset";

                case BorderStyle.None:
                    return "none";

                case BorderStyle.Hidden:
                    return "hidden";

                default:
                    throw new ArgumentException(nameof(input));

            }

        }

        // Private members

        private static bool TryParseAngle(string input, out double result) {

            input = input?.Trim().ToLowerInvariant();

            Regex stringAngleRegex = new Regex(@"to\s*(bottom(?:\s*(?:right|left))?|top(?:\s*(?:right|left)?)|left|right)$", RegexOptions.IgnoreCase);
            Regex numericAngleRegex = new Regex(@"(-?[\d.]+)(deg|g?rad|turn)$", RegexOptions.IgnoreCase);

            Match stringAngleMatch = stringAngleRegex.Match(input);
            Match numericAngleMatch = stringAngleMatch.Success ? null : numericAngleRegex.Match(input);

            bool success;

            if (stringAngleMatch.Success) {

                success = true;

                switch (stringAngleMatch.Groups[1].Value) {

                    case "bottom right":
                        result = 135.0;
                        break;

                    case "bottom left":
                        result = 225.0;
                        break;

                    case "top right":
                        result = 45.0;
                        break;

                    case "top left":
                        result = 315.0;
                        break;

                    case "bottom":
                        result = 180.0;
                        break;

                    case "top":
                        result = 0.0;
                        break;

                    case "left":
                        result = 270.0;
                        break;

                    case "right":
                        result = 90.0;
                        break;

                    default:
                        result = default;
                        success = false;
                        break;

                }

            }
            else if (numericAngleMatch.Success) {

                if (!double.TryParse(numericAngleMatch.Groups[1].Value, out result))
                    return false;

                success = true;

                string units = numericAngleMatch.Groups[2].Value;

                switch (units) {

                    case "deg":
                        break;

                    case "rad":
                        result = result * 180.0 / Math.PI;
                        break;

                    case "grad":
                        result = result * 180.0 / 200.0;
                        break;

                    case "turn":
                        result *= 360.0;
                        break;

                    default:
                        result = default;
                        success = false;
                        break;

                }

            }
            else {

                result = default;
                success = false;

            }

            return success;

        }

    }

}