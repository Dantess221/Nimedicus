using Nimedicus.Controls;
using Nimedicus.Model.DatabaseDataModels;
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

namespace Nimedicus.View.PatientTabs
{
    /// <summary>
    /// Логика взаимодействия для General.xaml
    /// </summary>
    public partial class General : UserControl
    {
        public General()
        {
            InitializeComponent();

            DataContextChanged += General_DataContextChanged;
        }

        private void General_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _viewModel = (GeneralVM)DataContext;
        }

        public void Initialize()
        {
            _viewModel.Initialize();
        }
        public void Refresh()
        {
            _viewModel.Refresh();
        }



        private GeneralVM _viewModel;

    }
}
