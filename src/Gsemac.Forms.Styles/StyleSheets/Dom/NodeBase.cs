using Gsemac.Collections.Specialized;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Gsemac.Forms.Styles.StyleSheets.Dom {

    public abstract class NodeBase :
        INode2 {

        // Public members

        public event EventHandler<NodeCollectionChangedEventArgs> ChildAdded;
        public event EventHandler<NodeCollectionChangedEventArgs> ChildRemoved;
        public event EventHandler<NodeEventArgs> SelectorChanged;
        public event EventHandler<NodeEventArgs> StylesChanged;

        public string Tag { get; } = string.Empty;
        public string Id { get; protected set; } = string.Empty;
        public INode2 Parent { get; set; }
        public ICollection<INode2> Children => children;
        public ICollection<string> Classes => classes;
        public ICollection<NodeState> States => states;
        public ICollection<IRuleset> Styles => styles;

        public IRuleset GetComputedStyle(IStyleComputationContext context) {

            if (styleIsDirty) {

                style = ComputeStyle(context);

                styleIsDirty = false;

            }

            return style;

        }

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        public override string ToString() {

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings() {
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = Parent is object,
                Indent = true,
            };

            using (StringWriter stringWriter = new StringWriter()) {

                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
                    WriteNode(xmlWriter, this);

                return stringWriter.ToString();

            }

        }

        // Protected members

        protected NodeBase(string tag) {

            Tag = tag;

            // Initialize collections.

            children = new ChildNodeCollection(this);
            classes = new ObservableHashSet<string>();
            states = new ObservableHashSet<NodeState>();
            styles = new ObservableHashSet<IRuleset>(new EquivalentRulesetEqualityComparer());

            AddEventHandlers();

        }

        protected virtual IRuleset ComputeStyle(IStyleComputationContext context) {

            return context.ComputeStyle(this, Styles);

        }

        protected void OnChildAdded(INode2 childNode) {

            if (childNode is null)
                throw new ArgumentNullException(nameof(childNode));

            ChildAdded?.Invoke(this, new NodeCollectionChangedEventArgs(this, childNode));

        }
        protected void OnChildRemoved(INode2 childNode) {

            if (childNode is null)
                throw new ArgumentNullException(nameof(childNode));

            ChildRemoved?.Invoke(this, new NodeCollectionChangedEventArgs(this, childNode));

        }
        protected void OnStylesChanged() {

            StylesChanged?.Invoke(this, new NodeEventArgs(this));

        }
        protected void OnSelectorChanged() {

            SelectorChanged?.Invoke(this, new NodeEventArgs(this));

        }

        protected virtual void Dispose(bool disposing) {

            if (!disposedValue && disposing) {

                // Remove event handlers first so none of the following actions trigger events.

                RemoveEventHandlers();

                // Remove the node from the DOM.

                if (Parent is object)
                    Parent.Children.Remove(this);

                // Dispose of all child nodes.

                INode2[] childrenToDispose = children.ToArray();

                children.Clear();

                foreach (INode2 child in childrenToDispose)
                    child.Dispose();

                disposedValue = true;

            }

        }

        // Private members

        private readonly IObservableCollection<INode2> children;
        private readonly IObservableCollection<string> classes;
        private readonly IObservableCollection<NodeState> states;
        private readonly IObservableCollection<IRuleset> styles;
        private IRuleset style;
        private bool styleIsDirty = true;
        private bool disposedValue = false;

        private void AddEventHandlers() {

            children.CollectionChanged += ChildrenCollectionChangedHandler;
            classes.CollectionChanged += SelectorChangedHandler;
            states.CollectionChanged += SelectorChangedHandler;
            styles.CollectionChanged += StylesChangedHandler;

        }
        private void RemoveEventHandlers() {

            children.CollectionChanged -= ChildrenCollectionChangedHandler;
            classes.CollectionChanged -= SelectorChangedHandler;
            states.CollectionChanged -= SelectorChangedHandler;
            styles.CollectionChanged -= StylesChangedHandler;

        }

        private void ChildrenCollectionChangedHandler(object sender, CollectionChangedEventArgs<INode2> e) {

            if (e.Action == CollectionChangedAction.Add && e.ChangedItems.Any()) {

                OnChildAdded(e.ChangedItems.First());

            }
            else if (e.Action == CollectionChangedAction.Remove && e.ChangedItems.Any()) {

                OnChildRemoved(e.ChangedItems.First());

            }

            OnSelectorChanged();

        }
        private void SelectorChangedHandler(object sender, EventArgs e) {

            OnSelectorChanged();

        }
        private void StylesChangedHandler(object sender, EventArgs e) {

            styleIsDirty = true;

        }

        private static void WriteNode(XmlWriter xmlWriter, INode2 node) {

            if (xmlWriter is null)
                throw new ArgumentNullException(nameof(xmlWriter));

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            xmlWriter.WriteStartElement(node.Tag);

            if (!string.IsNullOrEmpty(node.Id)) {

                xmlWriter.WriteStartAttribute("id");
                xmlWriter.WriteString(node.Id);
                xmlWriter.WriteEndAttribute();

            }

            if (node.Classes.Any()) {

                xmlWriter.WriteStartAttribute("class");
                xmlWriter.WriteString(string.Join(" ", node.Classes));
                xmlWriter.WriteEndAttribute();

            }

            foreach (INode2 childNode in node.Children)
                WriteNode(xmlWriter, childNode);

            xmlWriter.WriteEndElement();

        }

    }

}