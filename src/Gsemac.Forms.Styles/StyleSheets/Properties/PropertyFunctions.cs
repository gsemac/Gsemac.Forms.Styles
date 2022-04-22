using Gsemac.Forms.Styles.Properties;
using Gsemac.Forms.Styles.StyleSheets.Properties.Extensions;
using System;
using System.Drawing;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    internal static class PropertyFunctions {

        // Public members

        public static IPropertyValue EvaluateFunction(string functionName, IPropertyValue[] arguments) {

            functionName = functionName?.Trim().ToLowerInvariant();

            switch (functionName) {

                case PropertyFunctionName.LinearGradient:
                    return LinearGradient(arguments);

                case PropertyFunctionName.Rgb:
                    return Rgb(arguments);

                case PropertyFunctionName.Rgba:
                    return Rgba(arguments);

                case PropertyFunctionName.Url:
                    return Url(arguments);

                default:
                    throw new InvalidFunctionException(functionName);

            }

        }

        public static IPropertyValue<ILinearGradient> LinearGradient(IPropertyValue[] arguments) {

            const int minimumArguments = 2;

            if (arguments.Length < minimumArguments)
                throw new ArgumentException(string.Format(ExceptionMessages.PropertyFunctionRequiresNArguments, PropertyFunctionName.LinearGradient, minimumArguments, arguments.Length));

            IDimension angle = arguments[0].As<IDimension>();
            Color[] colors = arguments.Skip(1).Select(arg => arg.As<Color>()).ToArray();

            ILinearGradient gradient = new LinearGradient(angle.ToDegrees(), colors);

            return PropertyValue.Create(gradient);

        }
        public static IPropertyValue<Color> Rgb(IPropertyValue[] arguments) {

            const int requiredArguments = 3;

            if (arguments.Length != requiredArguments)
                throw new ArgumentException(string.Format(ExceptionMessages.PropertyFunctionRequiresNArguments, PropertyFunctionName.LinearGradient, requiredArguments, arguments.Length));

            int r = arguments[0].As<int>();
            int g = arguments[1].As<int>();
            int b = arguments[2].As<int>();

            Color color = Color.FromArgb(r, g, b);

            return PropertyValue.Create(color);

        }
        public static IPropertyValue<Color> Rgba(IPropertyValue[] arguments) {

            const int requiredArguments = 4;

            if (arguments.Length != requiredArguments)
                throw new ArgumentException(string.Format(ExceptionMessages.PropertyFunctionRequiresNArguments, PropertyFunctionName.LinearGradient, requiredArguments, arguments.Length));

            int r = arguments[0].As<int>();
            int g = arguments[1].As<int>();
            int b = arguments[2].As<int>();
            float alpha = arguments[3].As<float>();

            Color color = Color.FromArgb((int)Math.Round(byte.MaxValue * alpha), r, g, b); ;

            return PropertyValue.Create(color);

        }
        public static IPropertyValue<string> Url(IPropertyValue[] arguments) {

            const int requiredArguments = 1;

            if (arguments.Length != requiredArguments)
                throw new ArgumentException(string.Format(ExceptionMessages.PropertyFunctionRequiresNArguments, PropertyFunctionName.LinearGradient, requiredArguments, arguments.Length));

            string resourcePath = arguments[0].As<string>();

            // Strip outer quotes from the path.

            resourcePath = resourcePath.Trim('"', '\'');

            return PropertyValue.Create(resourcePath);

        }

    }

}