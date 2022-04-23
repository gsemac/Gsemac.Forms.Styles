using Gsemac.Forms.Styles.Properties;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal abstract class ValueConverterBase<SourceT, DestinationT> :
        IValueConverter<SourceT, DestinationT> {

        public Type SourceType => typeof(SourceT);
        public Type DestinationType => typeof(DestinationT);

        public abstract DestinationT Convert(SourceT value);

        public object Convert(object value) {

            if (value is SourceT)
                return Convert((SourceT)value);

            throw new ArgumentException(string.Format(ExceptionMessages.CannotConvertObjectsOfType, value.GetType()), nameof(value));

        }

    }

}