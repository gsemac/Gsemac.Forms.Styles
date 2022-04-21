using Gsemac.Forms.Styles.StyleSheets.Dom.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Dom {

    public sealed class NodeStyleWatcher :
        INodeStyleWatcher {

        // Public members

        public event EventHandler<StyleInvalidatedEventArgs> StyleInvalidated;
        public event EventHandler<StylesChangedEventArgs> StylesChanged;

        public NodeStyleWatcher(INode2 root) :
            this(root, Enumerable.Empty<IStyleSheet>()) {
        }
        public NodeStyleWatcher(INode2 root, IStyleSheet styleSheet) :
            this(root, new[] { styleSheet }) {
        }
        public NodeStyleWatcher(INode2 root, IEnumerable<IStyleSheet> styleSheets) {

            if (root is null)
                throw new ArgumentNullException(nameof(root));

            if (styleSheets is null)
                throw new ArgumentNullException(nameof(styleSheets));

            rootNode = root;
            this.styleSheets = new List<IStyleSheet>(styleSheets);

            AddEventHandlers();

        }
        public void InvalidateStyles() {

            ComputeStyles();

        }

        public void SuspendStyleUpdates() {

            styleUpdatesSuspended = true;

        }
        public void ResumeStyleUpdates() {

            styleUpdatesSuspended = false;

            if (modifiedNodes.Any())
                ComputeStyles();

        }

        public void Dispose() {

            if (!disposedValue) {

                RemoveEventHandlers();

                disposedValue = true;

            }

        }

        // Protected members

        private void OnStyleInvalidated(StyleInvalidatedEventArgs e) {

            StyleInvalidated?.Invoke(this, e);

        }
        private void OnStylesChanged(StylesChangedEventArgs e) {

            StylesChanged?.Invoke(this, e);

        }

        // Private members

        private sealed class NodeInfo {

            public int Depth { get; set; }
            public INode2 Node { get; }

            public NodeInfo(INode2 node) {

                Node = node;

            }

            public override bool Equals(object obj) {

                return obj.GetHashCode().Equals(GetHashCode());

            }
            public override int GetHashCode() {

                return Node.GetHashCode();

            }
            public override string ToString() {

                return Node.ToString();

            }

        }

        private readonly INode2 rootNode;
        private readonly IList<IStyleSheet> styleSheets;
        private readonly HashSet<NodeInfo> modifiedNodes = new HashSet<NodeInfo>();
        private bool styleUpdatesSuspended;
        private bool disposedValue;

        private void VisitNodes(Func<INode2, bool> visitor) {

            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));

            VisitNodes(rootNode, visitor);

        }
        private void VisitNodes(INode2 node, Func<INode2, bool> visitor) {

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));

            visitor(node);

            foreach (INode2 childNode in node.Children)
                VisitNodes(childNode, visitor);

        }

        private void AddEventHandlers() {

            VisitNodes(AddEventHandlers);

        }
        private bool AddEventHandlers(INode2 node) {

            node.StyleInvalidated += StyleInvalidatedHandler;
            node.StylesChanged += StylesChangedHandler;

            return true;

        }
        private void RemoveEventHandlers() {

            VisitNodes(RemoveEventHandlers);

        }
        private bool RemoveEventHandlers(INode2 node) {

            node.StyleInvalidated -= StyleInvalidatedHandler;
            node.StylesChanged -= StylesChangedHandler;

            return true;

        }

        private void StyleInvalidatedHandler(object sender, StyleInvalidatedEventArgs e) {

            // When node properties change in a way that can affect selectors, we need to recalculate styles for all nodes.

            if (sender is INode2 node)
                modifiedNodes.Add(new NodeInfo(node));

            OnStyleInvalidated(e);

            ComputeStyles();

        }
        private void StylesChangedHandler(object sender, StylesChangedEventArgs e) {

            OnStylesChanged(e);

        }

        private void AddRelatedNodesToModifiedNodes(NodeInfo nodeInfo) {

            if (nodeInfo is null)
                throw new ArgumentNullException(nameof(nodeInfo));

            // Add all parent nodes.

            INode2[] parents = nodeInfo.Node.GetParents().ToArray();
            int currentDepth = parents.Length;

            nodeInfo.Depth = currentDepth;

            foreach (INode2 parent in parents) {

                modifiedNodes.Add(new NodeInfo(parent) {
                    Depth = --currentDepth,
                });

            }

            // Add all following-sibling nodes (preceding-sibling nodes need not be considered).

            foreach (INode2 sibling in nodeInfo.Node.GetSiblings()) {

                modifiedNodes.Add(new NodeInfo(sibling) {
                    Depth = nodeInfo.Depth,
                });

            }

            // Add all child nodes.

            foreach (INode2 child in nodeInfo.Node.Children) {

                modifiedNodes.Add(new NodeInfo(child) {
                    Depth = nodeInfo.Depth + 1,
                });

            }

        }

        private void ComputeStyles() {

            // If style updates are suspended, don't recalculate styles yet.

            if (styleUpdatesSuspended)
                return;

            // Recalculate styles for all nodes that may need their style updated.
            // This includes any directly-modified nodes, or any parent, following-sibling, or child nodes of a directly-modified node.

            if (!modifiedNodes.Any())
                VisitNodes(rootNode, node => modifiedNodes.Add(new NodeInfo(node)));

            foreach (NodeInfo modifiedNode in modifiedNodes.ToArray())
                AddRelatedNodesToModifiedNodes(modifiedNode);

            // Relculate styles.
            // Ensure that parent nodes are always updated before their children to allow for easier inheritance of styles.

            foreach (NodeInfo modifiedNode in modifiedNodes.OrderBy(node => node.Depth))
                ComputeStyles(modifiedNode.Node);

            // Reset the list of modified nodes.

            modifiedNodes.Clear();

        }
        private bool ComputeStyles(INode2 node) {

            IEnumerable<IRuleset> styles = styleSheets.SelectMany(styleSheet => styleSheet.GetRulesets(node))
                .Where(ruleset => ruleset.Any());

            if (!styles.Any()) {

                node.Styles.Clear();

            }
            else {

                // Note that if the style lists are the same, Remove and Add will never be called.
                // This means the cached style returned by CalculateStyle won't be reset, and event handlers won't be called unnecessarily.

                IEqualityComparer<IRuleset> rulesetComparer = new EquivalentRulesetEqualityComparer();

                // #TODO This can cause StylesChanged to get called twice, could be more efficient

                foreach (IRuleset style in node.Styles.Except(styles, rulesetComparer).ToArray())
                    node.Styles.Remove(style);

                foreach (IRuleset style in styles.Except(node.Styles, rulesetComparer).ToArray())
                    node.Styles.Add(style);

            }

            return true;

        }

    }

}