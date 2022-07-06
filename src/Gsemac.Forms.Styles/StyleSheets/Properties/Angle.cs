namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public sealed class Angle :
        DimensionBase {

        // Public members

        public static Angle Zero => new Angle(0, string.Empty);

        public Angle(double value) :
            this(value, Properties.Units.Degree) {
        }
        public Angle(double value, string unit) :
            base(value, unit, Properties.Units.GetAngleUnits()) {
        }

    }

}