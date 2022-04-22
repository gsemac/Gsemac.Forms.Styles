﻿using Gsemac.Forms.Styles.StyleSheets.Properties.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class InitialValueFactory :
        IInitialValueFactory {

        // Public members

        public static InitialValueFactory Default => new InitialValueFactory();

        public IPropertyValue GetInitialValue(string propertyName, IRuleset style) {

            if (string.IsNullOrWhiteSpace(propertyName))
                return null;

            switch (propertyName.ToLowerInvariant()) {

                case PropertyName.BackgroundImage:
                    return PropertyValue.Create(new BackgroundImage());

                case PropertyName.BackgroundColor:
                    return PropertyValue.Create(Color.Transparent);

                case PropertyName.Border:
                    return PropertyValue.Create(new Border(style.Color));

                case PropertyName.BorderTopColor:
                case PropertyName.BorderRightColor:
                case PropertyName.BorderBottomColor:
                case PropertyName.BorderLeftColor:
                    return PropertyValue.Create(style.Color);

                case PropertyName.BorderTopStyle:
                case PropertyName.BorderRightStyle:
                case PropertyName.BorderBottomStyle:
                case PropertyName.BorderLeftStyle:
                    return PropertyValue.Create(BorderStyle.None);

                case PropertyName.Color:
                    return PropertyValue.Create(Color.Black); // Dependant on user agent

                default:
                    return PropertyValue.Null;

            }

        }
        public T GetInitialValue<T>(string propertyName, IRuleset style) {

            if (style is null)
                throw new ArgumentNullException(nameof(style));

            IPropertyValue initialValue = this.GetInitialValue(propertyName);

            if (initialValue is object && initialValue.Type.Equals(typeof(T)))
                return (T)initialValue.Value;

            return default;

        }

    }

}