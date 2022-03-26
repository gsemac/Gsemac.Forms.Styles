using System;

namespace Gsemac.Forms.Styles.Dom {

    public class ClassesChangedEventArgs :
        EventArgs {

        // Public memberss

        public string Class { get; }

        public ClassesChangedEventArgs(string className) {

            Class = className;

        }

    }

}