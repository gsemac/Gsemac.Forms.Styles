using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class UserNode :
       Dom.NodeBase {

        // Public members

        public UserNode(Rectangle clientRectangle, Point cursorPosition) :
            base("") {

            Rectangle cursorRect = new Rectangle(cursorPosition.X, cursorPosition.Y, 1, 1);

            if (clientRectangle.IntersectsWith(cursorRect)) {

                states |= NodeStates.Hover;

                if (Control.MouseButtons.HasFlag(MouseButtons.Left))
                    states |= NodeStates.Active;

            }

        }
        public UserNode(string tag, IEnumerable<string> classes) :
            base(tag) {

            this.tag = tag;

            foreach (string @class in classes)
                AddClass(@class);

        }
        public UserNode(INode baseNode) :
            base(baseNode.Tag) {

            this.baseNode = baseNode;

        }

        public void AddClass(string value) {

            classes.Add(value);

        }
        public void AddState(NodeStates value) {

            states |= value;

        }

        public void SetTag(string value) {

            tag = value;

        }
        public void SetClass(string value) {

            classes.Clear();

            AddClass(value);

        }
        public void SetId(string value) {

            id = value;

        }
        public void SetStates(NodeStates value) {

            states = value;

        }
        public void SetParent(INode node) {

            parent = node;

        }
        public void SetPseudoElement(string value) {

            this.pseudoElement = value;

        }

        // Private members

        private readonly INode baseNode = null;
        private readonly List<string> classes = new List<string>();
        private string pseudoElement = string.Empty;
        private string tag = string.Empty;
        private string id = string.Empty;
        private INode parent = null;
        private NodeStates states = NodeStates.None;

        private IEnumerable<string> GetClasses() {

            IEnumerable<string> baseNodeClasses = baseNode != null ?
                baseNode.Classes :
                Enumerable.Empty<string>();

            return baseNodeClasses.Concat(classes);

        }
        private string GetPseudoElement() {

            return string.IsNullOrEmpty(pseudoElement) && baseNode != null ?
                baseNode.PseudoElement :
                pseudoElement;

        }
        private string GetTag() {

            return string.IsNullOrEmpty(tag) && baseNode != null ?
                baseNode.Tag :
                tag;

        }
        private string GetId() {

            return string.IsNullOrEmpty(id) && baseNode != null ?
                baseNode.Id :
                id;

        }
        private NodeStates GetStates() {

            return states == NodeStates.None && baseNode != null ?
                baseNode.States :
                states;

        }
        private INode GetParent() {

            return parent is null && baseNode != null ?
                baseNode.Parent :
                parent;

        }

    }

}