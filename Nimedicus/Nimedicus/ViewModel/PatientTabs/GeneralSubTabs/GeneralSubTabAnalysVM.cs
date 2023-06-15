using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Nimedicus.Model;
using Nimedicus.Model.DatabaseDataModels;
using Nimedicus.Utils;
using Nimedicus.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Nimedicus.ViewModel.PatientTabs.GeneralSubTabs
{
    internal class GeneralSubTabAnalysVM : BaseTab
    {
        public override Task Init()
        {
            return Task.CompletedTask;
        }

        private GeneralVM _host;
        public GeneralVM Host { get => _host; set => SetProperty(ref _host, value); }


        public GeneralSubTabAnalysVM(GeneralVM host)
        {
            Host = host;
            // Инициализация коллекции данных в конструкторе или методе инициализации
            CurrentUserAnalysData = new ObservableCollection<DatabaseDataModels.AnalysData>();


            //var analysData = new AnalysData();
            //new DatabaseManager().CreateAnalys("Avbc", Host.Host.CurrentUser, out analysData);

            //_ = new AzureBlobStorageHelper(containerName: "nimedicus-analysis").UploadFileToAzure("avb1", @"C:\Users\3prok\Documents\Job\Nimedicus\test\NET C# Developer.pdf");

            //analysData.AnalysFileBlobName = "avb1";
            //var context = new DataContext();

            //var query = context.Analysis
            //   .First(x => x.AnalysDataId == analysData.AnalysDataId).AnalysFileBlobName = "avb1";

            //context.SaveChanges();

            RefreshCurrentUserAnalysData();
        }

        public void RefreshCurrentUserAnalysData()
        {
            var query = new DataContext().Analysis
                .Where(x => x.PatientLogin == Host.Host.CurrentUser.Login);

            _currentUserAnalysData.Clear();

            var list = new List<DatabaseDataModels.AnalysData>();
            foreach (var item in query)
            {
                var newItem = new DatabaseDataModels.AnalysData(item);
                list.Add(newItem);
            }

            CurrentUserAnalysData = new ObservableCollection<DatabaseDataModels.AnalysData>(list);

            OnPropertyChanged(nameof(CurrentUserAnalysData));
        }

        private ObservableCollection<DatabaseDataModels.AnalysData> _currentUserAnalysData;
        public ObservableCollection<DatabaseDataModels.AnalysData> CurrentUserAnalysData
        {
            get { return _currentUserAnalysData; }
            set
            {
                _currentUserAnalysData = value;
                OnPropertyChanged(nameof(CurrentUserAnalysData));
            }
        }


        private bool _isCreateAnalysView;
        public bool IsCreateAnalysView { get => _isCreateAnalysView; set { SetProperty(ref _isCreateAnalysView, value); } }

        public ICommand CreateAnalysCommand
        {
            get { return _createAnalysCommand ?? (_createAnalysCommand = new RelayCommand(CreateAnalys)); }
        }

        private void CreateAnalys()
        {
            IsCreateAnalysView = true;
        }

        private ICommand _createAnalysCommand;

        public ICommand SumbitAnalysCommand
        {
            get { return _sumbitAnalysCommand ?? (_sumbitAnalysCommand = new RelayCommand(SumbitAnalys)); }
        }

        private void SumbitAnalys()
        {
            new DatabaseManager().CreateAnalys(AnalysName, FilePath, Host.Host.CurrentUser);

            if (MessageBox.Show("Аналіз успішно додан!") == MessageBoxResult.OK)
            {
                Return();

                RefreshCurrentUserAnalysData();
            }    
        }

        private ICommand _sumbitAnalysCommand;

        public ICommand ReturnCommand
        {
            get { return _returnCommand ?? (_returnCommand = new RelayCommand(Return)); }
        }

        private void Return()
        {
            IsCreateAnalysView = false;
            FilePath = string.Empty;
            AnalysName = string.Empty;
        }

        private ICommand _returnCommand;

        private string _analysName;
        public string AnalysName
        {
            get { return _analysName; }
            set
            {
                if (SetProperty<string>(ref _analysName, value))
                {
                }
            }
        }


        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                if (SetProperty<string>(ref _filePath, value))
                {
                }
            }
        }

        public ICommand SetFilePathCommand
        {
            get { return _setFilePathCommand ?? (_setFilePathCommand = new RelayCommand(SetFilePath)); }
        }

        private async void SetFilePath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
            }
        }

        private ICommand _setFilePathCommand;
    }
}
