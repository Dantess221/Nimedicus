using Chilkat;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimedicus.Model.DatabaseDataModels
{
    public class AnalysData
    {
        [Key]
        public int AnalysDataId { get; set; }

        public string AnalysName { get; set; }
        public DateTime DataCreation { get; set; }
        // Другие свойства для анализов

        [ForeignKey("Patient")]
        public string PatientLogin { get; set; }

        public string AnalysFileBlobName { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
