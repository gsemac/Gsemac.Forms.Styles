using Gsemac.Forms.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators {

    public abstract class StyleApplicatorBase :
        IStyleApplicator {

        // Public members

        public void ApplyStyles() {

            MakeThisCurrentApplicator();

            foreach (Form form in Application.OpenForms.Cast<Form>())
                ApplyStyles(form);

            if (options.HasFlag(StyleApplicatorOptions.AddMessageFilter)) {

                messageFilter = new StyleApplicatorMessageFilter(this);

                Application.AddMessageFilter(messageFilter);

            }

        }

        public void ClearStyles() {

            if (options.HasFlag(StyleApplicatorOptions.AddMessageFilter) && messageFilter != null)
                Application.RemoveMessageFilter(messageFilter);

            foreach (Form form in Application.OpenForms.Cast<Form>())
                ClearStyles(form);

            if (options.HasFlag(StyleApplicatorOptions.DisposeStyleSheet) && StyleSheet != null)
                StyleSheet.Dispose();

            ClearCurrentApplicator();

        }

        public void ApplyStyles(Control control, ControlStyleOptions options = ControlStyleOptions.Default) {

            MakeThisCurrentApplicator();

            // Save control info for all controls before applying styles, which prevents changes to inherited properties affecting what is saved.
            // This is relevant for the BackColor and ForeColor properties of child controls.

            AddControlInfoRecursive(control, options);

            // Apply styles.

            ApplyStylesRecursive(control, options);

        }
        public void ClearStyles(Control control, ControlStyleOptions options = ControlStyleOptions.Default) {

            OnClearStyles(control);

            RemoveControlInfo(control);

            if (options.HasFlag(ControlStyleOptions.Recursive) && control.HasChildren) {

                foreach (Control childControl in control.Controls)
                    ClearStyles(childControl, options);

            }

            if (control.ContextMenuStrip != null)
                ClearStyles(control.ContextMenuStrip, options);

            control.Invalidate();

        }

        // Protected members

        protected delegate void ResetControlHandler(Control control);

        protected IStyleSheet StyleSheet { get; }

        protected class ControlInfo {

            // Public members

            public event ResetControlHandler ResetControl;

            public IControlState ControlState { get; set; }
            public bool ParentDraw { get; set; } = false;

            public void DoResetControl(Control control) {

                ResetControl?.Invoke(control);

            }

        }

        protected StyleApplicatorBase(IStyleSheet styleSheet, StyleApplicatorOptions options = StyleApplicatorOptions.Default) {

            StyleSheet = styleSheet;
            this.options = options;

        }

        protected virtual bool HasStyles(Control control) {

            return StyleSheet.GetRuleset(new ControlNode(control), false).Any();

        }

        protected abstract void OnApplyStyles(Control control);
        protected virtual void OnClearStyles(Control control) { }

        protected ControlInfo GetControlInfo(Control control) {

            if (controlInfo.TryGetValue(control, out ControlInfo info))
                return info;

            return null;

        }

        // Private members

        private readonly StyleApplicatorOptions options = StyleApplicatorOptions.Default;
        private readonly Dictionary<Control, ControlInfo> controlInfo = new Dictionary<Control, ControlInfo>();
        private IMessageFilter messageFilter = null;

        private static IStyleApplicator currentApplicator = null;

        private void AddControlInfo(Control control) {

            RemoveControlInfo(control);

            // Only store visual properties for Forms to avoid resizing them when the style is cleared.

            ControlInfo info = new ControlInfo() {
                ControlState = control.Save(control is Form ? ControlStateOptions.StoreVisualProperties : ControlStateOptions.Default)
            };

            control.ControlAdded += ControlAddedEventHandler;

            info.ResetControl += (c) => {
                control.ControlAdded -= ControlAddedEventHandler;
            };

            controlInfo[control] = info;

        }
        private bool RemoveControlInfo(Control control) {

            if (controlInfo.TryGetValue(control, out ControlInfo info)) {

                // Remove event handlers before doing anything else to prevent them from modifying the control's appearance.

                info.DoResetControl(control);

                // Only disable styles that the control didn't have originally.
                // Controls like Panel and TabPage will have UserPaint enabled by default, and it should not be disabled.

                control.Restore(info.ControlState);

                controlInfo.Remove(control);

                return true;

            }

            return false;

        }

        private void AddControlInfoRecursive(Control control, ControlStyleOptions options) {

            if (!options.HasFlag(ControlStyleOptions.RulesRequired) || HasStyles(control))
                AddControlInfo(control);
            else
                RemoveControlInfo(control);

            if (options.HasFlag(ControlStyleOptions.Recursive) && control.HasChildren) {

                foreach (Control childControl in GetStylableChildControls(control))
                    AddControlInfoRecursive(childControl, options);

                if (control.ContextMenuStrip != null)
                    AddControlInfoRecursive(control.ContextMenuStrip, options);

            }

        }
        private void ApplyStylesRecursive(Control control, ControlStyleOptions options) {

            if (!options.HasFlag(ControlStyleOptions.RulesRequired) || HasStyles(control))
                OnApplyStyles(control);
            else
                OnClearStyles(control);

            if (options.HasFlag(ControlStyleOptions.Recursive) && control.HasChildren) {

                foreach (Control childControl in GetStylableChildControls(control))
                    ApplyStylesRecursive(childControl, options);

                if (control.ContextMenuStrip != null)
                    ApplyStylesRecursive(control.ContextMenuStrip, options);

            }

            control.Invalidate();

        }

        private void ControlAddedEventHandler(object sender, ControlEventArgs e) {

            ApplyStyles(e.Control);

        }

        private IEnumerable<Control> GetStylableChildControls(Control control) {

            foreach (Control childControl in control.Controls) {

                if (childControl is TextBox && control is NumericUpDown)
                    continue;

                yield return childControl;

            }

        }

        private void MakeThisCurrentApplicator() {

            if (currentApplicator != null && currentApplicator != this)
                currentApplicator.ClearStyles();

            currentApplicator = this;

        }
        private void ClearCurrentApplicator() {

            if (currentApplicator == this)
                currentApplicator = null;

        }

    }

}