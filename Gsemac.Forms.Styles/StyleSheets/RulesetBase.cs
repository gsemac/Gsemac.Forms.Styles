using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public abstract class RulesetBase :
        IRuleset {

        // Public members

        public ISelector Selector { get; }

        public ColorProperty BackgroundColor => GetProperty(PropertyType.BackgroundColor) as ColorProperty;
        public ColorProperty BorderColor => GetProperty(PropertyType.BorderColor) as ColorProperty;
        public NumberProperty BorderWidth => GetProperty(PropertyType.BorderWidth) as NumberProperty;
        public BorderRadiusProperty BorderRadius => GetProperty(PropertyType.BorderRadius) as BorderRadiusProperty;
        public ColorProperty Color => GetProperty(PropertyType.Color) as ColorProperty;

        public abstract void AddProperty(IProperty property);
        public void AddProperties(IEnumerable<IProperty> properties) {

            foreach (IProperty property in properties)
                AddProperty(property);

        }
        public void InheritProperties(IEnumerable<IProperty> properties) {

            foreach (IProperty property in properties.Where(p => p.Inheritable))
                if (!HasProperty(property.Type))
                    AddProperty(property);

        }
        public abstract IProperty GetProperty(PropertyType propertyType);
        public virtual bool HasProperty(PropertyType propertyType) {

            return GetProperty(propertyType) != null;

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            sb.Append(Selector?.ToString() ?? "");
            sb.AppendLine(" {");

            foreach (IProperty property in this) {

                sb.Append('\t');
                sb.Append(property.ToString());
                sb.AppendLine(";");

            }

            sb.Append("}");

            return sb.ToString();

        }

        public abstract IEnumerator<IProperty> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Protected members

        protected RulesetBase() {
        }
        protected RulesetBase(string selector) {

            this.Selector = new Selector(selector);

        }

    }

}