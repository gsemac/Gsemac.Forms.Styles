using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal interface IValueConverterFactory {

        IValueConverter Create(Type sourceType, Type destinationType);

    }

}