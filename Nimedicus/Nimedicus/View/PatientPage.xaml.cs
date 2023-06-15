using Nimedicus.Model.DatabaseDataModels;
using Nimedicus.ViewModel.PatientTabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nimedicus.View
{
    /// <summary>
    /// Логика взаимодействия для PatientPage.xaml
    /// </summary>
    public partial class PatientPage : Page
    {
        public PatientPage(Patient user)
        {
            InitializeComponent();

            if(DataContext == null)
            {
                DataContext = new PatientTabsVM(user);
            }

            _viewModel = (PatientTabsVM)DataContext;
        }

        private Doctor Doctor { get; set; }
        private Nurse Nurse { get; set; }

        public PatientPage(Patient patient, Doctor doctor)
        {
            InitializeComponent();

            if (DataContext == null)
            {
                DataContext = new PatientTabsVM(patient, doctor);
            }
            Doctor = doctor;
            _viewModel = (PatientTabsVM)DataContext;
        }

        public PatientPage(Patient patient, Nurse nurse)
        {
            InitializeComponent();

            if (DataContext == null)
            {
                DataContext = new PatientTabsVM(patient, nurse);
            }
            Nurse = nurse;
            _viewModel = (PatientTabsVM)DataContext;
        }

        public void Initialize()
        {
            _viewModel.Initialize();
        }
        public void Refresh()
        {
            _viewModel.Refresh();
        }

        private PatientTabsVM _viewModel;



        private void ExitCommand(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите выйти?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)

                (App.Current.MainWindow as MainWindow).OpenLoginPage();
        }

        private void ReturnCommand(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите вернутся?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                if (_viewModel.IsDoctorView)
                    (App.Current.MainWindow as MainWindow).OpenDoctorPage(Doctor);
                else
                    (App.Current.MainWindow as MainWindow).OpenNursePage(Nurse);
        }
    }
}
