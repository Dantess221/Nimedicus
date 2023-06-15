using GalaSoft.MvvmLight.CommandWpf;
using Nimedicus.Model;
using Nimedicus.Model.DatabaseDataModels;
using Nimedicus.Utils;
using Nimedicus.Utils.Enums;
using Nimedicus.Utils.ZBindable;
using Nimedicus.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Nimedicus.ViewModel.PatientTabs
{
    public class DoctorVM : ZBindable
    {

        private Doctor _currentUser;
        public Doctor CurrentUser { get => _currentUser; set { SetProperty(ref _currentUser, value); } }


        private List<Patient> _patients;
        public List<Patient> Patients { get => _patients; set { SetProperty(ref _patients, value); } }
        private bool IsInitializing { get; set; }

        public DoctorVM(Doctor user)
        {
            CurrentUser = user;


            //new DatabaseManager().CreatePatient("se",Utils.Enums.Sex.Male, DateTime.Now, "Asa", "Sasa", "Dasa", "+3945465", "dasdasdasda", "dsadadad", "ss", "se");

            UpdatePatients();
        }

        private void UpdatePatients()
        {
            Patients = new DataContext().Patients.Where(x => x.DoctorLogin == CurrentUser.Login).ToList();
        }

        public async void Initialize()
        {
            IsInitializing = true;
            foreach (var page in Patients)
            {
                //await page.Init();
            }
            IsInitializing = false;
        }

        public void Refresh()
        {
            foreach (var page in Patients)
            {
                //page.Refresh();
            }
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

        public Patient CurrentPatient => (SelectedPage == -1 || Patients.Count < SelectedPage + 1)? null : Patients[SelectedPage];

        private int _selectedPage;
        public int SelectedPage
        {
            get { return _selectedPage; }
            set
            {
                if (SetProperty<int>(ref _selectedPage, value))
                {
                    if(IsAddPatientView == true && SelectedPage !=-1)
                    {
                        IsAddPatientView = false;
                    }

                    OnPropertyChanged(nameof(CurrentPatient));
                }
            }
        }
        public ICommand OpenPatientViewCommand
        {
            get { return _openPatientViewCommand ?? (_openPatientViewCommand = new RelayCommand(OpenPatientView)); }
        }

        private async void OpenPatientView()
        {
            (App.Current.MainWindow as MainWindow).OpenPatientPageForDoctor(CurrentPatient, CurrentUser);
        }

        private ICommand _addPatientViewCommand;

        public ICommand AddPatientViewCommand
        {
            get { return _addPatientViewCommand ?? (_addPatientViewCommand = new RelayCommand(AddPatientView)); }
        }

        private async void AddPatientView()
        {
            SelectedPage = -1;
            IsAddPatientView = !IsAddPatientView;
        }

        private ICommand _openPatientViewCommand;

        private bool _isAddPatientView;
        public bool IsAddPatientView
        {
            get { return _isAddPatientView; }
            set
            {
                if (SetProperty<bool>(ref _isAddPatientView, value))
                {
                }
            }
        }


        private string _patientName;
        public string PatientName
        {
            get { return _patientName; }
            set
            {
                if (SetProperty<string>(ref _patientName, value))
                {
                }
            }
        }
        private string _patientMiddleName;
        public string PatientMiddleName
        {
            get { return _patientMiddleName; }
            set
            {
                if (SetProperty<string>(ref _patientMiddleName, value))
                {
                }
            }
        }
        private string _patientLastName;
        public string PatientLastName
        {
            get { return _patientLastName; }
            set
            {
                if (SetProperty<string>(ref _patientLastName, value))
                {
                }
            }
        }
        private int _selectedSex = -1;
        public int SelectedSex
        {
            get { return _selectedSex; }
            set
            {
                if (SetProperty<int>(ref _selectedSex, value))
                {
                }
            }
        }

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
        private string _patientAddress;
        public string PatientAddress
        {
            get { return _patientAddress; }
            set
            {
                if (SetProperty<string>(ref _patientAddress, value))
                {
                }
            }
        }
        private string _patientMail;
        public string PatientMail
        {
            get { return _patientMail; }
            set
            {
                if (SetProperty<string>(ref _patientMail, value))
                {
                }
            }
        }
        private string _patientPhone;
        public string PatientPhone
        {
            get { return _patientPhone; }
            set
            {
                if (SetProperty<string>(ref _patientPhone, value))
                {
                }
            }
        }

        private string _patientLogin;
        public string PatientLogin
        {
            get { return _patientLogin; }
            set
            {
                if (SetProperty<string>(ref _patientLogin, value))
                {
                }
            }
        }
        private string _patientPassword;
        public string PatientPassword
        {
            get { return _patientPassword; }
            set
            {
                if (SetProperty<string>(ref _patientPassword, value))
                {
                }
            }
        }

        public ICommand SumbitPatientCommand
        {
            get { return _sumbitPatientCommand ?? (_sumbitPatientCommand = new RelayCommand(SumbitPatient, SumbitPatientCanExecute)); }
        }

        private bool SumbitPatientCanExecute()
        {
            if (String.IsNullOrEmpty(PatientName))
                return false;
            if (String.IsNullOrEmpty(PatientMiddleName))
                return false;
            if (String.IsNullOrEmpty(PatientLastName))
                return false;
            if (SelectedSex == -1)
                return false; 
            if (SelectedDate == null)
                return false; 
            if (String.IsNullOrEmpty(PatientAddress))
                return false; 
            if (String.IsNullOrEmpty(PatientMail))
                return false; 
            if (String.IsNullOrEmpty(PatientPhone))
                return false; 
            if (String.IsNullOrEmpty(PatientLogin))
                return false; 
            if (String.IsNullOrEmpty(PatientPassword))
                return false;

            return true;
        }

        private void SumbitPatient()
        {
            if (new DatabaseManager().CreatePatient(PatientLogin, (Sex)SelectedSex, SelectedDate.Value, PatientName, PatientLastName, PatientMiddleName, PatientPhone, PatientAddress, PatientMail, CurrentUser.Login, PatientPassword, true))
            {
                if (MessageBox.Show("Пацієнт успішно додан!") == MessageBoxResult.OK)
                {
                    UpdatePatients();
                    Return();
                }
            }
        }

        private ICommand _sumbitPatientCommand;

        public ICommand ReturnCommand
        {
            get { return _returnCommand ?? (_returnCommand = new RelayCommand(Return)); }
        }

        private void Return()
        {
            SelectedPage = 0;

            PatientName = string.Empty;
            PatientMiddleName = string.Empty;
            PatientLastName = string.Empty;
            SelectedSex = -1;
            SelectedDate = null;
            PatientAddress = string.Empty;
            PatientMail = string.Empty;
            PatientPhone = string.Empty;
            PatientLogin = string.Empty;
            PatientPassword = string.Empty;
        }

        private ICommand _returnCommand;

        public ICommand DeletePatientCommand
        {
            get { return _deletePatientCommand ?? (_deletePatientCommand = new RelayCommand(DeletePatient)); }
        }

        private void DeletePatient()
        {
            if (MessageBox.Show("Вы точно хотите удалить пользователя?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                new DatabaseManager().DeletePatient(CurrentPatient);

                UpdatePatients();

                SelectedPage = 0;
            }
        }

        private ICommand _deletePatientCommand;
    }
}
