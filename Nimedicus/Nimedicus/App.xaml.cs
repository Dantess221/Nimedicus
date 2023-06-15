using Nimedicus.Model;
using Nimedicus.View;
using Prism.Ioc;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Nimedicus
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    internal partial class App : PrismApplication
    {
        public MainWindow m_Wnd { get; set; }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
        protected override Window CreateShell()
        {
            m_Wnd = new MainWindow();
            return m_Wnd;
        }
    }
}
