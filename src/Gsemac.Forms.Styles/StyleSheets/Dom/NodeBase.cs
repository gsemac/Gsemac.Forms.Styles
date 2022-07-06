using Gsemac.Collections.Extensions;
using Gsemac.Collections.Specialized;
using Gsemac.Core;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using Gsemac.Forms.Styles.StyleSheets.Rulesets.Extensions;
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

        public event EventHandler<StyleInvalidatedEventArgs> StyleInvalidated;
        public event EventHandler<StylesChangedEventArgs> StylesChanged;

        public string Tag { get; }
        public string Id { get; protected set; }
        public INode2 Parent { get; set; }
        public ICollection<INode2> Children { get; }
        public ICollection<string> Classes { get; }
        public ICollection<NodeState> States { get; }
        public ICollection<IRuleset> Styles { get; }

        public IRuleset GetComputedStyle() {

            return style.Value;

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

        protected NodeBase(string tagName) {

            Tag = tagName;

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

            children.CollectionChanged += (sender, e) => OnStyleInvalidated();
            classes.CollectionChanged += (sender, e) => OnStyleInvalidated();
            styles.CollectionChanged += (sender, e) => OnStylesChanged();
            states.CollectionChanged += (sender, e) => OnStyleInvalidated();

            styles.CollectionChanged += StylesChangedHandler;

            // Initialize style.

            style = new ResettableLazy<IRuleset>(ComputeStyle);

        }

        protected virtual IRuleset ComputeStyle() {

            IRuleset ruleset = new Ruleset();

            if (Parent is object) {

                ruleset.Inherit(Parent.GetComputedStyle());

            }

            foreach (IRuleset style in Styles)
                ruleset.AddRange(style);

            return ruleset;

        }

        protected void OnStyleInvalidated() {

            OnStyleInvalidated(new StyleInvalidatedEventArgs(this));

        }
        protected void OnStyleInvalidated(StyleInvalidatedEventArgs e) {

            StyleInvalidated?.Invoke(this, e);

        }
        protected void OnStylesChanged() {

            OnStylesChanged(new StylesChangedEventArgs(this));

        }
        protected void OnStylesChanged(StylesChangedEventArgs e) {

            StylesChanged?.Invoke(this, e);

        }

        // Private members

        private readonly IResettableLazy<IRuleset> style;

        private void StylesChangedHandler(object sender, EventArgs e) {

            style.Reset();

        }

        private static void WriteNode(XmlWriter xmlWriter, INode2 node) {

            if (xmlWriter is null)
                throw new ArgumentNullException(nameof(xmlWriter));

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            xmlWriter.WriteStartElement(node.Tag.ToLowerInvariant());

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