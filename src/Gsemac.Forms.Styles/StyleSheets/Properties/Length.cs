namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class Length :
        DimensionBase,
        ILengthPercentage {

        // Public members

        public static Length Zero => new Length(0, string.Empty);

        public Length(double value) :
            this(value, Units.Pixel) {
        }
        public Length(double value, string unit) :
            base(value, unit, Units.GetLengthUnits()) {
        }

    }

}