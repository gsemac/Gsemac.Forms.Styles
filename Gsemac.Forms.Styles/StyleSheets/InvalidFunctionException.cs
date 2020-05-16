using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    [Serializable]
    public class InvalidFunctionException :
    Exception {

        // Public members

        public InvalidFunctionException(string message) :
            base(message) {
        }
        public InvalidFunctionException(string message, Exception innerException) : base(message, innerException) {
        }
        public InvalidFunctionException() {
        }

        // Protected members

        protected InvalidFunctionException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) :
            base(serializationInfo, streamingContext) {
        }

    }

}
