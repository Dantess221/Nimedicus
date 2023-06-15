using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimedicus.Model.DatabaseDataModels
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        public DateTime DataCreation { get; set; }
        public string Message { get; set; }


        [ForeignKey("Patient")]
        public string PatientLogin { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
