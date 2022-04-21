using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

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