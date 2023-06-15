using GalaSoft.MvvmLight.CommandWpf;
using Nimedicus.Model;
using Nimedicus.Model.DatabaseDataModels;
using Nimedicus.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Nimedicus.ViewModel.PatientTabs
{
    internal class RecieptVM : BaseTab
    {
        public override Task Init()
        {
            return Task.CompletedTask;
        }

        private PatientTabsVM _host;
        public PatientTabsVM Host { get => _host; set => SetProperty(ref _host, value); }

        public RecieptVM(PatientTabsVM host)
        {
            Host = host;
            // Инициализация коллекции данных в конструкторе или методе инициализации
            _currentUserReceiptData = new ObservableCollection<DatabaseDataModels.ReceiptData>();

            RefreshCurrentUserReceiptData();

        }

        public void RefreshCurrentUserReceiptData()
        {
            var query = new DataContext().Receipts
                .Where(x => x.PatientLogin == Host.CurrentUser.Login);

            _currentUserReceiptData.Clear();

            var list = new List<DatabaseDataModels.ReceiptData>();
            foreach (var item in query)
            {
                var newItem = new DatabaseDataModels.ReceiptData(item);
                list.Add(newItem);
            }

            CurrentUserReceiptData = new ObservableCollection<DatabaseDataModels.ReceiptData>(list);

            OnPropertyChanged(nameof(CurrentUserReceiptData));
        }

        private ObservableCollection<DatabaseDataModels.ReceiptData> _currentUserReceiptData;
        public ObservableCollection<DatabaseDataModels.ReceiptData> CurrentUserReceiptData
        {
            get { return _currentUserReceiptData; }
            set
            {
                _currentUserReceiptData = value;
                OnPropertyChanged(nameof(CurrentUserReceiptData));
            }
        }

        private bool _isCreateReceiptView;
        public bool IsCreateReceiptView { get => _isCreateReceiptView; set { SetProperty(ref _isCreateReceiptView, value); } }

        public ICommand CreateReceiptCommand
        {
            get { return _createReceiptCommand ?? (_createReceiptCommand = new RelayCommand(CreateReceipt)); }
        }

        private void CreateReceipt()
        {
            IsCreateReceiptView = true;
        }

        private ICommand _createReceiptCommand;


        public ICommand SumbitReceiptCommand
        {
            get { return _sumbitReceiptCommand ?? (_sumbitReceiptCommand = new RelayCommand(SumbitReceipt, SumbitReceiptCanExecute)); }
        }

        private bool SumbitReceiptCanExecute()
        {
            return DrugList.Count > 0 && SelectedDate != null;
        }

        private void SumbitReceipt()
        {
           new DatabaseManager().CreateReceipt(DrugList.ToList(),SelectedDate.Value,Host.CurrentUser,Host.Doctor);

            if (MessageBox.Show("Рецепт успішно додан!") == MessageBoxResult.OK)
            {
                Return();

                RefreshCurrentUserReceiptData();
            }
        }

        private ICommand _sumbitReceiptCommand;

        public ICommand ReturnCommand
        {
            get { return _returnCommand ?? (_returnCommand = new RelayCommand(Return)); }
        }

        private void Return()
        {
            IsCreateReceiptView = false;
            SelectedDate = null;
            NewItem = string.Empty;
            DrugList.Clear();
        }

        private ICommand _returnCommand;



        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (SetProperty<DateTime?>(ref _selectedDate, value))
                {
                }
            }
        }

        public ObservableCollection<string> DrugList { get; } = new ObservableCollection<string>();

        private ICommand _addCommand;
        public ICommand AddCommand { get => _addCommand ?? (_addCommand = new RelayCommand(AddItem)); }

        private RelayCommand<string> _deleteCommand;
        public RelayCommand<string> DeleteCommand { get => _deleteCommand ?? (_deleteCommand = new RelayCommand<string>(DeleteItem)); }

        private void AddItem()
        {
            if (!string.IsNullOrEmpty(NewItem))
            {
                DrugList.Add(NewItem);
                NewItem = string.Empty;
                OnPropertyChanged(nameof(DrugList));
            }
        }
        private string _newItem;

        public string NewItem
        {
            get { return _newItem; }
            set
            {
                _newItem = value;
                OnPropertyChanged(nameof(NewItem));
            }
        }
        private void DeleteItem(string item)
        {
            DrugList.Remove(item);
            OnPropertyChanged(nameof(DrugList));
        }
    }
}
