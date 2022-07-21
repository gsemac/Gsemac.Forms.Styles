namespace Gsemac.Forms.Styles.StyleSheets.Dom {

    public class Node :
        NodeBase {

        // Public members

        public static Node Empty => new Node();

        // Private members

        public Node() :
            base(string.Empty) {
        }

    }

}