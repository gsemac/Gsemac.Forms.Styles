using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class ControlNode :
        NodeBase {

        // Public members

        public override string Tag => "Control";
        public override string Id => control.Name;
        public override NodeStates States { get; } = NodeStates.None;
        public override INode Parent => control.Parent != null ? new ControlNode(control.Parent) : null;

        public ControlNode(Control control) :
            this(control, control.ClientRectangle) {
        }
        public ControlNode(Control control, Rectangle clientRectangle) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            this.control = control;

            Point mousePos = control.PointToClient(Cursor.Position);
            Rectangle mouseRect = new Rectangle(mousePos.X, mousePos.Y, 1, 1);

            if (clientRectangle.IntersectsWith(mouseRect)) {

                States |= NodeStates.Hover;

                if (Control.MouseButtons.HasFlag(MouseButtons.Left))
                    States |= NodeStates.Active;

            }

            switch (control) {

                case CheckBox checkBox:

                    if (checkBox.Checked)
                        States |= NodeStates.Checked;

                    break;

            }

            if (control.Focused)
                States |= NodeStates.Focus;

            if (!control.Enabled)
                States |= NodeStates.Disabled;

        }

        // Protected members

        protected override IEnumerable<string> GetClasses() {

            Type currentType = control.GetType();

            while (currentType != typeof(Control)) {

                yield return currentType.Name;

                currentType = currentType.BaseType;

            }

            yield return typeof(Control).Name;

        }

        // Private members

        private readonly Control control;

    }

}