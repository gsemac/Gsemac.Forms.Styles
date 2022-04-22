using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public interface IPropertyValueWriter {

        void Write(Color value);
        void Write(BorderStyle value);
        void Write(double value);
        void Write(int value);
        void Write(object value);

    }

}