using Nimedicus.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimedicus.ViewModel.PatientTabs
{
    internal class HistoryVM : BaseTab
    {
        public override Task Init()
        {
            return Task.CompletedTask;
        }

        private PatientTabsVM _host;
        public PatientTabsVM Host { get => _host; set => SetProperty(ref _host, value); }

        public HistoryVM(PatientTabsVM host)
        {
            Host = host;
            // Инициализация коллекции данных в конструкторе или методе инициализации
            _currentUserEvents = new ObservableCollection<DatabaseDataModels.Event>();

            RefreshCurrentUserHistory();

        }

        public void RefreshCurrentUserHistory()
        {
            var query = new DataContext().Events
                .Where(x => x.PatientLogin == Host.CurrentUser.Login);

            _currentUserEvents.Clear();

            var list = new List<DatabaseDataModels.Event>();
            foreach (var item in query)
            {
                var newItem = new DatabaseDataModels.Event(item);
                list.Add(newItem);
            }

            CurrentUserEvents = new ObservableCollection<DatabaseDataModels.Event>(list);

            OnPropertyChanged(nameof(CurrentUserEvents));
        }

        private ObservableCollection<DatabaseDataModels.Event> _currentUserEvents;
        public ObservableCollection<DatabaseDataModels.Event> CurrentUserEvents
        {
            get { return _currentUserEvents; }
            set
            {
                _currentUserEvents = value;
                OnPropertyChanged(nameof(_currentUserEvents));
            }
        }
    }
}
