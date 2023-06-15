using Nimedicus.Model.DatabaseDataModels;
using Nimedicus.ViewModel.PatientTabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для DoctorPage.xaml
    /// </summary>
    public partial class NursePage : Page
    {
        public NursePage(Nurse user)
        {
            InitializeComponent();
            if (DataContext == null)
            {
                DataContext = new NurseVM(user);
            }

            _viewModel = (NurseVM)DataContext;
        }

        private NurseVM _viewModel;



        private void SVGButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите выйти?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)

                (App.Current.MainWindow as MainWindow).OpenLoginPage();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9+]+"); // Регулярное выражение для разрешения только цифр и символа "+"
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
