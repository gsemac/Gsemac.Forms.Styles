using Gsemac.Forms.Styles.Properties;
using Gsemac.Forms.Styles.StyleSheets.Properties.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Drawing;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public abstract class PropertyFactoryBase :
        IPropertyFactory {

        // Public members

        public IProperty Create(string propertyName, IPropertyValue[] arguments, IRuleset ruleset) {

            if (propertyName is null)
                throw new ArgumentNullException(nameof(propertyName));

            if (ruleset is null)
                throw new ArgumentNullException(nameof(ruleset));

            propertyName = propertyName.Trim();

            // Check if we're defining a variable (using the "--" prefix).
            // Variable names are case-sensitive.

            if (propertyName.StartsWith(VariablePrefix))
                return CreatePropertyFromSingleArgument<string>(propertyName, arguments, ruleset);

            // Standard property names are not case-sensitive.

            switch (propertyName.ToLowerInvariant()) {

                case PropertyName.Border:
                    return CreateBorderProperty(arguments, ruleset);

                case PropertyName.AccentColor:
                case PropertyName.BackgroundColor:
                case PropertyName.BorderBottomColor:
                case PropertyName.BorderColor:
                case PropertyName.BorderLeftColor:
                case PropertyName.BorderRightColor:
                case PropertyName.BorderTopColor:
                case PropertyName.Color:
                    return CreatePropertyFromSingleArgument<Color>(propertyName, arguments, ruleset);

                case PropertyName.BorderRadius:
                    return CreateBorderRadiusProperty(arguments, ruleset);

                case PropertyName.BorderBottomLeftRadius:
                case PropertyName.BorderBottomRightRadius:
                case PropertyName.BorderTopLeftRadius:
                case PropertyName.BorderTopRightRadius:
                    return CreatePropertyFromSingleArgument<ILengthPercentage>(propertyName, arguments, ruleset);

                case PropertyName.BorderBottomWidth:
                case PropertyName.BorderLeftWidth:
                case PropertyName.BorderRightWidth:
                case PropertyName.BorderTopWidth:
                    return CreatePropertyFromSingleArgument<LineWidth>(propertyName, arguments, ruleset);

                case PropertyName.BorderBottomStyle:
                case PropertyName.BorderLeftStyle:
                case PropertyName.BorderRightStyle:
                case PropertyName.BorderStyle:
                case PropertyName.BorderTopStyle:
                    return CreatePropertyFromSingleArgument<BorderStyle>(propertyName, arguments, ruleset);

                case PropertyName.BorderWidth:
                    return CreateBorderWidthProperty(arguments, ruleset);

                case PropertyName.BackgroundImage:
                    return CreateBackgroundImageProperty(arguments, ruleset);

                case PropertyName.Opacity:
                    return CreatePropertyFromSingleArgument<double>(propertyName, arguments, ruleset);

                default:

                    if (!options.AllowUnknownProperties)
                        throw new InvalidPropertyException(string.Format(ExceptionMessages.InvalidPropertyName, propertyName));

                    return CreateUnknownProperty(propertyName, arguments);

            }

        }

        // Protected members

        protected PropertyFactoryBase() :
            this(PropertyFactoryOptions.Default) {
        }
        protected PropertyFactoryBase(IPropertyFactoryOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            this.options = options;

        }

        protected virtual IPropertyValue GetInitialValue(string propertyName, IRuleset ruleset) {

            if (string.IsNullOrWhiteSpace(propertyName))
                return null;

            switch (propertyName.ToLowerInvariant()) {

                case PropertyName.AccentColor:
                    return PropertyValue.Auto;

                case PropertyName.BackgroundImage:
                    return PropertyValue.Create(new BackgroundImage());

                case PropertyName.BackgroundColor:
                    return PropertyValue.Create(Color.Transparent);

                case PropertyName.Border:
                    return PropertyValue.Create(new Border(ruleset.Color));

                case PropertyName.BorderTopColor:
                case PropertyName.BorderRightColor:
                case PropertyName.BorderBottomColor:
                case PropertyName.BorderLeftColor:
                    return PropertyValue.Create(ruleset.Color);

                case PropertyName.BorderTopStyle:
                case PropertyName.BorderRightStyle:
                case PropertyName.BorderBottomStyle:
                case PropertyName.BorderLeftStyle:
                    return PropertyValue.Create(BorderStyle.None);

                case PropertyName.BorderTopWidth:
                case PropertyName.BorderRightWidth:
                case PropertyName.BorderBottomWidth:
                case PropertyName.BorderLeftWidth:
                    return PropertyValue.Create(Length.Zero);

                case PropertyName.Color:
                    return PropertyValue.Create(Color.Black); // Depends on user agent

                case PropertyName.Opacity:
                    return PropertyValue.Create(1.0);

                default:
                    return PropertyValue.Null;

            }

        }
        protected T GetInitialValue<T>(string propertyName, IRuleset ruleset) {

            if (ruleset is null)
                throw new ArgumentNullException(nameof(ruleset));

            IPropertyValue initialValue = GetInitialValue(propertyName, ruleset);

            if (initialValue is object && initialValue.Type.Equals(typeof(T)))
                return (T)initialValue.Value;

            return default;

        }

        // Private members

        private const string VariablePrefix = "--";

        private readonly IPropertyFactoryOptions options;

        private IProperty CreatePropertyWithDefaultValue<T>(string propertyName, IRuleset ruleset) {

            if (ruleset is null)
                throw new ArgumentNullException(nameof(ruleset));

            T value = GetInitialValue<T>(propertyName, ruleset);
            bool isInheritable = IsInheritable(propertyName);

            return Property.Create(propertyName, value, isInheritable);

        }
        private IProperty CreatePropertyFromSingleArgument<T>(string propertyName, IPropertyValue[] arguments, IRuleset ruleset) {

            if (ruleset is null)
                throw new ArgumentNullException(nameof(ruleset));

            // Treat "initial" as a string if we're initializing a variable.

            bool propertyIsVariable = propertyName.StartsWith(VariablePrefix);

            if (arguments is null || arguments.Length <= 0 || (!propertyIsVariable && arguments[0].Equals(PropertyValue.Initial)))
                return CreatePropertyWithDefaultValue<T>(propertyName, ruleset);

            bool isInheritable = IsInheritable(propertyName);

            if (IsVariableReference(arguments)) {

                VariableReference value = arguments[0].As<VariableReference>();

                return Property.Create(propertyName, value, isInheritable);

            }
            else {

                T value = arguments.First().As<T>();

                return Property.Create(propertyName, value, isInheritable);

            }

        }
        private IProperty CreateUnknownProperty(string propertyName, IPropertyValue[] arguments) {

            // Unknown properties will be treated in a very basic fashion because we don't know anything about how they should be initialized.
            // For now, complex custom properties should be handled by creating a custom PropertyFactory implementation.

            return new Property(propertyName, arguments.FirstOrDefault());

        }

        private IProperty CreateBackgroundImageProperty(IPropertyValue[] arguments, IRuleset ruleset) {

            if (ruleset is null)
                throw new ArgumentNullException(nameof(ruleset));

            if (arguments is null || arguments.Length <= 0)
                return CreatePropertyWithDefaultValue<BackgroundImage>(PropertyName.BackgroundImage, ruleset);

            return Property.Create(PropertyName.BackgroundImage,
                CreateBackgroundImage(arguments),
                IsInheritable(PropertyName.BackgroundImage));

        }
        private IProperty CreateBorderProperty(IPropertyValue[] arguments, IRuleset ruleset) {

            if (ruleset is null)
                throw new ArgumentNullException(nameof(ruleset));

            if (arguments is null || arguments.Length <= 0)
                return new BorderProperty(GetInitialValue<Border>(PropertyName.Border, ruleset));

            return new BorderProperty(CreateBorder(arguments));

        }
        private IProperty CreateBorderRadiusProperty(IPropertyValue[] arguments, IRuleset ruleset) {

            if (ruleset is null)
                throw new ArgumentNullException(nameof(ruleset));

            if (arguments is null || arguments.Length <= 0)
                return new BorderRadiusProperty(GetInitialValue<BorderRadii>(PropertyName.BorderRadius, ruleset));

            return new BorderRadiusProperty(CreateBorderRadius(arguments));

        }
        private IProperty CreateBorderWidthProperty(IPropertyValue[] arguments, IRuleset ruleset) {

            if (ruleset is null)
                throw new ArgumentNullException(nameof(ruleset));

            if (arguments is null || arguments.Length <= 0)
                return new BorderWidthProperty(GetInitialValue<BorderWidths>(PropertyName.BorderWidth, ruleset));

            return new BorderWidthProperty(CreateBorderWidth(arguments));

        }

        private static bool TryGetValueAsTypeDirectly<T>(IPropertyValue[] arguments, out T value) {

            value = default;

            if (arguments.Length == 1 && arguments.First().Is<T>()) {

                value = (T)arguments.First().Value;

                return true;

            }

            return false;

        }

        private static BackgroundImage CreateBackgroundImage(IPropertyValue[] arguments) {

            if (TryGetValueAsTypeDirectly(arguments, out BackgroundImage value))
                return value;

            return new BackgroundImage(arguments.Select(v => v.As<IImage>()));

        }
        private static Border CreateBorder(IPropertyValue[] arguments) {

            if (TryGetValueAsTypeDirectly(arguments, out Border value))
                return value;

            // TODO: Implement this

            throw new NotImplementedException();

            //// At least a border style MUST be specified.
            //// Arguments are allowed to occur in any order.

            //BorderStyle? borderStyle = values.Select(value => value.GetString())
            //    .Select(value => PropertyUtilities.TryParseBorderStyle(value, out BorderStyle result) ? (BorderStyle?)result : null)
            //    .Where(result => result != null)
            //    .FirstOrDefault();

            //if (!borderStyle.HasValue)
            //    throw new ArgumentException(nameof(values));

            //double borderWidth = values.Select(value => value.GetString())
            //    .Select(value => PropertyUtilities.TryParseNumber(value, out double result) ? (double?)result : null)
            //    .Where(result => result != null)
            //    .FirstOrDefault() ?? 0.0;

            //Color borderColor = values.Select(value => value.GetString())
            //   .Select(value => PropertyUtilities.TryParseColor(value, out Color result) ? (Color?)result : null)
            //   .Where(result => result != null)
            //   .FirstOrDefault() ?? default;

            //return new Border(borderWidth, borderStyle.Value, borderColor);

        }
        private static BorderRadii CreateBorderRadius(IPropertyValue[] arguments) {

            if (TryGetValueAsTypeDirectly(arguments, out BorderRadii value))
                return value;

            Length[] measurments = arguments
                .Select(arg => arg.As<Length>())
                .ToArray();

            if (measurments.Count() == 4)
                return new BorderRadii(measurments[0], measurments[1], measurments[2], measurments[3]);
            else if (measurments.Count() == 3)
                return new BorderRadii(measurments[0], measurments[1], measurments[2]);
            else if (measurments.Count() == 2)
                return new BorderRadii(measurments[0], measurments[1]);
            else
                return new BorderRadii(measurments[0]);

        }
        private static BorderWidths CreateBorderWidth(IPropertyValue[] arguments) {

            if (TryGetValueAsTypeDirectly(arguments, out BorderWidths value))
                return value;

            LineWidth[] measurments = arguments
                .Select(arg => arg.As<LineWidth>())
                .ToArray();

            if (measurments.Count() == 4)
                return new BorderWidths(measurments[0], measurments[1], measurments[2], measurments[3]);
            else if (measurments.Count() == 3)
                return new BorderWidths(measurments[0], measurments[1], measurments[2]);
            else if (measurments.Count() == 2)
                return new BorderWidths(measurments[0], measurments[1]);
            else
                return new BorderWidths(measurments[0]);

        }

        private static bool IsInheritable(string propertyName) {

            // https://stackoverflow.com/a/30536051/5383169 (David Bonnet)

            if (propertyName is null)
                return false;

            switch (propertyName.ToLowerInvariant()) {

                case PropertyName.BorderCollapse:
                case PropertyName.BorderSpacing:
                case PropertyName.CaptionSide:
                case PropertyName.Color:
                case PropertyName.Cursor:
                case PropertyName.Direction:
                case PropertyName.EmptyCells:
                case PropertyName.FontFamily:
                case PropertyName.FontSize:
                case PropertyName.FontStyle:
                case PropertyName.FontVariant:
                case PropertyName.FontWeight:
                case PropertyName.FontSizeAdjust:
                case PropertyName.FontStretch:
                case PropertyName.Font:
                case PropertyName.LetterSpacing:
                case PropertyName.LineHeight:
                case PropertyName.ListStyleImage:
                case PropertyName.ListStylePosition:
                case PropertyName.ListStyleType:
                case PropertyName.ListStyle:
                case PropertyName.Orphans:
                case PropertyName.Quotes:
                case PropertyName.TabSize:
                case PropertyName.TextAlign:
                case PropertyName.TextAlignLast:
                case PropertyName.TextDecorationColor:
                case PropertyName.TextIndent:
                case PropertyName.TextJustify:
                case PropertyName.TextShadow:
                case PropertyName.TextTransform:
                case PropertyName.Visibility:
                case PropertyName.WhiteSpace:
                case PropertyName.Widows:
                case PropertyName.WordBreak:
                case PropertyName.WordSpacing:
                case PropertyName.WordWrap:
                    return true;

                default:
                    return false;

            }

        }
        private static bool IsVariableReference(IPropertyValue[] arguments) {

            return arguments.Length == 1 &&
                arguments[0].Is<VariableReference>();

        }

    }

}