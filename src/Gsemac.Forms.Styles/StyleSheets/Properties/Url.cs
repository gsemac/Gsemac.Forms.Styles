namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class Url :
        IImage {

        // Public members

        public string Value { get; }

        public Url(string value) {

            Value = value;

        }

        public override string ToString() {

            return $"url({Value})";

        }

    }

}