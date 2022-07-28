using Gsemac.Forms.Styles.Applicators2;
using Gsemac.Forms.Styles.Dom;
using Gsemac.Forms.Styles.StyleSheets;
using Gsemac.Forms.Styles.StyleSheets.Dom;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles {

    public class FormsStyleManager :
        IStyleManager {

        // Public members

        public ICollection<IStyleSheet> StyleSheets { get; } = new List<IStyleSheet>();

        public FormsStyleManager() :
            this(StyleManagerOptions.Default) {

        }
        public FormsStyleManager(IStyleManagerOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            this.options = options;

            messageFilter = new StyleManagerMessageFilter(this);

            if (options.EnableDefaultStyles)
                InitializeDefaultStyles();

        }

        public void ApplyStyles() {

            InitializeStyleApplicatorFactory();

            if (!HasStyleSheets()) {

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

            InitializeStyleApplicatorFactory();

            // Clear all existing styles for this control before attempting to apply them.

            ResetStyles(control);

            if (HasStyleSheets())
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

            ResetStylesInternal(control, recursive: true);

        }

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Protected members

        protected virtual void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    ResetStyles();

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
        private readonly IStyleManagerOptions options = StyleManagerOptions.Default;
        private readonly IMessageFilter messageFilter;
        private IStyleApplicatorFactory styleApplicatorFactory;
        private bool addedMessageFilter;
        private bool disposedValue;

        private bool HasStyleSheets() {

            return StyleSheets is object && StyleSheets.Any();

        }

        private void RestoreControlState(Control control, ControlInfo controlInfo) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (controlInfo is null)
                throw new ArgumentNullException(nameof(controlInfo));

            // Restore the control to its default appearance.

            if (controlInfo.StyleApplicator is object)
                controlInfo.StyleApplicator.DeinitializeStyle(control);

            controlInfo.StyleInitialized = false;

            controlInfo.State.Restore(control);


        }
        private IEnumerable<Control> GetChildControls(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            foreach (Control childControl in control.Controls)
                yield return childControl;

            // The context menu isn't stored in the child control collection.

            if (control.ContextMenuStrip is object)
                yield return control.ContextMenuStrip;

        }

        private void ApplyStylesInternal(Control control, bool createStyleWatcher, bool recursive) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            ControlInfo info = new ControlInfo(control) {
                StyleApplicator = styleApplicatorFactory.Create(control.GetType()),
            };

            controlInfo.Add(control, info);

            if (recursive) {

                // Apply styles to all child controls.

                foreach (Control childControl in GetChildControls(control))
                    ApplyStylesInternal(childControl, createStyleWatcher: false, recursive: recursive);

            }

            // Only top-level controls (i.e. Forms) should have style watchers attached to them.

            if (createStyleWatcher) {

                INodeStyleWatcher styleWatcher = new NodeStyleWatcher(new ControlNode2(control), StyleSheets);

                info.StyleWatcher = styleWatcher;

                styleWatcher.StylesChanged += StylesChangedHandler;

                styleWatcher.InvalidateStyles();

            }

            // Remove the control information we've saved if the control is disposed.

            control.Disposed += ControlDisposedHandler;

        }
        private void ResetStylesInternal(Control control, bool recursive) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (controlInfo.TryGetValue(control, out ControlInfo info)) {

                // Restore the control to its default appearance.

                RestoreControlState(control, info);

                // Some controls need to be invalidated manually to update the appearance (e.g. Panel).

                control.Invalidate();

                // Remove the metadata we have stored.

                controlInfo.Remove(control);

                info.Dispose();

                if (recursive) {

                    // Reset styles for all child controls.

                    foreach (Control childControl in GetChildControls(control))
                        ResetStylesInternal(childControl, recursive: recursive);

                }

            }

        }

        private void InitializeStyleApplicatorFactory() {

            if (options.EnableCustomRendering)
                styleApplicatorFactory = new UserPaintStyleApplicatorFactory();
            else
                styleApplicatorFactory = new PropertyStyleApplicatorFactory();

        }
        private void InitializeDefaultStyles() {

            IStyleSheetFactory styleSheetFactory = StyleSheetFactory.Default;
            IStyleSheetOptions styleSheetOptions = new StyleSheetOptions() {
                Origin = StyleOrigin.UserAgent,
            };

            StyleSheets.Add(styleSheetFactory.Parse(Properties.StyleSheets.Default, styleSheetOptions));

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

                    RestoreControlState(node.Control, info);

                }

                node.Control.Invalidate();

            }

        }

    }

}