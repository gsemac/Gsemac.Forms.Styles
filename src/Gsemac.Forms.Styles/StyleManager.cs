using Gsemac.Forms.Styles.Applicators2;
using Gsemac.Forms.Styles.Dom;
using Gsemac.Forms.Styles.StyleSheets;
using Gsemac.Forms.Styles.StyleSheets.Dom;
using Gsemac.Forms.Styles.StyleSheets.Properties;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles {

    public class StyleManager :
        IStyleManager {

        // Public members

        public ICollection<IStyleSheet> StyleSheets { get; } = new List<IStyleSheet>();

        public StyleManager(IStyleApplicatorFactory styleApplicatorFactory) {

            if (styleApplicatorFactory is null)
                throw new ArgumentNullException(nameof(styleApplicatorFactory));

            this.messageFilter = new StyleManagerMessageFilter(this);
            this.styleApplicatorFactory = styleApplicatorFactory;

        }

        public void ApplyStyles() {

            if (!HasStyles()) {

                // If there are no style sheets, clear styles instead.

                ResetStyles();

            }
            else {

                // Apply styles to all open forms.

                foreach (Form form in Application.OpenForms.Cast<Form>())
                    ApplyStyles(form);

                // Create a message filter to style any forms that might be opened later.

                if (!addedMessageFilter) {

                    Application.AddMessageFilter(messageFilter);

                    addedMessageFilter = true;

                }

            }

        }
        public void ApplyStyles(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            // Clear all existing styles for this control before attempting to apply them.

            ResetStyles(control);

            if (HasStyles())
                ApplyStylesInternal(control, createStyleWatcher: true, recursive: true);

        }
        public void ResetStyles() {

            // Remove the message filter.

            if (addedMessageFilter) {

                Application.RemoveMessageFilter(messageFilter);

                addedMessageFilter = false;

            }

            // Reset styles for all open forms.

            foreach (Form form in Application.OpenForms.Cast<Form>())
                ResetStyles(form);

        }
        public void ResetStyles(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            //// Remove any DOMs associated with the control.

            //controlDoms.Remove(control);

            //// Restore the state of the control and its children.
            //// This will removed the stored state information.

            //RestoreControlState(control, recursive: true);

        }

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Protected members

        protected virtual void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                }

                disposedValue = true;
            }

        }

        // Private members

        private sealed class ControlInfo :
            IDisposable {

            // Public members

            public IControlState State { get; }
            public IStyleApplicator StyleApplicator { get; set; }
            public bool StyleInitialized { get; set; } = false;
            public INodeStyleWatcher StyleWatcher { get; set; }

            public ControlInfo(Control control) {

                // Only store visual state for forms, because we don't want to reposition them when resetting the style.

                State = ControlState.Save(control, control is Form ? ControlStateOptions.StoreVisualProperties : ControlStateOptions.Default);

            }

            public void Dispose() {

                StyleWatcher?.Dispose();

            }

        }

        private readonly IDictionary<Control, ControlInfo> controlInfo = new Dictionary<Control, ControlInfo>();
        private readonly IStyleApplicatorFactory styleApplicatorFactory;
        private readonly IMessageFilter messageFilter;
        private bool addedMessageFilter;
        private bool disposedValue;

        private bool HasStyles() {

            return StyleSheets is object && StyleSheets.Any();

        }

        public void ApplyStylesInternal(Control control, bool createStyleWatcher, bool recursive) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            ControlInfo info = new ControlInfo(control) {
                StyleApplicator = styleApplicatorFactory.Create(control.GetType()),
            };

            controlInfo.Add(control, info);

            if (recursive) {

                // Apply styles to all child controls.

                foreach (Control childControl in control.Controls)
                    ApplyStylesInternal(childControl, createStyleWatcher: false, recursive: recursive);

                // The context menu won't be stored in the child control collection, so apply its styles separately.

                if (control.ContextMenuStrip is object)
                    ApplyStylesInternal(control.ContextMenuStrip, createStyleWatcher: false, recursive: recursive);

            }

            // Only top-level controls (i.e. Forms) should have style watchers attached to them.

            if (createStyleWatcher) {

                INodeStyleWatcher styleWatcher = new NodeStyleWatcher(new ControlNode2(control), StyleSheets);

                info.StyleWatcher = styleWatcher;

                styleWatcher.StylesChanged += StylesChangedHandler;

                styleWatcher.InvalidateStyles();

            }

            // Remove the control information we've saved when the control is disposed.

            control.Disposed += ControlDisposedHandler;

        }

        private void ControlDisposedHandler(object sender, EventArgs e) {

            Control control = (Control)sender;

            control.Disposed -= ControlDisposedHandler;

            if (controlInfo.TryGetValue(control, out ControlInfo info)) {

                controlInfo.Remove(control);

                if (info.StyleWatcher is object)
                    info.StyleWatcher.StylesChanged -= StylesChangedHandler;

                info.Dispose();

            }

        }
        private void StylesChangedHandler(object sender, StylesChangedEventArgs e) {

            ControlNode2 node = (ControlNode2)e.Node;

            if (controlInfo.TryGetValue(node.Control, out ControlInfo info)) {

                if (node.Styles.Any() && info.StyleApplicator is object) {

                    // Apply the new styles to the control.

                    if (!info.StyleInitialized)
                        info.StyleApplicator.InitializeStyle(node.Control);

                    IStyleComputationContext context = new StyleComputationContext();

                    info.StyleApplicator.ApplyStyle(node.Control, node.GetComputedStyle(context));

                }
                else if (info.StyleInitialized) {

                    // Restore the control to its default appearance.

                    if (info.StyleApplicator is object)
                        info.StyleApplicator.DeinitializeStyle(node.Control);

                    info.StyleInitialized = false;

                    info.State.Restore(node.Control);

                }

                node.Control.Invalidate();

            }

        }

    }

}