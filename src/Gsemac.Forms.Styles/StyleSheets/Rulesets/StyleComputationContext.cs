using Gsemac.Collections.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Dom;
using Gsemac.Forms.Styles.StyleSheets.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Rulesets {

    internal class StyleComputationContext :
        IStyleComputationContext {

        // Public members

        public StyleComputationContext() :
            this(SystemColorPalette.Default, PropertyFactory.Default) {
        }
        public StyleComputationContext(ISystemColorPalette systemColorPalette, IPropertyFactory propertyFactory) {

            if (systemColorPalette is null)
                throw new ArgumentNullException(nameof(systemColorPalette));

            if (propertyFactory is null)
                throw new ArgumentNullException(nameof(propertyFactory));

            this.systemColorPalette = systemColorPalette;
            this.propertyFactory = propertyFactory;

        }

        public IProperty ComputeProperty(IProperty property, INode2 node, IEnumerable<IRuleset> styles) {

            if (property is null)
                throw new ArgumentNullException(nameof(property));

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            if (styles is null)
                throw new ArgumentNullException(nameof(styles));

            // Compute the property value in a loop, because variable references might resolve to keywords and vice-versa.

            while (IsVariableReference(property) || IsKeywordReference(property)) {

                if (IsVariableReference(property))
                    property = ResolveVariableReference(property, styles);

                if (IsKeywordReference(property))
                    property = ResolveKeywordReference(property, node, styles);

            }

            return property;

        }
        public IRuleset ComputeStyle(INode2 node, IEnumerable<IRuleset> styles) {

            IRuleset computedStyle = new Ruleset();

            // Assume the styles are already ordered by origin and specificity, and apply to the current node.
            // We'll take the last instance of each property specified.

            foreach (IProperty property in styles.SelectMany(style => style).Reverse()) {

                if (computedStyle.ContainsKey(property.Name))
                    continue;

                IProperty computedProperty = ComputeProperty(property, node, styles);

                computedStyle.Add(computedProperty);

            }

            // Inherit any inheritable properties from the parent node that haven't been specified yet.

            if (node.Parent is object) {

                IEnumerable<IProperty> inheritedProperties = node.Parent.GetComputedStyle(this)
                    .Where(property => property.Inherited);

            }

            return computedStyle;

        }

        // Private members

        private readonly ISystemColorPalette systemColorPalette;
        private readonly IPropertyFactory propertyFactory;

        private IProperty ResolveVariableReference(IProperty property, IEnumerable<IRuleset> styles) {

            if (property is null)
                throw new ArgumentNullException(nameof(property));

            if (styles is null)
                throw new ArgumentNullException(nameof(styles));

            HashSet<string> seenVariables = new HashSet<string>();

            // The variable may resolve to another variable reference, so this process is recursive.

            while (IsVariableReference(property)) {

                // Get the name of the referenced variable.

                string variableName = property.Value.As<string>();

                if (!PropertyUtilities.IsVariable(variableName))
                    return property;

                seenVariables.Add(variableName);

                // Find the value of the referenced variable.

                IProperty variableDefinition = styles
                    .Reverse()
                    .Select(style => style.TryGetValue(variableName, out IProperty p) ? p : null)
                    .Where(p => p is object)
                    .FirstOrDefault();

                // If there is no matching variable definition, just reset the property.

                if (variableDefinition is null)
                    return propertyFactory.Create(property.Name, PropertyValue.Unset);

                IPropertyValue variableValue = variableDefinition.Value;

                // If the value of this variable is another variable we've encountered already, we have a cycle.
                // If we find a cycle, just reset the property.

                if (variableValue.IsVariableReference && seenVariables.Contains(variableValue.As<string>()))
                    return propertyFactory.Create(property.Name, PropertyValue.Unset);

                // Create a new property with the new value.

                property = propertyFactory.Create(property.Name, variableValue);

            }

            return property;

        }

        private IProperty ResolveKeywordReference(IProperty property, INode2 node, IEnumerable<IRuleset> styles) {

            if (property is null)
                throw new ArgumentNullException(nameof(property));

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            if (styles is null)
                throw new ArgumentNullException(nameof(styles));

            HashSet<string> seenKeywords = new HashSet<string>();

            // The variable may resolve to another keyword, so this process is recursive.

            while (IsKeywordReference(property)) {

                // Get the name of the keyword.

                string keyword = property.Value.As<string>();

                seenKeywords.Add(keyword);

                // Get the value associated with the keyword.

                switch (keyword.ToLowerInvariant()) {

                    case Keyword.Inherit:
                        property = ResolveInheritKeyword(property, node);
                        break;

                    case Keyword.Initial:
                        property = ResolveInitialKeyword(property, styles);
                        break;

                    case Keyword.Revert:
                        property = ResolveRevertKeyword(property, styles);
                        break;

                    case Keyword.Unset:
                        property = ResolveUnsetKeyword(property);
                        break;

                    case Keyword.CanvasText:
                        property = propertyFactory.Create(property.Name, systemColorPalette.CanvasText);
                        break;

                    case Keyword.CurrentColor:
                        property = ResolveCurrentColorKeyword(property, styles);
                        break;

                }

                if (property.Value.IsKeyword && seenKeywords.Contains(property.Value.As<string>())) {

                    // If we've found ourselves in a cycle of keywords, we'll return a default value for the property.
                    // This might happen if "initial" is resolved as "revert" (due to no initial value being specified in the property definition),
                    // but the user agent doesn't specify a value for a property, causing it to get resolved back to "initial".
                    // With well-defined properties, this shouldn't occur.

                    IPropertyValue initialPropertyValue = PropertyValue.Create(property.ValueType, Activator.CreateInstance(property.ValueType));

                    return propertyFactory.Create(property.Name, initialPropertyValue);

                }

            }

            return property;

        }

        private IProperty ResolveInheritKeyword(IProperty property, INode2 node) {

            if (property is null)
                throw new ArgumentNullException(nameof(property));

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            // If the property value is not the inherit keyword, ignore it.

            if (!property.Value.Equals(PropertyValue.Inherit))
                return property;

            // Get the property value from the parent node.
            // If the node doesn't have a parent or the parent doesn't have the requested property, fall back to the property's initial value.

            IProperty parentProperty = null;

            if (!node.Parent?.GetComputedStyle(this).TryGetValue(property.Name, out parentProperty) ?? false)
                return propertyFactory.Create(property.Name);

            // Return the new property, resolving further if necessary.

            return ResolveInheritKeyword(parentProperty, node.Parent);

        }
        private IProperty ResolveInitialKeyword(IProperty property, IEnumerable<IRuleset> styles) {

            if (property is null)
                throw new ArgumentNullException(nameof(property));

            if (styles is null)
                throw new ArgumentNullException(nameof(styles));

            // Create an instance of the property with its initial value.

            IProperty initialProperty = propertyFactory.Create(property.Name);

            if (initialProperty.Value.Equals(PropertyValue.Initial)) {

                // If the initial value is the "initial" keyword, revert to the user-agent-supplied value.
                // This scenario ideally shouldn't occur, but it could come up for improper property defintions.

                initialProperty = propertyFactory.Create(property.Name, PropertyValue.Revert);

            }

            return initialProperty;

        }
        private IProperty ResolveRevertKeyword(IProperty property, IEnumerable<IRuleset> styles) {

            if (property is null)
                throw new ArgumentNullException(nameof(property));

            if (styles is null)
                throw new ArgumentNullException(nameof(styles));

            IProperty userAgentProperty = styles
                .Reverse()
                .Where(style => style.Origin == StyleOrigin.UserAgent)
                .Select(style => style.TryGetValue(property.Name, out IProperty value) ? value : null)
                .Where(p => p is object)
                .FirstOrDefault();

            // If there's nothing to revert back to, use the property's initial value.

            if (userAgentProperty is null)
                return propertyFactory.Create(property.Name, PropertyValue.Initial);

            return userAgentProperty;

        }
        private IProperty ResolveUnsetKeyword(IProperty property) {

            if (property is null)
                throw new ArgumentNullException(nameof(property));

            // If the property is inheritable, inherit; otherwise, use the initial value.

            if (property.Inherited)
                return propertyFactory.Create(property.Name, PropertyValue.Inherit);

            return propertyFactory.Create(property.Name, PropertyValue.Initial);

        }

        private IProperty ResolveCurrentColorKeyword(IProperty property, IEnumerable<IRuleset> styles) {

            if (property is null)
                throw new ArgumentNullException(nameof(property));

            if (styles is null)
                throw new ArgumentNullException(nameof(styles));

            IProperty colorProperty = styles
                .Reverse()
                .Select(style => style.TryGetValue(PropertyName.Color, out IProperty value) ? value : null)
                .Where(p => p is object)
                .FirstOrDefault();

            if (colorProperty is null)
                return propertyFactory.Create(property.Name, PropertyValue.Revert);

            return propertyFactory.Create(property.Name, colorProperty.Value);

        }

        private static bool IsVariableReference(IProperty property) {

            if (property is null)
                throw new ArgumentNullException(nameof(property));

            return property.Value.IsVariableReference;

        }
        private static bool IsKeywordReference(IProperty property) {

            if (property is null)
                throw new ArgumentNullException(nameof(property));

            return property.Value.IsKeyword;

        }

    }

}