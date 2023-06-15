using Newtonsoft.Json.Linq;
using Nimedicus.Controls;
using Nimedicus.Model.DatabaseDataModels;
using Nimedicus.ViewModel.PatientTabs;
using Nimedicus.ViewModel.PatientTabs.GeneralSubTabs;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class GeneralSubTabGeneral : UserControl, INotifyPropertyChanged
    {

        public GeneralSubTabGeneral()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty PatientProperty = DependencyProperty.Register(
            "Patient", typeof(Patient), typeof(GeneralSubTabGeneral), new FrameworkPropertyMetadata(null, PatientChanged));

        private static void PatientChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GeneralSubTabGeneral generalSubTabGeneral)
                generalSubTabGeneral.DataContext = new GeneralSubTabGeneralVM(generalSubTabGeneral.Patient);
;
        }
        
        public Patient Patient
        {
            get { return (Patient)GetValue(PatientProperty); }
            set { SetValue(PatientProperty, value); }
        }
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                propertyChanged(this, e);
            }
        }

    }
}
