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
                    return CreateColorProperty(propertyName, arguments);

                case PropertyName.BorderRadius:
                    return CreateBorderRadiusProperty(arguments);

                case PropertyName.BorderBottomLeftRadius:
                case PropertyName.BorderBottomRightRadius:
                case PropertyName.BorderBottomWidth:
                case PropertyName.BorderLeftWidth:
                case PropertyName.BorderRightWidth:
                case PropertyName.BorderTopLeftRadius:
                case PropertyName.BorderTopRightRadius:
                case PropertyName.BorderTopWidth:
                case PropertyName.BorderWidth:
                case PropertyName.Opacity:
                    return CreateMeasurementProperty(propertyName, arguments);

                case PropertyName.BorderBottomStyle:
                case PropertyName.BorderLeftStyle:
                case PropertyName.BorderRightStyle:
                case PropertyName.BorderStyle:
                case PropertyName.BorderTopStyle:
                    return CreateBorderStyleProperty(propertyName, arguments);

                case PropertyName.BackgroundImage:
                    return CreateBackgroundImageProperty(arguments);

                default:
                    throw new InvalidPropertyException(string.Format(ExceptionMessages.UnrecognizedProperty, propertyName));

            }

        }

        // Private members

        private readonly IPropertyInitialValueFactory initialValueFactory;

        private Property<T> CreateDefaultProperty<T>(string propertyName) {

            return new Property<T>(propertyName, initialValueFactory.GetInitialValue<T>(propertyName), PropertyUtilities.IsInheritable(propertyName));

        }

        private Property<BackgroundImage> CreateBackgroundImageProperty(IPropertyValue[] arguments) {

            if (arguments is null || arguments.Length <= 0)
                return CreateDefaultProperty<BackgroundImage>(PropertyName.BackgroundImage);

            return new Property<BackgroundImage>(PropertyName.BackgroundImage,
                CreateBackgroundImage(arguments),
                PropertyUtilities.IsInheritable(PropertyName.BackgroundImage));

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
        private Property<BorderStyle> CreateBorderStyleProperty(string propertyName, IPropertyValue[] arguments) {

            if (arguments is null || arguments.Length <= 0)
                return CreateDefaultProperty<BorderStyle>(propertyName);

            return new Property<BorderStyle>(propertyName,
                arguments.First().As<BorderStyle>(),
                PropertyUtilities.IsInheritable(propertyName));

        }
        private Property<Color> CreateColorProperty(string propertyName, IPropertyValue[] arguments) {

            if (arguments is null || arguments.Length <= 0)
                return CreateDefaultProperty<Color>(propertyName);

            return new Property<Color>(propertyName,
                arguments.First().As<Color>(),
                PropertyUtilities.IsInheritable(propertyName));

        }
        private Property<IDimension> CreateMeasurementProperty(string propertyName, IPropertyValue[] arguments) {

            if (arguments is null || arguments.Length <= 0)
                return CreateDefaultProperty<IDimension>(propertyName);

            return new Property<IDimension>(propertyName,
                arguments.First().As<IDimension>(),
                PropertyUtilities.IsInheritable(propertyName));

        }

        private static bool TryCreateValueDirectly<T>(IPropertyValue[] arguments, out T value) {

            value = default;

            if (arguments.Length == 1 && arguments.First().TryAs(out T valueAsT)) {

                value = valueAsT;

                return true;

            }

            return false;

        }

        private static BackgroundImage CreateBackgroundImage(IPropertyValue[] arguments) {

            if (TryCreateValueDirectly(arguments, out BackgroundImage value))
                return value;

            return new BackgroundImage(arguments.Select(v => v.As<IImage>()));

        }
        private static Border CreateBorder(IPropertyValue[] arguments) {

            if (TryCreateValueDirectly(arguments, out Border value))
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

            if (TryCreateValueDirectly(arguments, out BorderRadius value))
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

    }

}