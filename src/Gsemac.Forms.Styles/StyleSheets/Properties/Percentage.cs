namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public sealed class Percentage :
        DimensionBase,
        ILengthOrPercentage {

        // Public members

        public static Percentage Zero => new Percentage(0);

        public Percentage(double value) :
            base(value, PercentageUnit.Percent, PercentageUnit.GetUnits()) {
        }

    }

}