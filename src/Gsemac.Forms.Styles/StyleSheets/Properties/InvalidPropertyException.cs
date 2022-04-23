using System;
using System.Runtime.Serialization;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    [Serializable]
    public class InvalidPropertyException :
        Exception {

        // Public members

        public InvalidPropertyException() {
        }
        public InvalidPropertyException(string message) :
            base(message) {
        }
        public InvalidPropertyException(string message, Exception innerException) :
            base(message, innerException) {
        }

        // Protected members

        protected InvalidPropertyException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext) {
        }

    }

}