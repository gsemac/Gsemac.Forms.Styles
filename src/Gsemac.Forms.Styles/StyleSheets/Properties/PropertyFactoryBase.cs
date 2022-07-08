using Gsemac.Forms.Styles.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
                return definition.Create(arguments);

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

            foreach (IPropertyDefinition longhandDefinition in definition.GetLonghands())
                AddDefinition(longhandDefinition);

        }

        // Private members

        private const string VariablePrefix = "--";

        private readonly IPropertyFactoryOptions options;
        private readonly IDictionary<string, IPropertyDefinition> definitions = new Dictionary<string, IPropertyDefinition>(StringComparer.OrdinalIgnoreCase);

        private void AddDefaultDefinitions() {

            // Any properties where the initial value is controlled by the user agent will not have an initial value set.

            AddDefinition(Build(PropertyName.AccentColor)
                .WithType<Color>()
                .WithInitial(PropertyValue.Auto)
                .Build());

            AddDefinition(Build(PropertyName.BackgroundColor)
                .WithInitial(Color.Transparent)
                .Build());

            AddDefinition(Build(PropertyName.BackgroundImage)
                .WithType<BackgroundImage>()
                .WithInitial(PropertyValue.None)
                .Build());

            AddDefinition(Build(PropertyName.Border)
                .WithType<Borders>()
                .WithLonghand(PropertyName.BorderTop, (Borders p) => p.Top)
                    .WithLonghand(PropertyName.BorderTopWidth, (Border p) => p.Width).WithInitial(LineWidth.Medium).End()
                    .WithLonghand(PropertyName.BorderTopStyle, (Border p) => p.Style).WithInitial(BorderStyle.None).End()
                    .WithLonghand(PropertyName.BorderTopColor, (Border p) => p.Color).WithInitial(PropertyValue.CurrentColor).End()
                    .End()
              .WithLonghand(PropertyName.BorderRight, (Borders p) => p.Right)
                    .WithLonghand(PropertyName.BorderRightWidth, (Border p) => p.Width).WithInitial(LineWidth.Medium).End()
                    .WithLonghand(PropertyName.BorderRightStyle, (Border p) => p.Style).WithInitial(BorderStyle.None).End()
                    .WithLonghand(PropertyName.BorderRightColor, (Border p) => p.Color).WithInitial(PropertyValue.CurrentColor).End()
                    .End()
              .WithLonghand(PropertyName.BorderBottom, (Borders p) => p.Bottom)
                    .WithLonghand(PropertyName.BorderBottomWidth, (Border p) => p.Width).WithInitial(LineWidth.Medium).End()
                    .WithLonghand(PropertyName.BorderBottomStyle, (Border p) => p.Style).WithInitial(BorderStyle.None).End()
                    .WithLonghand(PropertyName.BorderBottomColor, (Border p) => p.Color).WithInitial(PropertyValue.CurrentColor).End()
                    .End()
              .WithLonghand(PropertyName.BorderLeft, (Borders p) => p.Left)
                    .WithLonghand(PropertyName.BorderLeftWidth, (Border p) => p.Width).WithInitial(LineWidth.Medium).End()
                    .WithLonghand(PropertyName.BorderLeftStyle, (Border p) => p.Style).WithInitial(BorderStyle.None).End()
                    .WithLonghand(PropertyName.BorderLeftColor, (Border p) => p.Color).WithInitial(PropertyValue.CurrentColor).End()
                    .End()
              .Build());

            AddDefinition(Build(PropertyName.BorderBottomStyle)
                .WithInitial(BorderStyle.None)
                .Build());

            AddDefinition(Build(PropertyName.BorderColor)
                .WithType<Color>()
                .Build());

            AddDefinition(Build(PropertyName.BorderLeftStyle)
                .WithInitial(BorderStyle.None)
                .Build());

            AddDefinition(Build(PropertyName.BorderRadius)
                .WithInitial(new BorderRadii())
                .WithLonghand(PropertyName.BorderTopLeftRadius, (BorderRadii p) => p.TopLeft).WithInitial(Length.Zero).End()
                .WithLonghand(PropertyName.BorderTopRightRadius, (BorderRadii p) => p.TopRight).WithInitial(Length.Zero).End()
                .WithLonghand(PropertyName.BorderBottomRightRadius, (BorderRadii p) => p.BottomRight).WithInitial(Length.Zero).End()
                .WithLonghand(PropertyName.BorderBottomLeftRadius, (BorderRadii p) => p.BottomLeft).WithInitial(Length.Zero).End()
                .Build());

            AddDefinition(Build(PropertyName.BorderRightStyle)
                .WithInitial(BorderStyle.None)
                .Build());

            AddDefinition(Build(PropertyName.BorderStyle)
                .WithType<BorderStyle>()
                .Build());

            AddDefinition(Build(PropertyName.BorderTopStyle)
                .WithInitial(BorderStyle.None)
                .Build());

            AddDefinition(Build(PropertyName.BorderWidth)
                .WithType<BorderWidths>()
                .WithInitial(new BorderWidths())
                .WithLonghand(PropertyName.BorderTopWidth, (BorderWidths p) => p.Top).WithInitial(LineWidth.Medium).End()
                .WithLonghand(PropertyName.BorderRightWidth, (BorderWidths p) => p.Right).WithInitial(LineWidth.Medium).End()
                .WithLonghand(PropertyName.BorderBottomWidth, (BorderWidths p) => p.Bottom).WithInitial(LineWidth.Medium).End()
                .WithLonghand(PropertyName.BorderLeftWidth, (BorderWidths p) => p.Left).WithInitial(LineWidth.Medium).End()
                .Build());

            AddDefinition(Build(PropertyName.Color)
                .WithType<Color>()
                .Build());

            AddDefinition(Build(PropertyName.Opacity)
                .WithType<double>()
                .WithInitial(1.0)
                .Build());

        }

        private IPropertyDefinitionBuilder Build(string propertyName) {

            return new PropertyDefinitionBuilder(propertyName);

        }

        private IProperty CreateVariableProperty(string propertyName, IPropertyValue[] arguments) {

            if (propertyName is null)
                throw new ArgumentNullException(nameof(propertyName));

            if (arguments is null)
                throw new ArgumentNullException(nameof(arguments));

            // TODO: Variables can represent multiple arguments, not just one.
            // https://developer.mozilla.org/en-US/docs/Web/CSS/--*

            // Variables are always inheritable.

            return Property.Create(propertyName, arguments[0], inherited: true);

        }
        private IProperty CreateUndefinedProperty(string propertyName, IPropertyValue[] arguments) {

            // Unknown properties will be treated in a very basic fashion because we don't know anything about how they should be initialized.
            // For now, complex custom properties should be handled by creating a custom PropertyFactory implementation.

            return new Property(propertyName, arguments.FirstOrDefault());

        }

    }

}