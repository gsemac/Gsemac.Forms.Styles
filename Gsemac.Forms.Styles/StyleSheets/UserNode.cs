using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class UserNode :
        NodeBase {

        // Public members

        public override string Tag => tag;
        public override string Id => id;
        public override NodeStates States => states;
        public override INode Parent => parent;

        public UserNode(Rectangle clientRectangle, Point cursorPosition) {

            Rectangle cursorRect = new Rectangle(cursorPosition.X, cursorPosition.Y, 1, 1);

            if (clientRectangle.IntersectsWith(cursorRect)) {

                states |= NodeStates.Hover;

                if (Control.MouseButtons.HasFlag(MouseButtons.Left))
                    states |= NodeStates.Active;

            }

        }
        public UserNode(string tag, IEnumerable<string> classes) {

            this.tag = tag;

            foreach (string @class in classes)
                AddClass(@class);

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

        // Protected members

        protected override IEnumerable<string> GetClasses() {

            return classes;

        }

        // Private members

        private readonly List<string> classes = new List<string>();
        private string tag = "";
        private string id = "";
        private INode parent = null;
        private NodeStates states = NodeStates.None;

    }

}