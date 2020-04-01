using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class Ruleset :
        IRuleset {

        // Public members

        public ISelector Selector { get; }

        public Ruleset() {
        }
        public Ruleset(string selector) {

            this.Selector = new Selector(selector);

        }

        public void AddProperty(IProperty property) {

            properties[property.Type] = property;

        }
        public void AddProperties(IEnumerable<IProperty> properties) {

            foreach (IProperty property in properties)
                AddProperty(property);

        }
        public void InheritProperties(IEnumerable<IProperty> properties) {

            foreach (IProperty property in properties.Where(p => p.Inheritable))
                AddProperty(property);

        }
        public IProperty GetProperty(PropertyType propertyType) {

            if (properties.TryGetValue(propertyType, out IProperty value))
                return value;

            return null;

        }
        public bool HasProperty(PropertyType propertyType) {

            return properties.ContainsKey(propertyType);

        }

        public IEnumerator<IProperty> GetEnumerator() {

            return properties.Values.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            sb.Append(Selector?.ToString() ?? "");
            sb.AppendLine(" {");

            foreach (IProperty property in properties.Values) {

                sb.Append('\t');
                sb.Append(property.ToString());
                sb.AppendLine(";");

            }

            sb.Append("}");

            return sb.ToString();

        }

        // Private members

        private readonly Dictionary<PropertyType, IProperty> properties = new Dictionary<PropertyType, IProperty>();

    }

}