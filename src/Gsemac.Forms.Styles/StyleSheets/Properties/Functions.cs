using Gsemac.Forms.Styles.Properties;
using Gsemac.Forms.Styles.StyleSheets.Properties.Extensions;
using System;
using System.Drawing;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    internal static class Functions {

        // Public members

        public static IPropertyValue EvaluateFunction(string functionName, IPropertyValue[] arguments) {

            functionName = functionName?.Trim().ToLowerInvariant();

            switch (functionName) {

                case FunctionName.LinearGradient:
                    return PropertyValue.Create(LinearGradient(arguments));

                case FunctionName.Rgb:
                    return PropertyValue.Create(Rgb(arguments));

                case FunctionName.Rgba:
                    return PropertyValue.Create(Rgba(arguments));

                case FunctionName.Url:
                    return PropertyValue.Create(Url(arguments));

                case FunctionName.Var:
                    return PropertyValue.Create(Var(arguments));

                default:
                    throw new InvalidFunctionException(functionName);

            }

        }

        public static ILinearGradient LinearGradient(IPropertyValue[] arguments) {

            const int minimumArguments = 2;

            if (arguments.Length < minimumArguments)
                throw new ArgumentException(string.Format(ExceptionMessages.PropertyFunctionRequiresNArguments, FunctionName.LinearGradient, minimumArguments, arguments.Length));

            IDimension angle = arguments[0].As<IDimension>();
            Color[] colors = arguments.Skip(1).Select(arg => arg.As<Color>()).ToArray();

            ILinearGradient gradient = new LinearGradient(angle.ToDegrees(), colors);

            return gradient;

        }
        public static Color Rgb(IPropertyValue[] arguments) {

            const int requiredArguments = 3;

            if (arguments.Length != requiredArguments)
                throw new ArgumentException(string.Format(ExceptionMessages.PropertyFunctionRequiresNArguments, FunctionName.LinearGradient, requiredArguments, arguments.Length));

            int r = arguments[0].As<int>();
            int g = arguments[1].As<int>();
            int b = arguments[2].As<int>();

            Color color = Color.FromArgb(r, g, b);

            return color;

        }
        public static Color Rgba(IPropertyValue[] arguments) {

            const int requiredArguments = 4;

            if (arguments.Length != requiredArguments)
                throw new ArgumentException(string.Format(ExceptionMessages.PropertyFunctionRequiresNArguments, FunctionName.LinearGradient, requiredArguments, arguments.Length));

            int r = arguments[0].As<int>();
            int g = arguments[1].As<int>();
            int b = arguments[2].As<int>();
            float alpha = arguments[3].As<float>();

            Color color = Color.FromArgb((int)Math.Round(byte.MaxValue * alpha), r, g, b); ;

            return color;

        }
        public static string Url(IPropertyValue[] arguments) {

            const int requiredArguments = 1;

            if (arguments.Length != requiredArguments)
                throw new ArgumentException(string.Format(ExceptionMessages.PropertyFunctionRequiresNArguments, FunctionName.LinearGradient, requiredArguments, arguments.Length));

            string resourcePath = arguments[0].As<string>();

            // Strip outer quotes from the path.

            resourcePath = resourcePath.Trim('"', '\'');

            return resourcePath;

        }
        public static VariableReference Var(IPropertyValue[] arguments) {

            const int requiredArguments = 1;

            if (arguments.Length != requiredArguments)
                throw new ArgumentException(string.Format(ExceptionMessages.PropertyFunctionRequiresNArguments, FunctionName.Var, requiredArguments, arguments.Length));

            return new VariableReference(arguments[0].As<string>());

        }

    }

}