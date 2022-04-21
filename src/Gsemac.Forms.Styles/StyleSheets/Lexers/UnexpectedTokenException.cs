using System;

namespace Gsemac.Forms.Styles.StyleSheets.Lexers {

    [Serializable]
    public class UnexpectedTokenException :
        Exception {

        // Public members

        public UnexpectedTokenException(string message) :
            base(message) {
        }
        public UnexpectedTokenException(string message, Exception innerException) : base(message, innerException) {
        }
        public UnexpectedTokenException() {
        }

        // Protected members

        protected UnexpectedTokenException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) :
            base(serializationInfo, streamingContext) {
        }

    }

}