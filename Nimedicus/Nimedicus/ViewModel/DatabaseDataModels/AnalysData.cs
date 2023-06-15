using Chilkat;
using GalaSoft.MvvmLight.Command;
using Nimedicus.Model.DatabaseDataModels;
using Nimedicus.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nimedicus.ViewModel.DatabaseDataModels
{
    public class AnalysData
    {
        public int AnalysDataId { get; set; }

        public string AnalysName { get; set; }
        public DateTime DataCreation { get; set; }
        public string PatientLogin { get; set; }

        public string AnalysFileBlobName { get; set; }

        public AnalysData(int analysDataId, string analysName, DateTime dataCreation, string patientLogin, string analysFileBlobName)
        {
            AnalysDataId = analysDataId;
            AnalysName = analysName;
            DataCreation = dataCreation;
            PatientLogin = patientLogin;
            AnalysFileBlobName = analysFileBlobName;
        }

        public AnalysData(Model.DatabaseDataModels.AnalysData analysData)
        {
            AnalysDataId = analysData.AnalysDataId;
            AnalysName = analysData.AnalysName;
            DataCreation = analysData.DataCreation;
            PatientLogin = analysData.PatientLogin;
            AnalysFileBlobName = analysData.AnalysFileBlobName;
        }
        public ICommand DownloadFileCommand
        {
            get { return _downloadFileCommand ?? (_downloadFileCommand = new RelayCommand(DownloadFile)); }
        }

        private async void DownloadFile()
        {
            if (string.IsNullOrEmpty(AnalysFileBlobName)) return;

           await new AzureBlobStorageHelper(containerName: "nimedicus-analysis").DownloadFileFromAzure(AnalysFileBlobName);
        }

        private ICommand _downloadFileCommand;
    }
}
