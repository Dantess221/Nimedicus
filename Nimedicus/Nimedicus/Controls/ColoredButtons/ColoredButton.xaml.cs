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

namespace Nimedicus.Controls.ColoredButtons
{
    /// <summary>
    /// Логика взаимодействия для ColoredButton.xaml
    /// </summary>
    /// <summary>
    /// Interaction logic for ColoredButton.xaml
    /// </summary>
    public partial class ColoredButton : BaseColoredButton
    {
        public ColoredButton()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }


}
