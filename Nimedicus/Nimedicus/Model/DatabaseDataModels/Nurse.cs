using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimedicus.Model.DatabaseDataModels
{
    public class Nurse : BaseUser
    {
        public virtual ICollection<Patient> AttachedPatients { get; set; }
    }
}
