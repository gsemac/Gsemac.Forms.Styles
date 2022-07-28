using Gsemac.Forms.Styles.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static Gsemac.Forms.Styles.StyleSheets.Properties.PropertyUtilities;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public abstract class PropertyFactoryBase :
        IPropertyFactory {

        // Public members

        public IProperty Create(string propertyName, IPropertyValue[] arguments) {

            if (propertyName is null)
                throw new ArgumentNullException(nameof(propertyName));

            if (arguments is null)
                arguments = Enumerable.Empty<IPropertyValue>().ToArray();

            propertyName = propertyName.Trim();

            // Check if we're defining a variable (using the "--" prefix).
            // Variable names are case-sensitive.

            if (propertyName.StartsWith(VariablePrefix))
                return CreateVariableProperty(propertyName, arguments);

            // Standard property names are not case-sensitive.

            if (definitions.TryGetValue(propertyName, out IPropertyDefinition definition))
                return Create(definition, arguments);

            if (!options.AllowUndefinedProperties)
                throw new InvalidPropertyException(string.Format(ExceptionMessages.InvalidPropertyName, propertyName));

            return CreateUndefinedProperty(propertyName, arguments);

        }

        // Protected members

        protected PropertyFactoryBase() :
            this(PropertyFactoryOptions.Default) {
        }
        protected PropertyFactoryBase(IPropertyFactoryOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            this.options = options;

            AddDefaultDefinitions();

        }

        protected void AddDefinition(IPropertyDefinition definition) {

            if (definition is null)
                throw new ArgumentNullException(nameof(definition));

            // Allow the same property to be defined multiple times.
            // This is because multiple properties might define the same shorthands (e.g. "border-top" and "border-width").

            definitions[definition.Name] = definition;

            foreach (IPropertyDefinition longhandDefinition in definition.Longhands)
                AddDefinition(longhandDefinition);

        }
        protected IProperty Create(IPropertyDefinition definition, IPropertyValue[] arguments) {

            if (definition is null)
                throw new ArgumentNullException(nameof(definition));

            if (arguments is null)
                arguments = Enumerable.Empty<IPropertyValue>().ToArray();

            IPropertyValue propertyValue;

            // We need to convert the given arguments into an object of type ValueType.

            if (arguments.Length <= 0) {

                // If we haven't been given any arguments, just set the value to the property's initial value.

                propertyValue = definition.InitialValue;

            }
            else if (arguments.Length == 1 && (arguments[0].Type.Equals(definition.ValueType) || IsVariableReference(arguments[0]) || IsKeyword(arguments[0]))) {

                // We have a single argument that is the exact type we need, so simply use the argument directly.

                propertyValue = arguments[0];

            }
            else if (definition.ValueType.Equals(typeof(BackgroundImage))) {

                propertyValue = PropertyValue.Create(CreateBackgroundImage(arguments));

            }
            else if (definition.ValueType.Equals(typeof(Border))) {

                propertyValue = PropertyValue.Create(CreateBorder(arguments));

            }
            else if (definition.ValueType.Equals(typeof(BorderColors))) {

                propertyValue = PropertyValue.Create(CreateBorderColors(arguments));

            }
            else if (definition.ValueType.Equals(typeof(Borders))) {

                propertyValue = PropertyValue.Create(CreateBorders(arguments));

            }
            else if (definition.ValueType.Equals(typeof(BorderRadii))) {

                propertyValue = PropertyValue.Create(CreateBorderRadius(arguments));

            }
            else if (definition.ValueType.Equals(typeof(BorderStyles))) {

                propertyValue = PropertyValue.Create(CreateBorderStyles(arguments));

            }
            else if (definition.ValueType.Equals(typeof(BorderWidths))) {

                propertyValue = PropertyValue.Create(CreateBorderWidths(arguments));

            }
            else if (arguments.Length == 1) {

                // We have a single argument, but its type is not the one we need.
                // Try to convert it to the desired type using the ValueConverter implementations.

                propertyValue = arguments[0].As(definition.ValueType);

            }
            else {

                // We have multiple arguments but no meaningful way to interpret them.

                throw new ArgumentException(string.Format(ExceptionMessages.InvalidPropertyArguments, definition.Name), nameof(arguments));

            }

            // Create the new property.

            return new Property(definition, propertyValue);

        }

        // Private members

        private class Property :
            PropertyBase {

            // Public members

            public Property(IPropertyDefinition definition, IPropertyValue value) :
                base(definition, value) {
            }

        }

        private const string VariablePrefix = "--";

        private readonly IPropertyFactoryOptions options;
        private readonly IDictionary<string, IPropertyDefinition> definitions = new Dictionary<string, IPropertyDefinition>(StringComparer.OrdinalIgnoreCase);

        private IPropertyDefinitionBuilder Define(string propertyName) {

            return new PropertyDefinitionBuilder(propertyName);

        }
        private void AddDefinition(IPropertyDefinitionBuilder builder) {

            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            AddDefinition(builder.Build());

        }
        private void AddDefaultDefinitions() {

            // Any properties where the initial value is controlled by the user agent will not have an initial value set.

            AddDefinition(Define(PropertyName.AccentColor)
                .WithType<Color>()
                .WithInitial(PropertyValue.Auto));

            AddDefinition(Define(PropertyName.BackgroundColor)
                .WithInitial(Color.Transparent));

            AddDefinition(Define(PropertyName.BackgroundImage)
                .WithType<BackgroundImage>()
                .WithInitial(PropertyValue.None));

            AddDefinition(Define(PropertyName.Border)
                .WithType<Borders>()
                .WithLonghand(PropertyName.BorderTop, (Borders p) => p.Top)
                    .WithLonghand(PropertyName.BorderTopWidth, (Border p) => p.Width).WithInitial(LineWidth.Medium).EndProperty()
                    .WithLonghand(PropertyName.BorderTopStyle, (Border p) => p.Style).WithInitial(BorderStyle.None).EndProperty()
                    .WithLonghand(PropertyName.BorderTopColor, (Border p) => p.Color).WithInitial(PropertyValue.CurrentColor).EndProperty()
                    .EndProperty()
              .WithLonghand(PropertyName.BorderRight, (Borders p) => p.Right)
                    .WithLonghand(PropertyName.BorderRightWidth, (Border p) => p.Width).WithInitial(LineWidth.Medium).EndProperty()
                    .WithLonghand(PropertyName.BorderRightStyle, (Border p) => p.Style).WithInitial(BorderStyle.None).EndProperty()
                    .WithLonghand(PropertyName.BorderRightColor, (Border p) => p.Color).WithInitial(PropertyValue.CurrentColor).EndProperty()
                    .EndProperty()
              .WithLonghand(PropertyName.BorderBottom, (Borders p) => p.Bottom)
                    .WithLonghand(PropertyName.BorderBottomWidth, (Border p) => p.Width).WithInitial(LineWidth.Medium).EndProperty()
                    .WithLonghand(PropertyName.BorderBottomStyle, (Border p) => p.Style).WithInitial(BorderStyle.None).EndProperty()
                    .WithLonghand(PropertyName.BorderBottomColor, (Border p) => p.Color).WithInitial(PropertyValue.CurrentColor).EndProperty()
                    .EndProperty()
              .WithLonghand(PropertyName.BorderLeft, (Borders p) => p.Left)
                    .WithLonghand(PropertyName.BorderLeftWidth, (Border p) => p.Width).WithInitial(LineWidth.Medium).EndProperty()
                    .WithLonghand(PropertyName.BorderLeftStyle, (Border p) => p.Style).WithInitial(BorderStyle.None).EndProperty()
                    .WithLonghand(PropertyName.BorderLeftColor, (Border p) => p.Color).WithInitial(PropertyValue.CurrentColor).EndProperty()
                    .EndProperty());

            AddDefinition(Define(PropertyName.BorderColor)
                .WithType<BorderColors>()
                .WithLonghand(PropertyName.BorderTopColor, (BorderColors p) => p.Top).WithInitial(PropertyValue.CurrentColor).EndProperty()
                .WithLonghand(PropertyName.BorderRightColor, (BorderColors p) => p.Right).WithInitial(PropertyValue.CurrentColor).EndProperty()
                .WithLonghand(PropertyName.BorderBottomColor, (BorderColors p) => p.Bottom).WithInitial(PropertyValue.CurrentColor).EndProperty()
                .WithLonghand(PropertyName.BorderLeftColor, (BorderColors p) => p.Left).WithInitial(PropertyValue.CurrentColor).EndProperty());

            AddDefinition(Define(PropertyName.BorderRadius)
                .WithInitial(new BorderRadii())
                .WithLonghand(PropertyName.BorderTopLeftRadius, (BorderRadii p) => p.TopLeft).WithInitial(Length.Zero).EndProperty()
                .WithLonghand(PropertyName.BorderTopRightRadius, (BorderRadii p) => p.TopRight).WithInitial(Length.Zero).EndProperty()
                .WithLonghand(PropertyName.BorderBottomRightRadius, (BorderRadii p) => p.BottomRight).WithInitial(Length.Zero).EndProperty()
                .WithLonghand(PropertyName.BorderBottomLeftRadius, (BorderRadii p) => p.BottomLeft).WithInitial(Length.Zero).EndProperty());

            AddDefinition(Define(PropertyName.BorderStyle)
                .WithType<BorderStyles>()
                .WithLonghand(PropertyName.BorderTopStyle, (BorderStyles p) => p.Top).WithInitial(BorderStyle.None).EndProperty()
                .WithLonghand(PropertyName.BorderRightStyle, (BorderStyles p) => p.Right).WithInitial(BorderStyle.None).EndProperty()
                .WithLonghand(PropertyName.BorderBottomStyle, (BorderStyles p) => p.Bottom).WithInitial(BorderStyle.None).EndProperty()
                .WithLonghand(PropertyName.BorderLeftStyle, (BorderStyles p) => p.Left).WithInitial(BorderStyle.None).EndProperty());

            AddDefinition(Define(PropertyName.BorderWidth)
                .WithType<BorderWidths>()
                .WithInitial(new BorderWidths())
                .WithLonghand(PropertyName.BorderTopWidth, (BorderWidths p) => p.Top).WithInitial(LineWidth.Medium).EndProperty()
                .WithLonghand(PropertyName.BorderRightWidth, (BorderWidths p) => p.Right).WithInitial(LineWidth.Medium).EndProperty()
                .WithLonghand(PropertyName.BorderBottomWidth, (BorderWidths p) => p.Bottom).WithInitial(LineWidth.Medium).EndProperty()
                .WithLonghand(PropertyName.BorderLeftWidth, (BorderWidths p) => p.Left).WithInitial(LineWidth.Medium).EndProperty());

            AddDefinition(Define(PropertyName.Color)
                .WithType<Color>()
                .WithInitial(PropertyValue.CanvasText));

            AddDefinition(Define(PropertyName.Opacity)
                .WithType<double>()
                .WithInitial(1.0));

        }

        private IProperty CreateVariableProperty(string propertyName, IPropertyValue[] arguments) {

            if (propertyName is null)
                throw new ArgumentNullException(nameof(propertyName));

            if (arguments is null)
                throw new ArgumentNullException(nameof(arguments));

            // TODO: Variables can represent multiple arguments, not just one.
            // https://developer.mozilla.org/en-US/docs/Web/CSS/--*

            // Variables are always inheritable.

            IPropertyDefinition definition = Define(propertyName)
                .WithInherited(true)
                .Build();

            return Create(definition, arguments);

        }
        private IProperty CreateUndefinedProperty(string propertyName, IPropertyValue[] arguments) {

            // Unknown properties will be treated in a very basic fashion because we don't know anything about how they should be initialized.
            // For now, complex custom properties should be handled by creating a custom PropertyFactory implementation.

            IPropertyDefinitionBuilder definition = new PropertyDefinitionBuilder(propertyName);

            if (arguments.Any()) {

                definition.WithInitial(arguments[0])
                    .WithType(arguments[0].Type);

            }

            return Create(definition.Build(), arguments);

        }

        private static bool TryTakeArgumentAs<T>(IList<IPropertyValue> arguments, out T value) {

            if (arguments is null)
                throw new ArgumentException(nameof(arguments));

            value = default;

            foreach (IPropertyValue arg in arguments) {

                if (arg.TryAs(out T tArg))
                    arguments.Remove(arg);

                value = tArg;

                return true;

            }

            return false;

        }

        private static BackgroundImage CreateBackgroundImage(IPropertyValue[] arguments) {

            if (arguments is null)
                throw new ArgumentNullException(nameof(arguments));

            return new BackgroundImage(arguments.Select(v => v.As<IImage>()));

        }
        private static Border CreateBorder(IPropertyValue[] arguments) {

            if (arguments is null)
                throw new ArgumentNullException(nameof(arguments));

            IList<IPropertyValue> argumentsList = arguments.ToList();

            LineWidth width = TryTakeArgumentAs(argumentsList, out LineWidth lineWidthResult) ?
                lineWidthResult :
                LineWidth.Medium;

            BorderStyle borderStyle = TryTakeArgumentAs(argumentsList, out BorderStyle borderStyleResult) ?
                borderStyleResult :
                BorderStyle.None;

            Color color = TryTakeArgumentAs(argumentsList, out Color colorResult) ?
                colorResult :
                Color.Black;

            return new Border(width, borderStyle, color);

        }
        private static BorderColors CreateBorderColors(IPropertyValue[] arguments) {

            if (arguments is null)
                throw new ArgumentNullException(nameof(arguments));

            Color[] colors = arguments
                .Select(arg => arg.As<Color>())
                .ToArray();

            if (colors.Count() == 4)
                return new BorderColors(colors[0], colors[1], colors[2], colors[3]);
            else if (colors.Count() == 3)
                return new BorderColors(colors[0], colors[1], colors[2]);
            else if (colors.Count() == 2)
                return new BorderColors(colors[0], colors[1]);
            else
                return new BorderColors(colors[0]);

        }
        private static Borders CreateBorders(IPropertyValue[] arguments) {

            if (arguments is null)
                throw new ArgumentNullException(nameof(arguments));

            return new Borders(
                arguments[0].As<Border>(),
                arguments[1].As<Border>(),
                arguments[2].As<Border>(),
                arguments[3].As<Border>()
                );

        }
        private static BorderRadii CreateBorderRadius(IPropertyValue[] arguments) {

            if (arguments is null)
                throw new ArgumentNullException(nameof(arguments));

            ILengthPercentage[] measurments = arguments
                .Select(arg => arg.As<ILengthPercentage>())
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
        private static BorderStyles CreateBorderStyles(IPropertyValue[] arguments) {

            if (arguments is null)
                throw new ArgumentNullException(nameof(arguments));

            BorderStyle[] borderStyles = arguments
                .Select(arg => arg.As<BorderStyle>())
                .ToArray();

            if (borderStyles.Count() == 4)
                return new BorderStyles(borderStyles[0], borderStyles[1], borderStyles[2], borderStyles[3]);
            else if (borderStyles.Count() == 3)
                return new BorderStyles(borderStyles[0], borderStyles[1], borderStyles[2]);
            else if (borderStyles.Count() == 2)
                return new BorderStyles(borderStyles[0], borderStyles[1]);
            else
                return new BorderStyles(borderStyles[0]);

        }
        private static BorderWidths CreateBorderWidths(IPropertyValue[] arguments) {

            if (arguments is null)
                throw new ArgumentNullException(nameof(arguments));

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

    }

}