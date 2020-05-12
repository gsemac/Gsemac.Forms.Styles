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
        public override INode Parent { get; }

        public UserNode(Control baseControl, Rectangle clientRectangle) {

            Point cursorPos = Cursor.Position;
            Point cursotRelativePos = baseControl.PointToClient(cursorPos);
            Rectangle cursorRect = new Rectangle(cursotRelativePos.X, cursotRelativePos.Y, 1, 1);

            if (clientRectangle.IntersectsWith(cursorRect)) {

                states |= NodeStates.Hover;

                if (Control.MouseButtons.HasFlag(MouseButtons.Left))
                    states |= NodeStates.Active;

            }

        }
        public UserNode(string tag, string className, INode parent = null, NodeStates states = NodeStates.None) {

            this.tag = tag;
            this.classes.Add(className);
            this.states = states;
            this.Parent = parent;

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
        public void SetId(string value) {

            id = value;

        }
        public void SetStates(NodeStates value) {

            states = value;

        }

        // Protected members

        protected override IEnumerable<string> GetClasses() {

            return classes;

        }

        // Private members

        private readonly List<string> classes = new List<string>();
        private string tag = "";
        private string id = "";
        private NodeStates states = NodeStates.None;

    }

}