using Gsemac.Core;
using Gsemac.Forms.Styles.Applicators2;
using Gsemac.Forms.Styles.Applicators2.Properties;
using Gsemac.Forms.Styles.Applicators2.UserPaint;
using Gsemac.Forms.Styles.Dom;
using Gsemac.Forms.Styles.StyleSheets;
using Gsemac.Forms.Styles.StyleSheets.Dom;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles {

    public class FormsStyleManager :
        IControlStyleManager {

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

            if (options.DefaultStylesEnabled)
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
                ApplyStylesInternal(control, isTopLevelControl: true, recursive: true);

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

        private sealed class NodeData :
            IDisposable {

            // Public members

            public IControlState ControlState { get; }
            public INode2 Node { get; }
            public IStyleApplicator StyleApplicator { get; }
            public IDomSelectorObserver SelectorObserver { get; }

            public bool StyleInitialized { get; set; } = false;

            public NodeData(Control control) {

                if (control is null)
                    throw new ArgumentNullException(nameof(control));

                // Only store visual state for forms, because we don't want to reposition them when resetting the style.

                ControlState = Forms.ControlState.Save(control, new ControlStateOptions() {
                    IncludeLayoutProperties = !(control is Form),
                    IncludeVisualProperties = true,
                });

            }
            public NodeData(Control control, IStyleApplicator styleApplicator) :
                this(control) {

                if (styleApplicator is null)
                    throw new ArgumentNullException(nameof(styleApplicator));

                StyleApplicator = styleApplicator;

            }
            public NodeData(Control control, IStyleApplicator styleApplicator, INode2 domNode) :
                this(control, styleApplicator) {

                if (domNode is null)
                    throw new ArgumentNullException(nameof(domNode));

                Node = domNode;
                SelectorObserver = new DomSelectorObserver(domNode);

            }
            public NodeData(IStyleApplicator styleApplicator) {

                if (styleApplicator is null)
                    throw new ArgumentNullException(nameof(styleApplicator));

                StyleApplicator = styleApplicator;

            }

            public void Dispose() {

                Node?.Dispose();
                SelectorObserver?.Dispose();

            }

        }

        private readonly IDictionary<IComponent, NodeData> componentNodeData = new Dictionary<IComponent, NodeData>();
        private readonly IStyleManagerOptions options = StyleManagerOptions.Default;
        private readonly IMessageFilter messageFilter;
        private IStyleApplicatorFactory styleApplicatorFactory;
        private bool addedMessageFilter;
        private bool disposedValue;

        private bool HasStyleSheets() {

            return StyleSheets is object && StyleSheets.Any();

        }

        private void RestoreControlState(Control control, NodeData controlInfo) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (controlInfo is null)
                throw new ArgumentNullException(nameof(controlInfo));

            // Restore the control to its default appearance.

            if (controlInfo.SelectorObserver is object) {

                controlInfo.SelectorObserver.SelectorChanged -= SelectorChangedHandler;
                controlInfo.SelectorObserver.Dispose();

            }

            if (controlInfo.StyleApplicator is object)
                controlInfo.StyleApplicator.DeinitializeStyle(control);

            controlInfo.StyleInitialized = false;

            controlInfo.ControlState.Restore(control);

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

        private void ApplyStylesInternal(Control control, bool isTopLevelControl, bool recursive) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            // Ignore this control if we've already applied styles to it.

            if (componentNodeData.ContainsKey(control))
                return;

            IStyleApplicator styleApplicator = styleApplicatorFactory.Create(control.GetType());

            NodeData info;

            if (isTopLevelControl) {

                // Only top-level controls (i.e. Forms) should have style watchers attached to them.

                info = new NodeData(control, styleApplicator, new ControlNode2(control));

                info.SelectorObserver.SelectorChanged += SelectorChangedHandler;

                componentNodeData.Add(control, info);

            }
            else {

                info = new NodeData(control, styleApplicator);

                componentNodeData.Add(control, info);

            }

            if (recursive) {

                // Apply styles to all child controls.
                // The child control collection might change for wrapped controls (i.e. with BorderControl), so compute the array ahead of time.

                foreach (Control childControl in GetChildControls(control).ToArray())
                    ApplyStylesInternal(childControl, isTopLevelControl: false, recursive: recursive);

                foreach (ToolTip toolTip in ControlUtilities.GetToolTips(control))
                    componentNodeData.Add(toolTip, new NodeData(styleApplicatorFactory.Create<ToolTip>()));

            }

            if (isTopLevelControl && info.SelectorObserver is object) {

                // Refresh the observer so that styles are updated.

                info.SelectorObserver.Refresh();

            }

            // When new child controls are added, make sure to apply styles to them as well.

            control.ControlAdded += ControlAddedHandler;

            // Remove the control information we've saved if the control is disposed.

            control.Disposed += ControlDisposedHandler;

        }
        private void ResetStylesInternal(Control control, bool recursive) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            bool enableRecursion = false;

            if (componentNodeData.TryGetValue(control, out NodeData nodeData)) {

                enableRecursion = true;

                // Restore the control to its default appearance.

                RestoreControlState(control, nodeData);

                // Some controls need to be invalidated manually to update the appearance (e.g. Panel).

                control.Invalidate();

                // Remove the metadata we have stored.

                componentNodeData.Remove(control);

                nodeData.Dispose();

            }

            // Recurse through controls that are hidden from the DOM, in case their child controls have styles applied to them.
            // See TextBoxUserPaintStyleApplicator for more information on why this might be necessary.

            enableRecursion = enableRecursion || IsDomHidden(control);

            if (enableRecursion && recursive) {

                // Reset styles for all child controls.
                // The child control collection might change for wrapped controls (i.e. with BorderControl), so compute the array ahead of time.

                foreach (Control childControl in GetChildControls(control).ToArray())
                    ResetStylesInternal(childControl, recursive: recursive);

                foreach (ToolTip toolTip in ControlUtilities.GetToolTips(control)) {

                    if (componentNodeData.TryGetValue(toolTip, out NodeData toolTipNodeData)) {

                        if (toolTipNodeData.StyleApplicator is object)
                            toolTipNodeData.StyleApplicator.DeinitializeStyle(toolTip);

                        componentNodeData.Remove(toolTip);

                        toolTipNodeData.Dispose();

                    }

                }

            }

        }

        private void InitializeStyleApplicatorFactory() {

            if (options.CustomRenderingEnabled) {

                styleApplicatorFactory = new UserPaintStyleApplicatorFactory(new UserPaintStyleApplicatorFactoryOptions() {
                    CustomScrollBarsEnabled = options.CustomScrollBarsEnabled,
                });

            }
            else {

                styleApplicatorFactory = new PropertyStyleApplicatorFactory();

            }

        }
        private void InitializeDefaultStyles() {

            IStyleSheetFactory styleSheetFactory = StyleSheetFactory.Default;
            IStyleSheetOptions styleSheetOptions = new StyleSheetOptions() {
                Origin = StyleOrigin.UserAgent,
            };

            StyleSheets.Add(styleSheetFactory.Parse(Properties.StyleSheets.Default, styleSheetOptions));

        }

        private void ControlAddedHandler(object sender, ControlEventArgs e) {

            ApplyStylesInternal(e.Control, isTopLevelControl: e.Control is Form, recursive: true);

        }
        private void ControlDisposedHandler(object sender, EventArgs e) {

            Control control = (Control)sender;

            control.ControlAdded -= ControlAddedHandler;
            control.Disposed -= ControlDisposedHandler;

            if (componentNodeData.TryGetValue(control, out NodeData info)) {

                componentNodeData.Remove(control);

                info.Dispose();

            }

        }
        private void SelectorChangedHandler(object sender, NodeEventArgs e) {

            // We need to decide which styles apply to the current node.
            // It may not have to be redrawn if the styles match the ones it already had applied.

            int oldStylesHashCode = GetStylesHashCode(e.CurrentNode.Styles);

            IEnumerable<IRuleset> styles = StyleSheets.SelectMany(styleSheet => styleSheet.GetStyles(e.CurrentNode))
                .Where(ruleset => ruleset.Any());

            int newStylesHashCode = GetStylesHashCode(styles);

            if (oldStylesHashCode != newStylesHashCode) {

                e.CurrentNode.Styles.Clear();

                foreach (IRuleset style in styles)
                    e.CurrentNode.Styles.Add(style);

                RedrawNode(e.CurrentNode);

            }

        }

        private void RedrawNode(INode2 node) {

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            IComponent component = null;
            Control control = null;

            if (node is ControlNode2 controlNode) {

                component = controlNode.Control;
                control = controlNode.Control;

            }
            else if (node is ToolTipNode toolTipNode) {

                component = toolTipNode.ToolTip;

            }

            if (component is null)
                return;

            if (componentNodeData.TryGetValue(component, out NodeData nodeData)) {

                if (node.Styles.Any(style => !options.RequireNonDefaultStyles || style.Origin != StyleOrigin.UserAgent) && nodeData.StyleApplicator is object) {

                    // Apply the new styles to the control.

                    if (!nodeData.StyleInitialized) {

                        nodeData.StyleApplicator.InitializeStyle(component);

                        nodeData.StyleInitialized = true;

                    }

                    IStyleComputationContext context = new StyleComputationContext();

                    nodeData.StyleApplicator.ApplyStyle(component, node.GetComputedStyle(context));

                }
                else if (nodeData.StyleInitialized) {

                    // Restore the control to its default appearance.

                    if (control is object)
                        RestoreControlState(control, nodeData);

                }

                // Make the control redraw itself.

                if (control is object)
                    control.Invalidate();

            }

        }

        private static int GetStylesHashCode(IEnumerable<IRuleset> styles) {

            if (styles is null)
                throw new ArgumentNullException(nameof(styles));

            IEqualityComparer<IRuleset> styleComparer = new EquivalentRulesetEqualityComparer();

            IHashCodeBuilder styleHashCodeBuilder = new HashCodeBuilder();

            foreach (int hashCode in styles.Select(style => styleComparer.GetHashCode(style)))
                styleHashCodeBuilder.WithValue(hashCode);

            return styleHashCodeBuilder.GetHashCode();

        }
        private static bool IsDomHidden(Control control) {

            return control.GetType()
                .GetCustomAttributes(inherit: true)
                .OfType<DomHiddenAttribute>().Any();

        }

    }

}