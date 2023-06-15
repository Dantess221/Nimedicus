using Chilkat;
using Nimedicus.Model.DatabaseDataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimedicus.ViewModel.DatabaseDataModels
{
    public class Event
    {
        public DateTime DataCreation { get; set; }
        public string Message { get; set; }
        public string PatientLogin { get; set; }
        public Event(string message, DateTime dataCreation, string patientLogin)
        {
            Message = message;
            DataCreation = dataCreation;
            PatientLogin = patientLogin;
        }

        public Event(Model.DatabaseDataModels.Event analysData)
        {
            Message = analysData.Message;
            DataCreation = analysData.DataCreation;
            PatientLogin = analysData.PatientLogin;
        }
    }
}
