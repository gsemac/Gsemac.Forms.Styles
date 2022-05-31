namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class Length :
        DimensionBase,
        ILengthPercentage {

        // Public members

        public static Length Zero => new Length(0, string.Empty);

        public Length(double value) :
            this(value, LengthUnit.Pixel) {
        }
        public Length(double value, string unit) :
            base(value, unit, LengthUnit.GetUnits()) {
        }

    }

}