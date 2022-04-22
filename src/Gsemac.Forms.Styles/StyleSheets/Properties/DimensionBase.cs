using Gsemac.Core;
using Gsemac.Forms.Styles.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public abstract class DimensionBase :
        IDimension {

        // Public members

        public string Unit { get; }
        public double Value { get; }

        public override int GetHashCode() {

            return new HashCodeBuilder()
                .Add(Unit)
                .Add(Value)
                .GetHashCode();

        }
        public override bool Equals(object obj) {

            if (obj is IDimension otherDimension) {

                return otherDimension.Value.Equals(Value) &&
                    otherDimension.Unit.Equals(Unit);

            }

            return false;

        }

        public override string ToString() {

            return $"{Value}{Unit}";

        }

        // Protected members

        protected DimensionBase(double value, string unit) {

            if (unit is null)
                throw new ArgumentNullException(nameof(unit));

            if (!string.IsNullOrWhiteSpace(unit))
                unit = unit.Trim().ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(unit) || !GetValidUnits().Contains(unit))
                throw new ArgumentException(string.Format(ExceptionMessages.UnrecognizedUnits, unit), nameof(unit));

            Value = value;
            Unit = unit;

        }

        protected abstract IEnumerable<string> GetValidUnits();

    }

}