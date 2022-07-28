using Gsemac.Forms.Styles.Properties;
using Gsemac.Forms.Styles.StyleSheets.Dom;
using Gsemac.Forms.Styles.StyleSheets.Properties;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Dom {

    public class ControlNode2 :
        StyleSheets.Dom.NodeBase {

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

            AddStates(control);
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

        // Protected members

        protected override IRuleset ComputeStyle(IStyleComputationContext context) {

            IRuleset ruleset = base.ComputeStyle(context);

            if (!ruleset.ContainsKey(PropertyName.BackgroundColor))
                ruleset.Add(PropertyFactory.Default.Create(PropertyName.BackgroundColor, GetBackgroundColor(Control)));

            if (!ruleset.ContainsKey(PropertyName.Color))
                ruleset.Add(PropertyFactory.Default.Create(PropertyName.Color, Control.ForeColor));

            // When rendering a control with child controls, "holes" clipped out around child controls.
            // Since nothing is rendered in these "holes", if we try to clear with Color.Transparent, we'll just get a black background around the control.
            // To resolve this, the clear color should be equal to the background color of the parent control.

            if (Parent is object) {

                Color clearColor = Parent.GetComputedStyle(context).BackgroundColor;

                ruleset.Add(PropertyFactory.Default.Create(CustomPropertyName.ClearColor, clearColor));

            }

            return ruleset;

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
        private void AddStates(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            SetEnabled(control.Enabled);
            SetFocused(control.Focused);

        }
        private void RemoveChild(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            Children.Remove(new ControlNode2(control));

        }

        private void SetEnabled(bool enabled) {

            if (enabled) {

                States.Add(NodeState.Enabled);
                States.Remove(NodeState.Disabled);

            }
            else {

                States.Remove(NodeState.Enabled);
                States.Add(NodeState.Disabled);

            }

        }
        private void SetFocused(bool focused) {

            if (focused) {

                States.Add(NodeState.Focus);

                if (ControlUtilities2.FocusCuesShown(Control))
                    States.Add(NodeState.FocusVisible);

            }
            else {

                States.Remove(NodeState.Focus);
                States.Remove(NodeState.FocusVisible);

            }

        }

        private void AddEventHandlers(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            control.ControlAdded += ControlAddedHandler;
            control.ControlRemoved += ControlRemovedHandler;
            control.Disposed += DisposedHandler;
            control.EnabledChanged += EnabledChangedHandler;
            control.GotFocus += GotFocusHandler;
            control.KeyDown += KeyDownHandler;
            control.KeyUp += KeyUpHandler;
            control.LostFocus += LostFocusHandler;
            control.MouseDown += MouseDownHandler;
            control.MouseEnter += MouseEnterHandler;
            control.MouseLeave += MouseLeaveHandler;
            control.MouseUp += MouseUpHandler;

        }
        private void RemoveEventHandlers(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            control.ControlAdded -= ControlAddedHandler;
            control.ControlRemoved -= ControlRemovedHandler;

            control.Disposed -= DisposedHandler;

            control.GotFocus -= GotFocusHandler;
            control.LostFocus -= LostFocusHandler;

            control.MouseEnter -= MouseEnterHandler;
            control.MouseLeave -= MouseLeaveHandler;

        }

        private void ControlAddedHandler(object sender, ControlEventArgs e) {

            AddChild(e.Control);

        }
        private void ControlRemovedHandler(object sender, ControlEventArgs e) {

            RemoveChild(e.Control);

        }
        private void DisposedHandler(object sender, EventArgs e) {

            RemoveEventHandlers((Control)sender);

        }
        private void EnabledChangedHandler(object sender, EventArgs e) {

            bool enabled = ((Control)sender).Enabled;

            SetEnabled(enabled);

        }
        private void GotFocusHandler(object sender, EventArgs e) {

            SetFocused(true);

        }
        private void KeyDownHandler(object sender, KeyEventArgs e) {

            if (sender is ButtonBase _) {

                bool active = e.KeyCode == Keys.Space &&
                    !e.Alt &&
                    !e.Control &&
                    !e.Shift;

                if (active)
                    States.Add(NodeState.Active);

            }

        }
        private void KeyUpHandler(object sender, KeyEventArgs e) {

            if (sender is ButtonBase _) {

                bool notActive = e.KeyCode == Keys.Space;

                if (notActive)
                    States.Remove(NodeState.Active);

            }

        }
        private void LostFocusHandler(object sender, EventArgs e) {

            SetFocused(false);

        }
        private void MouseDownHandler(object sender, MouseEventArgs e) {

            States.Add(NodeState.Active);

        }
        private void MouseEnterHandler(object sender, EventArgs e) {

            States.Add(NodeState.Hover);

        }
        private void MouseLeaveHandler(object sender, EventArgs e) {

            States.Remove(NodeState.Hover);

        }
        private void MouseUpHandler(object sender, MouseEventArgs e) {

            States.Remove(NodeState.Active);

        }

        private static Color GetBackgroundColor(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            // If the control has a definite background color, return that.

            if (!control.BackColor.Equals(Color.Transparent))
                return control.BackColor;

            // Some controls, such as TabPage, will have their background color set to Color.Transparent.
            // Their true background color is derived from VisualStyleRenderer.

            if (control is TabPage tabPageControl && tabPageControl.UseVisualStyleBackColor) {

                // In an ideal world, we should be able to do this to get the TabPage background color:
                //
                // return new VisualStyleRenderer(VisualStyleElement.Tab.Body.Normal).GetColor(ColorProperty.FillColor);
                //
                // However, none of the color properties returned by GetColor actually match the default background color of the TabPage (white).
                // I don't really understand what the problem is, but we'll just return SystemColors.Window (255, 255, 255) explicitly for now.

                return SystemColors.Window;

            }

            // We weren't able to determine a suitable background color.

            return Color.Transparent;

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