using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimedicus.Utils.Interfaces
{
    public interface INotifyPropertyChangedExtended
    {
        event PropertyChangedExtendedEventHandler PropertyChangedExtended;
    }
}
