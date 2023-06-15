using Nimedicus.Utils.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace Nimedicus.Utils.ZBindable
{
    public class ZBindable : ZBindableBase, INotifyPropertyChangedExtended, INotifyDataErrorInfo
    {
        #region INotifyPropertyChanged, INotifyPropertyChangedExtended

        public event PropertyChangedExtendedEventHandler PropertyChangedExtended;

        /// <summary>
        /// Checks if a property already matches a desired value. Sets the property and
        ///             notifies listeners only when necessary.
        /// 
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam><param name="storage">Reference to a property with both getter and setter.</param><param name="value">Desired value for the property.</param><param name="propertyName">Name of the property used to notify listeners. This
        ///             value is optional and can be provided automatically when invoked from compilers that
        ///             support CallerMemberName.</param>
        /// <returns>
        /// True if the value was changed, false if the existing value matched the
        ///             desired value.
        /// </returns>
        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;

            var extendedPropertyChangedArgs = new PropertyChangedExtendedEventArgs(propertyName, typeof(T), storage, value);

            TrackChangesForProperty(propertyName, storage, value);

            storage = value;
            ValidateProperty(propertyName, value);
            OnPropertyChanged(propertyName);
            OnPropertyChangedExtended(extendedPropertyChangedArgs);

            // During OnPropertyChanged or OnPropertyChangedExtended value of properties might be reverted so we should do 
            // Equals(storage, value) to be sure that we have realy changed the value
            return Equals(storage, value);
        }

        protected virtual void OnPropertyChangedExtended(string propertyName, Type propertyType, object oldValue, object newValue)
        {
            // ISSUE: reference to a compiler-generated field
            var changedEventHandler = PropertyChangedExtended;
            if (changedEventHandler == null)
                return;
            var e = new PropertyChangedExtendedEventArgs(propertyName, propertyType, oldValue, newValue);
            changedEventHandler((object)this, e);
        }

        protected virtual void OnPropertyChangedExtended(PropertyChangedExtendedEventArgs e)
        {
            OnPropertyChangedExtended(this, e);
        }

        protected virtual void OnPropertyChangedExtended(object s, PropertyChangedExtendedEventArgs e)
        {
            var changedEventHandler = PropertyChangedExtended;
            changedEventHandler?.Invoke(s, e);
        }

        #endregion

        #region INotifyDataErrorInfo

        private ObservableDictionary<string, List<string>> _errors;
        protected ObservableDictionary<string, List<string>> Errors
        {
            get
            {
                if (_errors == null)
                {
                    _errors = new ObservableDictionary<string, List<string>>();
                }
                return _errors;
            }
        }


        public IEnumerable GetErrors(string propertyName)
        {
            return propertyName != null && Errors.ContainsKey(propertyName)
                ? Errors[propertyName]
                : Enumerable.Empty<string>();
        }

        public bool HasErrors => _errors != null && Errors.Any();
        public bool IsValid => !HasErrors;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected virtual void OnErrorsChanged(string propertyName)
        {
            //if (propertyName != null)
            {
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(nameof(IsValid));
        }

        protected void ValidateAllProperties()
        {
            var pis = GetType().GetProperties();

            foreach (var pi in pis)
            {
                if (!pi.GetCustomAttributesData().Any())
                    continue;

                ValidateProperty(pi.Name, pi.GetValue(this));
            }
        }


        private new void ValidateProperty(string propertyName, object newValue)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this) { MemberName = propertyName };

            Validator.TryValidateProperty(newValue, context, results);
            if (results.Any())
            {
                Errors[propertyName] = results.Select(r => r.ErrorMessage).Distinct().ToList();
                OnErrorsChanged(propertyName);
            }
            else
            {
                if (_errors != null && Errors.ContainsKey(propertyName))
                {
                    Errors.Remove(propertyName);
                    OnErrorsChanged(propertyName);
                }
            }
        }

        #endregion

        #region Changes Tracking

        public bool IsChanged => _changedProperties != null && ChangedProperties.Any();

        private ObservableDictionary<string, object> _changedProperties;
        public ObservableDictionary<string, object> ChangedProperties
        {
            get
            {
                if (_changedProperties == null)
                {
                    _changedProperties = new ObservableDictionary<string, object>();
                    _changedProperties.CollectionChanged += ChangedPropertiesOnCollectionChanged;
                }
                return _changedProperties;
            }
        }


        // DataErrorsChangedEventArgs is fine here because it has property name in payload
        public event EventHandler<DataErrorsChangedEventArgs> ChangedPropertiesChanged;

        private void ChangedPropertiesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            OnPropertyChanged(nameof(IsChanged));
        }

        public void ResetChanges()
        {
            if (_changedProperties == null)
            {
                return;
            }
            var pis = GetType().GetProperties();
            foreach (var changedPropertyName in ChangedProperties.Keys)
            {
                var curChangedPI = pis.First(pi => pi.Name.Equals(changedPropertyName));
                curChangedPI.SetValue(this, ChangedProperties[changedPropertyName]);
            }
        }

        public void ResetChanges(string propertyName)
        {
            if (_changedProperties == null)
            {
                return;
            }

            if (ChangedProperties.ContainsKey(propertyName))
            {
                ResetChanges(GetType().GetProperty(propertyName));
            }
        }

        public void ResetChanges(PropertyInfo propertyInfo)
        {
            if (_changedProperties == null)
            {
                return;
            }

            if (ChangedProperties.ContainsKey(propertyInfo.Name))
            {
                propertyInfo.SetValue(this, ChangedProperties[propertyInfo.Name]);
            }
        }

        public void ApplyChanges()
        {
            if (_changedProperties == null)
            {
                return;
            }

            ChangedProperties.Clear();
        }

        private void TrackChangesForProperty(string propertyName, object oldValue, object newValue)
        {
            var pi = GetType().GetProperty(propertyName);
            if (pi.GetCustomAttributesData().Any())
                return;

            if (_changedProperties != null && ChangedProperties.ContainsKey(propertyName))
            {
                if (Equals(ChangedProperties[propertyName], newValue))
                {
                    ChangedProperties.Remove(propertyName);
                    ChangedPropertiesChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
                }
            }
            else
            {
                if (_changedProperties != null && oldValue != null)
                {
                    ChangedProperties.Add(propertyName, oldValue);
                    ChangedPropertiesChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
                }
            }
        }

        #endregion

        public ZBindable()
        {
        }

        public override void Dispose()
        {
            base.Dispose();
            PropertyChangedExtended = null;
            ApplyChanges();
            if (_errors != null)
            {
                _errors.Clear();
            }
        }
    }
}
