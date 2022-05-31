namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public sealed class LineWidth :
        Length {

        // Public members

        public static LineWidth Thin => new LineWidth(1);
        public static LineWidth Medium => new LineWidth(2);
        public static LineWidth Thick => new LineWidth(4);
        public static new LineWidth Zero => new LineWidth(0);

        public LineWidth(double value) :
            this(new Length(value)) {
        }
        public LineWidth(Length length) :
            base(length.Value, length.Unit) {
        }

        public override string ToString() {

            if (Equals(Thin))
                return "thin";

            if (Equals(Medium))
                return "medium";

            if (Equals(Thick))
                return "thick";

            return base.ToString();

        }

    }

}