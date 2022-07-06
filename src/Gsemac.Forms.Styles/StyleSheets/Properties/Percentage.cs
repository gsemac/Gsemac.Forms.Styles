namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public sealed class Percentage :
        DimensionBase,
        ILengthPercentage {

        // Public members

        public static Percentage Zero => new Percentage(0);

        public Percentage(double value) :
            base(value, Units.Percent, Units.GetPercentageUnits()) {
        }

    }

}