using Nimedicus.Model.DatabaseDataModels;
using Nimedicus.ViewModel.PatientTabs.GeneralSubTabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimedicus.ViewModel.PatientTabs
{
    internal class GeneralVM : BaseTab
    {
        public override Task Init()
        {
            return Task.CompletedTask;
        }

        private List<BaseTab> _pagesVMs;
        private GeneralSubTabGeneralVM _pageGeneral;
        private GeneralSubTabAnalysVM _pageAnalys;
        private GeneralSubTabScaldingVM _pageScalding;
        private bool IsInitializing { get; set; }

        private PatientTabsVM _host;
        public PatientTabsVM Host { get => _host; set => SetProperty(ref _host, value); }
        public GeneralVM(PatientTabsVM host)
        {
            Host = host;

            _pagesVMs = new List<BaseTab>();
            _pageGeneral = new GeneralSubTabGeneralVM(this);
            _pageAnalys = new GeneralSubTabAnalysVM(this);
            _pageScalding = new GeneralSubTabScaldingVM(this);

            _pagesVMs.Add(_pageGeneral);
            _pagesVMs.Add(_pageScalding);
            _pagesVMs.Add(_pageAnalys);
        }

        public async void Initialize()
        {
            IsInitializing = true;
            foreach (var page in _pagesVMs)
            {
                await page.Init();
            }
            IsInitializing = false;
        }

        public void Refresh()
        {
            foreach (var page in _pagesVMs)
            {
                page.Refresh();
            }
        }
        public void SelectPage(Type selectPage)
        {
            int i = 0;
            foreach (var page in _pagesVMs)
            {
                if (page.GetType() == selectPage)
                {
                    SelectedPage = i;
                    break;
                }
                i++;
            }
        }

        public BaseTab CurrentPage => _pagesVMs[SelectedPage];

        private int _selectedPage = 0;
        public int SelectedPage
        {
            get { return _selectedPage; }
            set
            {
                if (SetProperty<int>(ref _selectedPage, value))
                {
                    OnPropertyChanged(nameof(CurrentPage));
                }
            }
        }
    }
}
