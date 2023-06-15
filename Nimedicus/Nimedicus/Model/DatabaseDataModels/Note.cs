using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimedicus.Model.DatabaseDataModels
{
    public class Note
    {
        [Key]
        public int NoteId { get; set; }

        public string Message { get; set; }
        public DateTime DataCreation { get; set; }

        [ForeignKey("AttachedNurse")]
        public string AttachedNurseId { get; set; }

        [ForeignKey("Patient")]
        public string PatientLogin { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual Nurse AttachedNurse { get; set; }
    }
}
