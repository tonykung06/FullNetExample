using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FullNetExample.Domain
{
    public class ClientChangeTracker : INotifyPropertyChanged
    {
        private bool _isDirty;

        // Tracking the dirtiness of a model record on UI
        // TODO: this should be attached to ViewModels instead of Entity Model
        public bool IsDirty
        {
            get { return _isDirty; }
            set { SetWithNotify(value, ref _isDirty); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void SetWithNotify<T>
          (T value, ref T field, [CallerMemberName] string propertyName = "")
        {
            if (!Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
