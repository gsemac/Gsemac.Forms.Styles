using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal interface IValueConverter {

        Type SourceType { get; }
        Type DestinationType { get; }

        object Convert(object value);

    }

    internal interface IValueConverter<SourceT, DestinationT> :
        IValueConverter {

        DestinationT Convert(SourceT value);

    }

}