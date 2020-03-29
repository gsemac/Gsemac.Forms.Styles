using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Controls {

    public class ControlNode :
        NodeBase {

        // Public members

        public override string Id => control.Name;
        public override NodeStates States { get; } = NodeStates.None;

        public ControlNode(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            this.control = control;

            Point mousePos = control.PointToClient(Cursor.Position);
            Rectangle mouseRect = new Rectangle(mousePos.X, mousePos.Y, 1, 1);

            if (control.ClientRectangle.IntersectsWith(mouseRect)) {

                if (Control.MouseButtons.HasFlag(MouseButtons.Left))
                    States |= NodeStates.Active;
                else
                    States |= NodeStates.Hover;

            }

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