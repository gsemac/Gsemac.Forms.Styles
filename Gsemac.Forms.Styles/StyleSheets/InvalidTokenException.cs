using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    [Serializable]
    public class InvalidTokenException :
        Exception {

        // Public members

        public InvalidTokenException(string message) :
            base(message) {
        }
        public InvalidTokenException(string message, Exception innerException) : base(message, innerException) {
        }
        public InvalidTokenException() {
        }

        // Protected members

        protected InvalidTokenException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) :
            base(serializationInfo, streamingContext) {
        }

    }

}