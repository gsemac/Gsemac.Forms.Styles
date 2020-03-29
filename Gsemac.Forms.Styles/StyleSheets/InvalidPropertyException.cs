using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    [Serializable]
    public class InvalidPropertyException :
        Exception {

        // Public members

        public InvalidPropertyException(string message) :
            base(message) {
        }
        public InvalidPropertyException(string message, Exception innerException) : base(message, innerException) {
        }
        public InvalidPropertyException() {
        }

        // Protected members

        protected InvalidPropertyException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) :
            base(serializationInfo, streamingContext) {
        }

    }

}