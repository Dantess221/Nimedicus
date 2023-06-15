using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimedicus.Model.DatabaseDataModels
{
    public class Doctor : BaseUser
    {
        public virtual ICollection<Patient> AttachedPatients { get; set; }
    }
}
