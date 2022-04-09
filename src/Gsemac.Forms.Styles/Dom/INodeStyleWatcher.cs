using System;

namespace Gsemac.Forms.Styles.Dom {

    public interface INodeStyleWatcher :
        IDisposable {

        event EventHandler<StyleInvalidatedEventArgs> StyleInvalidated;
        event EventHandler<StylesChangedEventArgs> StylesChanged;

        void SuspendStyleUpdates();
        void ResumeStyleUpdates();
        void InvalidateStyles();

    }

}