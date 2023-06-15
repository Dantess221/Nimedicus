using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimedicus.Utils
{
    public class PropertyChangedExtendedEventArgs<T> : PropertyChangedEventArgs
    {
        public T OldValue { get; private set; }
        public T NewValue { get; private set; }

        public PropertyChangedExtendedEventArgs(string propertyName, T oldValue, T newValue)
            : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public void RevertChanges(object sender)
        {
            var senderType = sender.GetType();
            var pi = senderType.GetProperty(PropertyName);
            pi.SetValue(sender, OldValue);
        }
    }

    public delegate void PropertyChangedExtendedEventHandler<T>(object sender, PropertyChangedExtendedEventArgs<T> e);

    public class PropertyChangedExtendedEventArgs : PropertyChangedEventArgs
    {
        public object OldValue { get; private set; }
        public object NewValue { get; private set; }
        public Type PropertyType { get; private set; }

        public PropertyChangedExtendedEventArgs(string propertyName, Type propertyType, object oldValue, object newValue)
            : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
            PropertyType = propertyType;
        }

        public void RevertChanges(object sender)
        {
            var senderType = sender.GetType();
            var pi = senderType.GetProperty(PropertyName);
            pi.SetValue(sender, OldValue);
        }
    }

    public delegate void PropertyChangedExtendedEventHandler(object sender, PropertyChangedExtendedEventArgs e);
}
