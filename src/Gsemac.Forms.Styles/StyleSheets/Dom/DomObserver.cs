﻿using System;

namespace Gsemac.Forms.Styles.StyleSheets.Dom {

    public class DomObserver :
        IDomObserver {

        // Public members

        public event EventHandler<NodeCollectionChangedEventArgs> ChildAdded;
        public event EventHandler<NodeCollectionChangedEventArgs> ChildRemoved;
        public event EventHandler<NodeEventArgs> StylesChanged;
        public event EventHandler<NodeEventArgs> SelectorChanged;

        public DomObserver(INode2 rootNode) {

            if (rootNode is null)
                throw new ArgumentNullException(nameof(rootNode));

            this.rootNode = rootNode;

            AddEventHandlers(rootNode, recursive: true);

        }

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Protected members

        protected virtual void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    // Remove all event handlers.
                    // Ignore event handlers 

                    ignoreEventHandlers = true;
                    //foreach (INode2 node in added.ToArray())
                    //    RemoveEventHandlers(node, recursive: false);
                    RemoveEventHandlers(rootNode, recursive: true);

                }

                disposedValue = true;

            }

        }

        // Private members

        private readonly INode2 rootNode;
        private bool ignoreEventHandlers = false;
        private bool disposedValue;

        private void EnsureNotDisposed() {

            // This is called at the beginning of all event handlers, and ensures that event handlers don't accidentally silently linger.
            // I encountered a bug earlier that was causing event handlers to accumulate because they weren't being removed properly.

            if (disposedValue)
                throw new ObjectDisposedException(nameof(DomObserver));

        }

        private void AddEventHandlers(INode2 node, bool recursive) {

            if (ignoreEventHandlers)
                return;

            // Remove event handlers before adding them to ensure that we never add them more than once per node.

            RemoveEventHandlers(node, recursive: true);

            node.ChildAdded += ChildAddedHandler;
            node.ChildRemoved += ChildRemovedHandler;
            node.SelectorChanged += SelectorChangedHandler;
            node.StylesChanged += StylesChangedHandler;

            if (recursive) {

                foreach (INode2 child in node.Children)
                    AddEventHandlers(child, recursive);

            }

        }
        private void RemoveEventHandlers(INode2 node, bool recursive) {

            node.ChildAdded -= ChildAddedHandler;
            node.ChildRemoved -= ChildRemovedHandler;
            node.SelectorChanged -= SelectorChangedHandler;
            node.StylesChanged -= StylesChangedHandler;

            if (recursive) {

                foreach (INode2 child in node.Children)
                    RemoveEventHandlers(child, recursive);

            }

        }

        private void ChildAddedHandler(object sender, NodeCollectionChangedEventArgs e) {

            EnsureNotDisposed();

            if (ignoreEventHandlers)
                return;

            // Add event handlers to the DOM node that was just added.

            AddEventHandlers(e.AffectedNode, recursive: true);

            ChildAdded?.Invoke(this, e);

        }
        private void ChildRemovedHandler(object sender, NodeCollectionChangedEventArgs e) {

            EnsureNotDisposed();

            if (ignoreEventHandlers)
                return;

            // Remove event handlers to the DOM node that was just removed.

            RemoveEventHandlers(e.AffectedNode, recursive: true);

            ChildRemoved?.Invoke(this, e);

        }
        private void SelectorChangedHandler(object sender, NodeEventArgs e) {

            EnsureNotDisposed();

            if (ignoreEventHandlers)
                return;

            SelectorChanged?.Invoke(this, e);

        }
        private void StylesChangedHandler(object sender, NodeEventArgs e) {

            EnsureNotDisposed();

            if (ignoreEventHandlers)
                return;

            StylesChanged?.Invoke(this, e);

        }

    }

}