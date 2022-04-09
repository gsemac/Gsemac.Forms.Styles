using System;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Dom {

    public class ControlNode2 :
        NodeBase {

        // Public members

        public Control Control { get; }

        public ControlNode2(Control control) :
            this(control, populateChildren: true) {
        }
        public ControlNode2(Control control, bool populateChildren) :
            base(GetTagName(control)) {

            Control = control;
            Id = GetId(control);

            AddClasses(control);

            if (populateChildren)
                AddChildren(control);

            AddEventHandlers(control);

        }

        public override bool Equals(object obj) {

            if (obj is ControlNode2 controlDomNode)
                return controlDomNode.Control == Control;

            return base.Equals(obj);

        }
        public override int GetHashCode() {

            return Control.GetHashCode();

        }

        // Private members

        private void AddClasses(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            Type currentType = control.GetType();

            while (currentType != typeof(Control)) {

                Classes.Add(currentType.Name);

                currentType = currentType.BaseType;

            }

            Classes.Add(typeof(Control).Name);

        }
        private void AddChild(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            ControlNode2 node = new ControlNode2(control);

            control.Disposed += (sender, e) => Children.Remove(node);

            Children.Add(node);

        }
        private void AddChildren(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            foreach (Control childControl in control.Controls)
                AddChild(childControl);

        }
        private void RemoveChild(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            Children.Remove(new ControlNode2(control));

        }

        private void AddEventHandlers(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            control.ControlAdded += ControlAddedHandler;
            control.ControlRemoved += ControlRemovedHandler;

            control.GotFocus += GotFocusHandler;
            control.LostFocus += LostFocusHandler;

            control.Disposed += DisposedHandler;

        }
        private void RemoveEventHandlers(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            control.ControlAdded -= ControlAddedHandler;
            control.ControlRemoved -= ControlRemovedHandler;

            control.GotFocus -= GotFocusHandler;
            control.LostFocus -= LostFocusHandler;

            control.Disposed -= DisposedHandler;

        }

        private static string GetId(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            return control.Name;

        }
        private static string GetTagName(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            return control.GetType().Name;

        }

        private void ControlAddedHandler(object sender, ControlEventArgs e) {

            AddChild(e.Control);

        }
        private void ControlRemovedHandler(object sender, ControlEventArgs e) {

            RemoveChild(e.Control);

        }
        private void GotFocusHandler(object sender, EventArgs e) {

            States.Add(NodeState.Focused);

        }
        private void LostFocusHandler(object sender, EventArgs e) {

            States.Remove(NodeState.Focused);

        }
        private void DisposedHandler(object sender, EventArgs e) {

            RemoveEventHandlers((Control)sender);

        }

    }

}