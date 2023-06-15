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
using Windows.System;

namespace Nimedicus.View.PatientTabs
{
    /// <summary>
    /// Логика взаимодействия для General.xaml
    /// </summary>
    public partial class History : UserControl
    {
        public History()
        {
            InitializeComponent();
            DataContextChanged += History_DataContextChanged;
        }

        private void History_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _viewModel = (HistoryVM)DataContext;
            _viewModel.RefreshCurrentUserHistory();
        }

        private HistoryVM _viewModel;
    }

}
