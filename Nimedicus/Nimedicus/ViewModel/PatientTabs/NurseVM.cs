using Nimedicus.Model.DatabaseDataModels;
using Nimedicus.Model;
using Nimedicus.Utils.Enums;
using Nimedicus.Utils.ZBindable;
using Nimedicus.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;

namespace Nimedicus.ViewModel.PatientTabs
{
    public class NurseVM : ZBindable
    {

        private Nurse _currentUser;
        public Nurse CurrentUser { get => _currentUser; set { SetProperty(ref _currentUser, value); } }


        private List<Patient> _patients;
        public List<Patient> Patients { get => _patients; set { SetProperty(ref _patients, value); } }
        private bool IsInitializing { get; set; }

        public NurseVM(Nurse user)
        {
            CurrentUser = user;


            //new DatabaseManager().CreatePatient("se",Utils.Enums.Sex.Male, DateTime.Now, "Asa", "Sasa", "Dasa", "+3945465", "dasdasdasda", "dsadadad", "ss", "se");

            UpdatePatients();
        }

        private void UpdatePatients()
        {
            Patients = new DataContext().Patients.ToList();
        }

        public void SelectPage(Type selectPage)
        {
            int i = 0;
            foreach (var page in Patients)
            {
                if (page.GetType() == selectPage)
                {
                    SelectedPage = i;
                    break;
                }
                i++;
            }
        }

        public Patient CurrentPatient => (SelectedPage == -1 || Patients.Count < SelectedPage + 1) ? null : Patients[SelectedPage];

        private int _selectedPage;
        public int SelectedPage
        {
            get { return _selectedPage; }
            set
            {
                if (SetProperty<int>(ref _selectedPage, value))
                {
                    OnPropertyChanged(nameof(CurrentPatient));
                }
            }
        }
        public ICommand _openPatientViewCommand;
        public ICommand OpenPatientViewCommand
        {
            get { return _openPatientViewCommand ?? (_openPatientViewCommand = new RelayCommand(OpenPatientView)); }
        }

        private async void OpenPatientView()
        {
            (App.Current.MainWindow as MainWindow).OpenPatientPageForNurse(CurrentPatient, CurrentUser);
        }

        
    }
}
