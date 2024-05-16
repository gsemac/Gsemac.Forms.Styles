using Gsemac.Forms.Extensions;
using Gsemac.Forms.Styles.Dom;
using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators {

    public abstract class StyleApplicatorBase :
        IStyleApplicator {

        // Public members

        public void ApplyStyles() {

            MakeThisCurrentApplicator();

            if (StyleSheet != null) {

                foreach (Form form in Application.OpenForms.Cast<Form>())
                    ApplyStyles(form);

                if (options.HasFlag(StyleApplicatorOptions.AddMessageFilter)) {

                    messageFilter = new StyleApplicatorMessageFilter(this);

                    Application.AddMessageFilter(messageFilter);

                }

            }
            else {

                // Applying styles with a null style sheet is equivalent to clearing them.

                ClearStyles();

            }

        }
        public void ClearStyles() {

            MakeThisCurrentApplicator();

            if (options.HasFlag(StyleApplicatorOptions.AddMessageFilter) && messageFilter != null)
                Application.RemoveMessageFilter(messageFilter);

            foreach (Control control in controlInfo.Keys.ToArray())
                ClearStyles(control, new StyleApplicationOptions(StyleApplicationOptions.Default) { Recursive = false });

            ClearCurrentApplicator();

        }

        public void ApplyStyles(Control control, IStyleApplicationOptions options = null) {

            options = options ?? StyleApplicationOptions.Default;

            MakeThisCurrentApplicator();

            if (StyleSheet != null) {

                // Save control info for all controls before applying styles, which prevents changes to inherited properties affecting what is saved.
                // This is relevant for the BackColor and ForeColor properties of child controls.

                AddControlInfoRecursive(control, options);

                // Apply styles.

                ApplyStylesRecursive(control, options);

            }
            else {

                // Applying styles with a null style sheet is equivalent to clearing them.

                ClearStyles(control, options);

            }

        }
        public void ClearStyles(Control control, IStyleApplicationOptions options = null) {

            options = options ?? StyleApplicationOptions.Default;

            OnClearStyles(control);

            RemoveControlInfo(control);

            if (options.Recursive && control.HasChildren) {

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

            return StyleSheet.GetStyles(new ControlNode2(control)).Any();

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
                ControlState = control.Save(new ControlStateOptions() {
                    IncludeLayoutProperties = !(control is Form),
                    IncludeVisualProperties = true,
                }),
            };

            control.ControlAdded += ControlAddedEventHandler;
            control.Disposed += ControlDisposedEventHandler;

            info.ResetControl += (c) => {

                control.ControlAdded -= ControlAddedEventHandler;
                control.Disposed -= ControlDisposedEventHandler;

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

        private void AddControlInfoRecursive(Control control, IStyleApplicationOptions options) {

            if (!options.RequireStyle || HasStyles(control))
                AddControlInfo(control);
            else
                RemoveControlInfo(control);

            if (options.Recursive && control.HasChildren) {

                foreach (Control childControl in GetStylableChildControls(control))
                    AddControlInfoRecursive(childControl, options);

                if (control.ContextMenuStrip != null)
                    AddControlInfoRecursive(control.ContextMenuStrip, options);

            }

        }
        private void ApplyStylesRecursive(Control control, IStyleApplicationOptions options) {

            if (!options.RequireStyle || HasStyles(control))
                OnApplyStyles(control);
            else
                OnClearStyles(control);

            if (options.Recursive && control.HasChildren) {

                foreach (Control childControl in GetStylableChildControls(control))
                    ApplyStylesRecursive(childControl, options);

                if (control.ContextMenuStrip != null)
                    ApplyStylesRecursive(control.ContextMenuStrip, options);

            }

            control.Invalidate();

        }

        private void ControlAddedEventHandler(object sender, ControlEventArgs e) {
            Console.WriteLine($"added {e.Control}");
            ApplyStyles(e.Control);

        }
        private void ControlDisposedEventHandler(object sender, EventArgs e) {

            if (sender is Control control) {

                // Just remove the control info instead of calling RemoveControlInfo.
                // Because the control has been disposed, there is no need to restore its state.

                controlInfo.Remove(control);

            }

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