﻿namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class PropertyFactory :
        PropertyFactoryBase {

        // Public members

        public static PropertyFactory Default { get; } = new PropertyFactory();

        public PropertyFactory() { }
        public PropertyFactory(IPropertyFactoryOptions options) :
            base(options) {
        }

    }

}