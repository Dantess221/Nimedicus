using SharpVectors.Dom.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimedicus.Model.DatabaseDataModels
{
    public class Patient : BaseUser
    {
        [ForeignKey("Doctor")]
        public string DoctorLogin { get; set; }

        public virtual Doctor Doctor { get; set; }

        public virtual ICollection<Note> DoctorsNotes { get; set; } = new List<Note>();
        public virtual ICollection<AnalysData> Analysis { get; set; } = new List<AnalysData>();
        public virtual ICollection<ReceiptData> Receipts { get; set; } = new List<ReceiptData>();
        public virtual ICollection<Event> History { get; set; } = new List<Event>();
    }

}
