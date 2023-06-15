using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimedicus.Utils.Attributes
{
    internal class DoNotCacheViewAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class NotifyPropertyChangedInvocatorAttribute : Attribute
    {
        public NotifyPropertyChangedInvocatorAttribute() { }
        public NotifyPropertyChangedInvocatorAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        public string ParameterName { get; private set; }
    }
}
