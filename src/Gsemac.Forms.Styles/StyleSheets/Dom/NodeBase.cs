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

        public string Tag { get; }
        public string Id { get; protected set; }
        public INode2 Parent { get; set; }
        public ICollection<INode2> Children { get; }
        public ICollection<string> Classes { get; }
        public ICollection<NodeState> States { get; }
        public ICollection<IRuleset> Styles { get; }

        public IRuleset GetComputedStyle(IStyleComputationContext context) {

            if (styleIsDirty) {

                style = ComputeStyle(context);

                styleIsDirty = false;

            }

            return style;

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

            IObservableCollection<INode2> children = new ChildNodeCollection(this);
            IObservableCollection<string> classes = new ObservableHashSet<string>();
            IObservableCollection<NodeState> states = new ObservableHashSet<NodeState>();
            IObservableCollection<IRuleset> styles = new ObservableHashSet<IRuleset>(new EquivalentRulesetEqualityComparer());

            Classes = classes;
            Children = children;
            States = states;
            Styles = styles;

            // Register event handlers.

            children.CollectionChanged += ChildrenCollectionChangedHandler;
            classes.CollectionChanged += SelectorChangedHandler;
            states.CollectionChanged += SelectorChangedHandler;
            styles.CollectionChanged += StylesChangedHandler;

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

        // Private members

        private IRuleset style;
        bool styleIsDirty = true;

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