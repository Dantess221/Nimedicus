using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Nimedicus.Utils.ZBindable
{
    [Serializable]
    public class ZBindableBase : INotifyPropertyChanged, IDisposable
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            ValidateProperty(propertyName, value);
            OnPropertyChanged(propertyName);
            return true;
        }

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                propertyChanged(this, e);
            }
        }

        protected virtual void ValidateProperty(string propertyName, object newValue)
        {
        }

        public virtual void Dispose()
        {
            this.PropertyChanged = null;
        }
    }
}
