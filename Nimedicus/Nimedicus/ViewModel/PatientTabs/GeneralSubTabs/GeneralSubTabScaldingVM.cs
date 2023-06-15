using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimedicus.ViewModel.PatientTabs.GeneralSubTabs
{
    internal class GeneralSubTabScaldingVM : BaseTab
    {
        public override Task Init()
        {
            return Task.CompletedTask;
        }

        private GeneralVM _host;
        public GeneralVM Host { get => _host; set => SetProperty(ref _host, value); }
        public GeneralSubTabScaldingVM(GeneralVM host)
        {
            Host = host;
        }
    }
}
