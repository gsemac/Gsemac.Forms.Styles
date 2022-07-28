using Gsemac.Data.ValueConversion;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class SideOrCornerToAngleConverter :
        ValueConverterBase<SideOrCorner, Angle> {

        public override bool TryConvert(SideOrCorner value, out Angle result) {

            result = new Angle((int)value, Units.Degree);

            return true;

        }

    }

}