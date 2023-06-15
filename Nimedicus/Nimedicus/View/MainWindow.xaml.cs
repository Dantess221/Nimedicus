using Nimedicus.Model;
using Nimedicus.Model.DatabaseDataModels;
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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            frame.NavigationService.Navigate(new Login());
        }

        public void OpenLoginPage()
        {
            frame.NavigationService.Navigate(new Login());
        }
        public void OpenDoctorPage(Doctor user)
        {
            frame.NavigationService.Navigate(new DoctorPage(user));
        }
        public void OpenPatientPage(Patient user)
        {
            frame.NavigationService.Navigate(new PatientPage(user));
        }
        public void OpenPatientPageForDoctor(Patient patient, Doctor doctor)
        {
            frame.NavigationService.Navigate(new PatientPage(patient, doctor));
        }
        public void OpenPatientPageForNurse(Patient patient, Nurse nurse)
        {
            frame.NavigationService.Navigate(new PatientPage(patient, nurse));
        }
        public void OpenNursePage(Nurse user)
        {
            frame.NavigationService.Navigate(new NursePage(user));
        }

    }
}
