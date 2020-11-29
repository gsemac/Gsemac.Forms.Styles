using System;

namespace Gsemac.Forms.Styles.StyleSheets {

    [Serializable]
    public class InvalidPropertyException :
        Exception {

        // Public members

        public InvalidPropertyException(string propertyName) :
            base(CreateMessage(propertyName)) {
        }
        public InvalidPropertyException(string propertyName, Exception innerException) :
            base(CreateMessage(propertyName), innerException) {
        }
        public InvalidPropertyException() {
        }

        // Protected members

        protected InvalidPropertyException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) :
            base(serializationInfo, streamingContext) {
        }

        // Private members

        private static string CreateMessage(string propertyName) {

            return $"\"{propertyName}\" is not a valid property.";

        }

    }

}