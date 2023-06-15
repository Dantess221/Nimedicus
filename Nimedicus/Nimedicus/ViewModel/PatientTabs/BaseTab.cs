using Nimedicus.Utils.ZBindable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimedicus.ViewModel.PatientTabs
{
    public abstract class BaseTab : ZBindable
    {
        public abstract Task Init();
        public virtual void Refresh() { }
    }
}
