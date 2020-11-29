using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public abstract class RulesetBase :
        IRuleset {

        // Public members

        public ISelector Selector { get; }

        public ColorProperty BackgroundColor => GetProperty(PropertyType.BackgroundColor) as ColorProperty;
        public BackgroundImageProperty BackgroundImage => GetProperty(PropertyType.BackgroundImage) as BackgroundImageProperty;
        public ColorProperty BorderBottomColor => GetProperty(PropertyType.BorderBottomColor) as ColorProperty;
        public NumberProperty BorderBottomLeftRadius => GetProperty(PropertyType.BorderBottomLeftRadius) as NumberProperty;
        public NumberProperty BorderBottomRightRadius => GetProperty(PropertyType.BorderBottomRightRadius) as NumberProperty;
        public BorderStyleProperty BorderBottomStyle => GetProperty(PropertyType.BorderBottomStyle) as BorderStyleProperty;
        public NumberProperty BorderBottomWidth => GetProperty(PropertyType.BorderBottomWidth) as NumberProperty;
        public ColorProperty BorderLeftColor => GetProperty(PropertyType.BorderLeftColor) as ColorProperty;
        public BorderStyleProperty BorderLeftStyle => GetProperty(PropertyType.BorderLeftStyle) as BorderStyleProperty;
        public NumberProperty BorderLeftWidth => GetProperty(PropertyType.BorderLeftWidth) as NumberProperty;
        public ColorProperty BorderRightColor => GetProperty(PropertyType.BorderRightColor) as ColorProperty;
        public BorderStyleProperty BorderRightStyle => GetProperty(PropertyType.BorderRightStyle) as BorderStyleProperty;
        public NumberProperty BorderRightWidth => GetProperty(PropertyType.BorderRightWidth) as NumberProperty;
        public ColorProperty BorderTopColor => GetProperty(PropertyType.BorderTopColor) as ColorProperty;
        public NumberProperty BorderTopLeftRadius => GetProperty(PropertyType.BorderTopLeftRadius) as NumberProperty;
        public NumberProperty BorderTopRightRadius => GetProperty(PropertyType.BorderTopRightRadius) as NumberProperty;
        public BorderStyleProperty BorderTopStyle => GetProperty(PropertyType.BorderTopStyle) as BorderStyleProperty;
        public NumberProperty BorderTopWidth => GetProperty(PropertyType.BorderTopWidth) as NumberProperty;
        public ColorProperty Color => GetProperty(PropertyType.Color) as ColorProperty;
        public NumberProperty Opacity => GetProperty(PropertyType.Opacity) as NumberProperty;

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
        protected RulesetBase(ISelector selector) {

            this.Selector = selector;

        }

    }

}