using Gsemac.Core;
using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Gsemac.Forms.Styles.Dom {

    public abstract class DomNodeBase :
        IDomNode {

        // Public members

        public event EventHandler<ClassesChangedEventArgs> ClassAdded;
        public event EventHandler<ClassesChangedEventArgs> ClassRemoved;
        public event EventHandler<StylesChangedEventArgs> StyleAdded;
        public event EventHandler<StylesChangedEventArgs> StyleRemoved;

        public string Tag { get; }
        public string Id { get; protected set; }
        public IDomNode Parent { get; set; }
        public ICollection<string> Classes { get; }
        public ICollection<IDomNode> Children { get; }
        public ICollection<IRuleset> Styles { get; } = new List<IRuleset>();

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

        protected DomNodeBase(string tagName) {

            Tag = tagName;

            ClassCollection classes = new ClassCollection(this);

            classes.ClassAdded += ClassAdded;
            classes.ClassRemoved += ClassRemoved;

            Classes = classes;

            StyleCollection styles = new StyleCollection(this);

            styles.StyleAdded += StyleAdded;
            styles.StyleRemoved += StyleRemoved;

            styles.StyleAdded += StylesChangedHandler;
            styles.StyleRemoved += StylesChangedHandler;

            Styles = Styles;

            style = new ResettableLazy<IRuleset>(CalculateStyleInternal);

            Children = new ChildNodeCollection(this);

        }

        // Private members

        private readonly IResettableLazy<IRuleset> style;

        private IRuleset CalculateStyleInternal() {

            IRuleset ruleset = new Ruleset();

            foreach (IRuleset style in Styles)
                ruleset.AddProperties(style);

            return ruleset;

        }

        private void StylesChangedHandler(object sender, StylesChangedEventArgs e) {

            style.Reset();

        }

        private static void WriteNode(XmlWriter xmlWriter, IDomNode node) {

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

            foreach (IDomNode childNode in node.Children)
                WriteNode(xmlWriter, childNode);

            xmlWriter.WriteEndElement();

        }

    }

}