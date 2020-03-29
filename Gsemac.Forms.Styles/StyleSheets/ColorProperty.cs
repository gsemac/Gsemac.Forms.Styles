﻿using Gsemac.Forms.Styles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class ColorProperty :
        PropertyBase<Color> {

        public ColorProperty(PropertyType propertyType, Color propertyValue) :
            base(propertyType, propertyValue) {
        }

        public override string ToString() {

            return ToString(ColorTranslator.ToHtml(Value).ToLowerInvariant());

        }

    }

}