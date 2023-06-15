using Nimedicus.Model;
using Nimedicus.Model.DatabaseDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimedicus.ViewModel.PatientTabs.GeneralSubTabs
{
    internal class GeneralSubTabGeneralVM : BaseTab
    {
        public override Task Init()
        {
            return Task.CompletedTask;
        }

        private GeneralVM _host;
        public GeneralVM Host { get => _host; set => SetProperty(ref _host, value); }


        private Patient _patient;
        public Patient Patient { get => _patient; set => SetProperty(ref _patient, value); }

        public string CurrentUserDoctorFullName
        {
            get
            {
                
                var doctor = Patient == null ? null : new DataContext().Doctors.FirstOrDefault(x => x.Login == Patient.DoctorLogin);
                return doctor == null ? null : doctor.LastName + " " + doctor.Name + " " + doctor.MiddleName;
            }
        }

        public GeneralSubTabGeneralVM(GeneralVM host)
        {
            Host = host;
            Patient = Host.Host.CurrentUser;
        }

        public GeneralSubTabGeneralVM(Patient patient)
        {
            Patient = patient;
        }
    }
}
