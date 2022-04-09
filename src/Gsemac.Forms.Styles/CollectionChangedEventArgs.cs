using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Gsemac.Collections.Specialized {

    /// <summary>
    /// Describes the action that caused a <see cref="IObservableCollection{T}.CollectionChanged"/> event.
    /// </summary>
    public enum CollectionChangedAction {
        /// <inheritdoc cref="NotifyCollectionChangedAction.Add"/>
        Add,
        /// <inheritdoc cref="NotifyCollectionChangedAction.Remove"/>
        Remove,
        /// <inheritdoc cref="NotifyCollectionChangedAction.Replace"/>
        Replace,
        /// <inheritdoc cref="NotifyCollectionChangedAction.Move"/>
        Move,
        /// <inheritdoc cref="NotifyCollectionChangedAction.Reset"/>
        Reset,
    }

    public class CollectionChangedEventArgs<T> :
        EventArgs {

        // Public members

        public CollectionChangedAction Action { get; }
        public IEnumerable<T> ChangedItems { get; }
        public int? Index { get; }

        public CollectionChangedEventArgs(CollectionChangedAction action, IEnumerable<T> changedItems) {

            Action = action;
            ChangedItems = changedItems;

        }
        public CollectionChangedEventArgs(CollectionChangedAction action, IEnumerable<T> changedItems, int index) :
            this(action, changedItems) {

            Index = index;

        }

    }

}