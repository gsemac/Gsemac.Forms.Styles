using Gsemac.Forms.Styles.Dom.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.Dom {

    public class Dom :
        IDom {

        // Public members

        public IDomNode Root { get; }
        public ICollection<IStyleSheet> StyleSheets { get; }

        public Dom(IDomNode root) :
            this(root, Enumerable.Empty<IStyleSheet>()) {
        }
        public Dom(IDomNode root, IStyleSheet styleSheet) :
            this(root, new[] { styleSheet }) {
        }
        public Dom(IDomNode root, IEnumerable<IStyleSheet> styleSheets) {

            if (root is null)
                throw new ArgumentNullException(nameof(root));

            if (styleSheets is null)
                throw new ArgumentNullException(nameof(styleSheets));

            Root = root;
            StyleSheets = new List<IStyleSheet>(styleSheets);

            SubscribeToClassUpdates();
            RecalculateStyles();

        }

        public void SuspendStyleUpdates() {

            styleUpdatesSuspended = true;

        }
        public void ResumeStyleUpdates() {

            styleUpdatesSuspended = false;

            if (modifiedNodes.Any())
                RecalculateStyles();

        }

        public override string ToString() {

            return Root.ToString();

        }

        // Private members

        private sealed class NodeInfo {

            public int Depth { get; set; }
            public IDomNode Node { get; }

            public NodeInfo(IDomNode node) {

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

        private bool styleUpdatesSuspended = false;
        private readonly HashSet<NodeInfo> modifiedNodes = new HashSet<NodeInfo>();

        private void VisitNodes(Func<IDomNode, bool> visitor) {

            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));

            VisitNodes(Root, visitor);

        }
        private void VisitNodes(IDomNode node, Func<IDomNode, bool> visitor) {

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));

            visitor(node);

            foreach (IDomNode childNode in node.Children)
                VisitNodes(childNode, visitor);

        }

        private void SubscribeToClassUpdates() {

            VisitNodes(SubscribeToClassUpdates);

        }
        private bool SubscribeToClassUpdates(IDomNode node) {

            node.ClassAdded += PropertyChangedHandler;
            node.ClassRemoved += PropertyChangedHandler;

            return true;

        }

        private void PropertyChangedHandler(object sender, EventArgs e) {

            // When node properties change in a way that can affect selectors, we need to recalculate styles for all nodes.

            if (sender is IDomNode node)
                modifiedNodes.Add(new NodeInfo(node));

            RecalculateStyles();

        }

        private void AddRelatedNodesToModifiedNodes(NodeInfo nodeInfo) {

            if (nodeInfo is null)
                throw new ArgumentNullException(nameof(nodeInfo));

            // Add all parent nodes.

            IDomNode[] parents = nodeInfo.Node.GetParents().ToArray();
            int currentDepth = parents.Length;

            nodeInfo.Depth = currentDepth;

            foreach (IDomNode parent in parents) {

                modifiedNodes.Add(new NodeInfo(parent) {
                    Depth = --currentDepth,
                });

            }

            // Add all following-sibling nodes (preceding-sibling nodes need not be considered).

            foreach (IDomNode sibling in nodeInfo.Node.GetSiblings()) {

                modifiedNodes.Add(new NodeInfo(sibling) {
                    Depth = nodeInfo.Depth,
                });

            }

            // Add all child nodes.

            foreach (IDomNode child in nodeInfo.Node.Children) {

                modifiedNodes.Add(new NodeInfo(child) {
                    Depth = nodeInfo.Depth + 1,
                });

            }

        }
        private void RecalculateStyles() {

            // If style updates are suspended, don't recalculate styles yet.

            if (styleUpdatesSuspended)
                return;

            // Recalculate styles for all nodes that may need their style updated.
            // This includes any directly-modified nodes, or any parent, following-sibling, or child nodes of a directly-modified node.

            if (!modifiedNodes.Any())
                VisitNodes(Root, node => modifiedNodes.Add(new NodeInfo(node)));

            foreach (NodeInfo modifiedNode in modifiedNodes.ToArray())
                AddRelatedNodesToModifiedNodes(modifiedNode);

            // Relculate styles.
            // Ensure that parent nodes are always updated before their children to allow for easier inheritance of styles.

            foreach (NodeInfo modifiedNode in modifiedNodes.OrderBy(node => node.Depth))
                RecalculateStyles(modifiedNode.Node);

            // Reset the list of modified nodes.

            modifiedNodes.Clear();

        }
        private bool RecalculateStyles(IDomNode node) {

            IEnumerable<IRuleset> styles = new List<IRuleset> { new Ruleset() }; // new List<IRuleset>(StyleSheet.GetRuleset(node));

            // Note that if the style lists are the same, Remove and Add will never be called.
            // This means the cached style returned by CalculateStyle won't be reset, and event handlers won't be called unnecessarily.

            foreach (IRuleset style in node.Styles.Except(styles))
                node.Styles.Remove(style);

            foreach (IRuleset style in styles.Except(node.Styles))
                node.Styles.Add(style);

            return true;

        }

    }

}