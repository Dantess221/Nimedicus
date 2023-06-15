using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Nimedicus.Utils.ExtensionMethods
{
    public static class FreezableExtensions
    {
        public static void TryFreeze(this Freezable freezable)
        {
            if (freezable.CanFreeze)
                freezable.Freeze();
        }
    }
}
