using Gsemac.Forms.Styles.StyleSheets.Dom.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Dom {

    public class DomSelectorObserver :
        IDomSelectorObserver {

        // Public members

        public event EventHandler<NodeEventArgs> SelectorChanged;

        public DomSelectorObserver(INode2 rootNode) {

            if (rootNode is null)
                throw new ArgumentNullException(nameof(rootNode));

            this.rootNode = rootNode;
            domObserver = new DomObserver(this.rootNode);

            domObserver.SelectorChanged += SelectorChangedHandler;

        }

        public void Refresh() {

            // Add all nodes to the list of modified nodes.

            InvalidateStylesRecursive(rootNode, 0, invalidateRelatedNodes: false);

            TriggerEvents();

        }

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Protected members

        private void OnSelectorChanged(INode2 node) {

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            SelectorChanged?.Invoke(this, new NodeEventArgs(node));

        }

        protected virtual void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    domObserver.Dispose();

                }

                disposedValue = true;

            }
        }

        // Private members

        private sealed class NodeInfo {

            public int Depth { get; }
            public INode2 Node { get; }

            public NodeInfo(INode2 node, int depth) {

                if (node is null)
                    throw new ArgumentNullException(nameof(node));

                Node = node;
                Depth = depth;

            }

            public override bool Equals(object obj) {

                if (!(obj is NodeInfo nodeInfo))
                    return false;

                return ReferenceEqualityComparer.Instance.Equals(Node, nodeInfo.Node);

            }
            public override int GetHashCode() {

                return ReferenceEqualityComparer.Instance.GetHashCode(Node);

            }

        }

        private readonly INode2 rootNode;
        private readonly IDomObserver domObserver;
        private readonly HashSet<NodeInfo> modifiedNodes = new HashSet<NodeInfo>();
        private bool disposedValue;

        private void SelectorChangedHandler(object sender, NodeEventArgs e) {

            if (disposedValue)
                throw new ObjectDisposedException(nameof(DomSelectorObserver));

            // When node properties change in a way that can affect selectors, we need to recalculate styles for all nodes.

            InvalidateStyles(e.CurrentNode, invalidateRelatedNodes: true);

            TriggerEvents();

        }

        private void InvalidateStyles(INode2 node, bool invalidateRelatedNodes) {

            InvalidateStyles(node, node.GetAncestors().Count(), invalidateRelatedNodes);

        }
        private void InvalidateStyles(INode2 node, int depth, bool invalidateRelatedNodes) {

            lock (modifiedNodes)
                modifiedNodes.Add(new NodeInfo(node, depth));

            if (invalidateRelatedNodes) {

                // Invalidate all ancestor nodes.

                int parentsDepth = depth;

                foreach (INode2 ancestor in node.GetAncestors())
                    InvalidateStyles(ancestor, --parentsDepth, invalidateRelatedNodes: false);

                // Invalidate all following-sibling nodes (preceding-sibling nodes need not be considered).

                foreach (INode2 sibling in node.GetSiblings())
                    InvalidateStyles(sibling, depth, invalidateRelatedNodes: false);

                // Invalidate all child nodes.

                foreach (INode2 child in node.Children)
                    InvalidateStyles(child, depth + 1, invalidateRelatedNodes: false);

            }

        }
        private void InvalidateStylesRecursive(INode2 node, int depth, bool invalidateRelatedNodes) {

            InvalidateStyles(node, depth, invalidateRelatedNodes);

            foreach (INode2 child in node.Children)
                InvalidateStylesRecursive(child, depth + 1, invalidateRelatedNodes);

        }

        private void TriggerEvents() {

            // Event handlers may trigger changes that cause the visual state of nodes to be changed.
            // We only want to process the modified nodes already in the list.

            NodeInfo[] modifiedNodesArray;

            lock (modifiedNodes) {

                modifiedNodesArray = modifiedNodes.OrderBy(node => node.Depth).ToArray();
                modifiedNodes.Clear();

            }

            foreach (NodeInfo nodeInfo in modifiedNodesArray)
                OnSelectorChanged(nodeInfo.Node);

        }

    }

}