using System;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Dom {

    public class ControlNode :
        DomNodeBase {

        // Public members

        public Control Control { get; }

        public ControlNode(Control control) :
            base(GetTagName(control)) {

            Control = control;
            Id = GetId(control);

            AddClasses(control);
            AddChildren(control);
            AddEventHandlers(control);

        }

        public override bool Equals(object obj) {

            if (obj is ControlNode controlDomNode)
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

            ControlNode node = new ControlNode(control);

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

            Children.Remove(new ControlNode(control));

        }

        private void AddEventHandlers(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            control.ControlAdded += (sender, e) => AddChild(e.Control);
            control.ControlRemoved += (sender, e) => RemoveChild(e.Control);

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

    }

}