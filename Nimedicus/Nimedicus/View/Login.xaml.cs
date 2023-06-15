using Nimedicus.Model;
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
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = loginTextBox.Text;
            string password = passwordBox.Password;

            // Проверка введенных данных и выполнение входа
            if (CheckCredentials(login, password))
            {
                // Вход выполнен успешно
               // if (MessageBox.Show("Вход выполнен!") == MessageBoxResult.OK)
                {
                    using (var context = new DataContext())
                    {
                        var patient = context.Patients.FirstOrDefault(a => a.Login == login);
                        if (patient != null)
                        {
                            (App.Current.MainWindow as MainWindow).OpenPatientPage(patient);
                            return;
                        }

                        var doctor = context.Doctors.FirstOrDefault(a => a.Login == login);
                        if (doctor != null)
                        {
                            (App.Current.MainWindow as MainWindow).OpenDoctorPage(doctor);
                            return;
                        }

                        var nurse = context.Nurses.FirstOrDefault(a => a.Login == login);
                        if (nurse != null)
                        {
                            (App.Current.MainWindow as MainWindow).OpenNursePage(nurse);
                            return;
                        }
                    }
                }
            }
            else
            {
                // Неверные учетные данные
                MessageBox.Show("Неверный логин или пароль!");
            }
        }

        private bool CheckCredentials(string login, string password)
        {
            using (var context = new DataContext())
            {
                // Проверка наличия соответствующей записи в таблице Auth
                var auth = context.Auths.FirstOrDefault(a => a.Login == login && a.Password == password);
                if (auth != null)
                {
                    // Учетные данные верные
                    return true;
                }
            }

            // Учетные данные неверные или отсутствуют в таблице Auth
            return false;
        }
    }
}
