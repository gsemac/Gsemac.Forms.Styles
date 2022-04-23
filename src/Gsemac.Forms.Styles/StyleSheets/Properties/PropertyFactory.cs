using Gsemac.Forms.Styles.Properties;
using Gsemac.Forms.Styles.StyleSheets.Properties.Extensions;
using System;
using System.Drawing;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class PropertyFactory :
        IPropertyFactory {

        // Public members

        public static PropertyFactory Default => new PropertyFactory();

        public PropertyFactory() :
            this(PropertyInitialValueFactory.Default) {
        }
        public PropertyFactory(IPropertyInitialValueFactory initialValueFactory) {

            if (initialValueFactory is null)
                throw new ArgumentNullException(nameof(initialValueFactory));

            this.initialValueFactory = initialValueFactory;

        }

        public IProperty Create(string propertyName, IPropertyValue[] arguments) {

            switch (propertyName) {

                case PropertyName.Border:
                    return CreateBorderProperty(arguments);

                case PropertyName.BackgroundColor:
                case PropertyName.BorderBottomColor:
                case PropertyName.BorderColor:
                case PropertyName.BorderLeftColor:
                case PropertyName.BorderRightColor:
                case PropertyName.BorderTopColor:
                case PropertyName.Color:
                    return CreatePropertyFromSingleArgument<Color>(propertyName, arguments);

                case PropertyName.BorderRadius:
                    return CreateBorderRadiusProperty(arguments);

                case PropertyName.BorderBottomLeftRadius:
                case PropertyName.BorderBottomRightRadius:
                case PropertyName.BorderTopLeftRadius:
                case PropertyName.BorderTopRightRadius:
                    return CreatePropertyFromSingleArgument<ILengthOrPercentage>(propertyName, arguments);

                case PropertyName.BorderBottomWidth:
                case PropertyName.BorderLeftWidth:
                case PropertyName.BorderRightWidth:
                case PropertyName.BorderTopWidth:
                case PropertyName.BorderWidth:
                    return CreatePropertyFromSingleArgument<Length>(propertyName, arguments);

                case PropertyName.BorderBottomStyle:
                case PropertyName.BorderLeftStyle:
                case PropertyName.BorderRightStyle:
                case PropertyName.BorderStyle:
                case PropertyName.BorderTopStyle:
                    return CreatePropertyFromSingleArgument<BorderStyle>(propertyName, arguments);

                case PropertyName.BackgroundImage:
                    return CreateBackgroundImageProperty(arguments);

                case PropertyName.Opacity:
                    return CreatePropertyFromSingleArgument<double>(propertyName, arguments);

                default:
                    throw new InvalidPropertyException(string.Format(ExceptionMessages.InvalidPropertyName, propertyName));

            }

        }

        // Private members

        private readonly IPropertyInitialValueFactory initialValueFactory;

        private Property<T> CreatePropertyWithDefaultValue<T>(string propertyName) {

            T value = initialValueFactory.GetInitialValue<T>(propertyName);
            bool isInheritable = IsInheritable(propertyName);

            return new Property<T>(propertyName, value, isInheritable);

        }
        private Property<T> CreatePropertyFromSingleArgument<T>(string propertyName, IPropertyValue[] arguments) {

            if (arguments is null || arguments.Length <= 0)
                return CreatePropertyWithDefaultValue<T>(propertyName);

            T value = arguments.First().As<T>();
            bool isInheritable = IsInheritable(propertyName);

            return new Property<T>(propertyName, value, isInheritable);

        }

        private Property<BackgroundImage> CreateBackgroundImageProperty(IPropertyValue[] arguments) {

            if (arguments is null || arguments.Length <= 0)
                return CreatePropertyWithDefaultValue<BackgroundImage>(PropertyName.BackgroundImage);

            return new Property<BackgroundImage>(PropertyName.BackgroundImage,
                CreateBackgroundImage(arguments),
                IsInheritable(PropertyName.BackgroundImage));

        }
        private BorderProperty CreateBorderProperty(IPropertyValue[] arguments) {

            if (arguments is null || arguments.Length <= 0)
                return new BorderProperty(initialValueFactory.GetInitialValue<Border>(PropertyName.Border));

            return new BorderProperty(CreateBorder(arguments));

        }
        private BorderRadiusProperty CreateBorderRadiusProperty(IPropertyValue[] arguments) {

            if (arguments is null || arguments.Length <= 0)
                return new BorderRadiusProperty(initialValueFactory.GetInitialValue<BorderRadius>(PropertyName.BorderRadius));

            return new BorderRadiusProperty(CreateBorderRadius(arguments));

        }

        private static bool TryGetValueAsTypeDirectly<T>(IPropertyValue[] arguments, out T value) {

            value = default;

            if (arguments.Length == 1 && arguments.First().TryAs(out T valueAsT)) {

                value = valueAsT;

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
        private static BorderRadius CreateBorderRadius(IPropertyValue[] arguments) {

            if (TryGetValueAsTypeDirectly(arguments, out BorderRadius value))
                return value;

            Length[] measurments = arguments
                .Select(arg => arg.As<Length>())
                .ToArray();

            if (measurments.Count() == 4)
                return new BorderRadius(measurments[0], measurments[1], measurments[2], measurments[3]);
            else if (measurments.Count() == 3)
                return new BorderRadius(measurments[0], measurments[1], measurments[2]);
            else if (measurments.Count() == 2)
                return new BorderRadius(measurments[0], measurments[1]);
            else
                return new BorderRadius(measurments[0]);

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

    }

}