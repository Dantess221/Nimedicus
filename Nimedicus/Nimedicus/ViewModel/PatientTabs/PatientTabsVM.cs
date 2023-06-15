using Nimedicus.Model;
using Nimedicus.Model.DatabaseDataModels;
using Nimedicus.Utils.ZBindable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimedicus.ViewModel.PatientTabs
{
    public class PatientTabsVM : ZBindable
    {
        private List<BaseTab> _pagesVMs;
        private GeneralVM _pageGeneral;
        private HistoryVM _pageHistory;
        private RecieptVM _pageReciept;

        private Patient _currentUser;
        public Patient CurrentUser { get => _currentUser; set { if (SetProperty(ref _currentUser, value)) OnPropertyChanged(nameof(CurrentUserFullName)); } }

        private Doctor _doctor;
        public Doctor Doctor { get => _doctor; set { if (SetProperty(ref _doctor, value)) OnPropertyChanged(nameof(CurrentDoctorFullName)); } }

        private Nurse _nurse;
        public Nurse Nurse { get => _nurse; set { if (SetProperty(ref _nurse, value)) OnPropertyChanged(nameof(CurrentDoctorFullName)); } }
        public string CurrentUserFullName => CurrentUser.LastName + " " + CurrentUser.Name + " " + CurrentUser.MiddleName;
        public string CurrentDoctorFullName => Doctor.LastName + " " + Doctor.Name + " " + Doctor.MiddleName;

        public string CurrentUserDoctorFullName
        {
            get
            {
                var doctor = new DataContext().Doctors.First(x => x.Login == CurrentUser.DoctorLogin);
                return doctor == null ? null : doctor.LastName + " " + doctor.Name + " " + doctor.MiddleName;
            }
        }
        private bool _isDoctorView;
        public bool IsDoctorView { get => _isDoctorView; set { if(SetProperty(ref _isDoctorView, value)) OnPropertyChanged(nameof(IsDoctorOrNurseView)); } }

        private bool _isNurseView;
        public bool IsNurseView { get => _isNurseView; set { if(SetProperty(ref _isNurseView, value)) OnPropertyChanged(nameof(IsDoctorOrNurseView)); } }

        public bool IsDoctorOrNurseView { get => IsDoctorView || IsNurseView; }
        private bool IsInitializing { get; set; }

        public PatientTabsVM(Patient user)
        {
            CurrentUser = user;

            _pagesVMs = new List<BaseTab>();
            _pageGeneral = new GeneralVM(this);
            _pageHistory = new HistoryVM(this);
            _pageReciept = new RecieptVM(this);

            _pagesVMs.Add(_pageGeneral);
            _pagesVMs.Add(_pageHistory);
            _pagesVMs.Add(_pageReciept);

            IsDoctorView = false;
            //new DatabaseManager().CreateAnalys("Аналіз крові загальний", @"C:\Users\3prok\Documents\Job\Nimedicus\test\Receipt№00002.pdf", CurrentUser);
            //new DatabaseManager().CreateReceipt(new List<string> { "Супразид", "Нимесил", "Гидазепам" }, DateTime.Today.AddMonths(1), CurrentUser, new DataContext().Doctors.First(x => x.Login == CurrentUser.DoctorLogin));
        }

        public PatientTabsVM(Patient patient, Doctor doctor)
        {
            CurrentUser = patient;

            _pagesVMs = new List<BaseTab>();
            _pageGeneral = new GeneralVM(this);
            _pageHistory = new HistoryVM(this);
            _pageReciept = new RecieptVM(this);

            _pagesVMs.Add(_pageGeneral);
            _pagesVMs.Add(_pageHistory);
            _pagesVMs.Add(_pageReciept);

            IsDoctorView = true;
            Doctor = doctor;
            //new DatabaseManager().CreateAnalys("Аналіз крові загальний", @"C:\Users\3prok\Documents\Job\Nimedicus\test\Receipt№00002.pdf", CurrentUser);
            //new DatabaseManager().CreateReceipt(new List<string> { "Супразид", "Нимесил", "Гидазепам" }, DateTime.Today.AddMonths(1), CurrentUser, new DataContext().Doctors.First(x => x.Login == CurrentUser.DoctorLogin));
        }
        public PatientTabsVM(Patient patient, Nurse nurse)
        {
            CurrentUser = patient;

            _pagesVMs = new List<BaseTab>();
            _pageGeneral = new GeneralVM(this);
            _pageHistory = new HistoryVM(this);
            _pageReciept = new RecieptVM(this);

            _pagesVMs.Add(_pageGeneral);
            _pagesVMs.Add(_pageHistory);
            _pagesVMs.Add(_pageReciept);

            IsNurseView = true;
            Nurse = nurse;
            //new DatabaseManager().CreateAnalys("Аналіз крові загальний", @"C:\Users\3prok\Documents\Job\Nimedicus\test\Receipt№00002.pdf", CurrentUser);
            //new DatabaseManager().CreateReceipt(new List<string> { "Супразид", "Нимесил", "Гидазепам" }, DateTime.Today.AddMonths(1), CurrentUser, new DataContext().Doctors.First(x => x.Login == CurrentUser.DoctorLogin));
        }


        public async void Initialize()
        {
            IsInitializing = true;
            foreach (var page in _pagesVMs)
            {
                await page.Init();
            }
            IsInitializing = false;
        }

        public void Refresh()
        {
            foreach (var page in _pagesVMs)
            {
                page.Refresh();
            }
        }
        public void SelectPage(Type selectPage)
        {
            int i = 0;
            foreach (var page in _pagesVMs)
            {
                if (page.GetType() == selectPage)
                {
                    SelectedPage = i;
                    break;
                }
                i++;
            }
        }

        public BaseTab CurrentPage => SelectedPage!=null?_pagesVMs[SelectedPage.Value]:null;

        private int? _selectedPage = null;
        public int? SelectedPage
        {
            get { return _selectedPage; }
            set
            {
                if (SetProperty<int?>(ref _selectedPage, value))
                {
                    OnPropertyChanged(nameof(CurrentPage));
                }
            }
        }
    }
}
