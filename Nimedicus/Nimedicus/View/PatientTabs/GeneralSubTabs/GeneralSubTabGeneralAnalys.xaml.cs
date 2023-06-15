using Nimedicus.Model;
using Nimedicus.ViewModel.PatientTabs;
using Nimedicus.ViewModel.PatientTabs.GeneralSubTabs;
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

namespace Nimedicus.View.PatientTabs.GeneralSubTabs
{
    /// <summary>
    /// Логика взаимодействия для General.xaml
    /// </summary>
    public partial class GeneralSubTabGeneralAnalys : UserControl
    {
        public GeneralSubTabGeneralAnalys()
        {
            InitializeComponent();
            DataContextChanged += GeneralSubTabGeneralAnalys_DataContextChanged; ;
        }

        private void GeneralSubTabGeneralAnalys_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            (DataContext as GeneralSubTabAnalysVM)?.RefreshCurrentUserAnalysData();
        }

    }
}
