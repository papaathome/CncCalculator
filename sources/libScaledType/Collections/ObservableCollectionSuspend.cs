using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace As.Tools.Collections
{
    public class ObservableCollectionSuspend<T> : ObservableCollection<T>
    {
        public bool IsNotificationsSuspended
        {
            get => _is_notifications_suspended;
            set
            {
                if (_is_notifications_suspended == value) return;
                _is_notifications_suspended = value;
                if (!_is_notifications_suspended && _is_notification_pending)
                {
                    OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                    OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                    OnCollectionChanged(
                        new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Reset));
                }
                _is_notification_pending = false;
            }
        }
        bool _is_notifications_suspended = false;

        public void BeginUpdate() => IsNotificationsSuspended = true;

        public void EndUpdate() => IsNotificationsSuspended = false;

        bool _is_notification_pending = false;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (IsNotificationsSuspended)
            {
                _is_notification_pending = true;
                return;
            }
            base.OnCollectionChanged(e);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (IsNotificationsSuspended)
            {
                _is_notification_pending = true;
                return;
            }
            base.OnPropertyChanged(e);
        }
    }
}
