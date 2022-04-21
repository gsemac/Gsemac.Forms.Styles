namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class ImageUrl :
        IImage {

        // Public members

        public string Value { get; }

        public ImageUrl(string value) {

            Value = value;

        }

        public override string ToString() {

            return $"url({Value})";

        }

    }

}