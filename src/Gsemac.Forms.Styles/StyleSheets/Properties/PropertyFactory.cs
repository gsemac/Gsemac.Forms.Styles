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

        public IProperty Create(string propertyName, IPropertyValue[] arguments) {

            switch (propertyName) {

                case PropertyName.Border:
                    return CreateBorderProperty(propertyName, arguments);

                case PropertyName.BackgroundColor:
                case PropertyName.BorderBottomColor:
                case PropertyName.BorderColor:
                case PropertyName.BorderLeftColor:
                case PropertyName.BorderRightColor:
                case PropertyName.BorderTopColor:
                case PropertyName.Color:
                    return CreateColorProperty(propertyName, arguments);

                case PropertyName.BorderRadius:
                    return CreateBorderRadiusProperty(propertyName, arguments);

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
                    return CreateBackgroundImageProperty(propertyName, arguments);

                default:
                    throw new InvalidPropertyException(string.Format(ExceptionMessages.UnrecognizedProperty, propertyName));

            }

        }

        // Private members

        private static Property<BackgroundImage> CreateBackgroundImageProperty(string propertyName, IPropertyValue[] values) {

            if (values is null || values.Length <= 0)
                return new Property<BackgroundImage>(propertyName);

            return new Property<BackgroundImage>(propertyName, CreateBackgroundImage(values));

        }
        private static BorderProperty CreateBorderProperty(string propertyName, IPropertyValue[] values) {

            if (values is null || values.Length <= 0)
                return new BorderProperty();

            return new BorderProperty(CreateBorder(values));

        }
        private static Property<BorderRadius> CreateBorderRadiusProperty(string propertyName, IPropertyValue[] values) {

            if (values is null || values.Length <= 0)
                return new Property<BorderRadius>(propertyName);

            return new Property<BorderRadius>(propertyName, CreateBorderRadius(values));

        }
        private static Property<BorderStyle> CreateBorderStyleProperty(string propertyName, IPropertyValue[] values) {

            if (values is null || values.Length <= 0)
                return new Property<BorderStyle>(propertyName);

            return new Property<BorderStyle>(propertyName, values.First().As<BorderStyle>());

        }
        private static Property<Color> CreateColorProperty(string propertyName, IPropertyValue[] values) {

            if (values is null || values.Length <= 0)
                return new Property<Color>(propertyName);

            return new Property<Color>(propertyName, values.First().As<Color>());

        }
        private static Property<IMeasurement> CreateMeasurementProperty(string propertyName, IPropertyValue[] values) {

            if (values is null || values.Length <= 0)
                return new Property<IMeasurement>(propertyName);

            return new Property<IMeasurement>(propertyName, values.First().As<IMeasurement>());

        }

        private static BackgroundImage CreateBackgroundImage(IPropertyValue[] values) {

            return new BackgroundImage(values.Select(v => v.As<IImage>()));

        }
        private static Border CreateBorder(IPropertyValue[] values) {

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
        private static BorderRadius CreateBorderRadius(IPropertyValue[] values) {

            if (values.Count() == 4)
                return new BorderRadius(values[0].As<double>(), values[1].As<double>(), values[2].As<double>(), values[3].As<double>());
            else if (values.Count() == 3)
                return new BorderRadius(values[0].As<double>(), values[1].As<double>(), values[2].As<double>());
            else if (values.Count() == 2)
                return new BorderRadius(values[0].As<double>(), values[1].As<double>());
            else
                return new BorderRadius(values[0].As<double>());

        }

    }

}