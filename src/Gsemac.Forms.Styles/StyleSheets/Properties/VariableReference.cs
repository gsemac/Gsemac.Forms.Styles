using Gsemac.Forms.Styles.Properties;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public sealed class VariableReference {

        // Public members

        public string Name { get; }

        public VariableReference(string variableName) {

            if (variableName is null)
                throw new ArgumentNullException(nameof(variableName));

            // Note that variable names are case-sensitive.

            variableName = variableName.Trim();

            if (string.IsNullOrWhiteSpace(variableName) || !variableName.StartsWith("--"))
                throw new ArgumentException(ExceptionMessages.InvalidVariableName, nameof(variableName));

            Name = variableName;

        }

        public override string ToString() {

            return $"var({Name})";

        }

    }

}