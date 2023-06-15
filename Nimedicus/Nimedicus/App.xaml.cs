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

            new DatabaseManager().CreateNurse("Nurse", Utils.Enums.Sex.Female, new DateTime(1988, 6, 4), "Валерія", "Шевчук", "Олександрівна", "+38063111111", "62333, Київська область, місто Київ, пров. Б. Грінченка, 20", "nurse@gmail.com", "Nurse");

            new DatabaseManager().CreateDoctor("Doctor1", Utils.Enums.Sex.Male, new DateTime(1978, 2, 23), "Григорій", "Середа", "Михайлович", "+38063222222", "63740, Київська область, місто Київ, пров. Михайла Грушевського, 64", "doctor1@gmail.com", "Doctor1");

            new DatabaseManager().CreateDoctor("Doctor2", Utils.Enums.Sex.Female, new DateTime(1984, 6, 13), "Марія", "Пономаренко", "Федорівна", "+38063333333", "18130, Київська область, місто Київ, вул. Львівська, 97", "doctor2@gmail.com", "Doctor2");

            return m_Wnd;
        }
    }
}
