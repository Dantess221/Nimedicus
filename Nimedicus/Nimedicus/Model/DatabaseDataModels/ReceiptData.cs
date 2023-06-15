using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Nimedicus.Model.DatabaseDataModels
{
    public class ReceiptData
    {
        [Key]
        public int ReceiptDataId { get; set; }
        public DateTime DataCreation { get; set; }
        public DateTime DataExpiration { get; set; }
        public string ListDrugsJson { get; set; }

        [ForeignKey("Patient")]
        public string PatientLogin { get; set; }

        public virtual Patient Patient { get; set; }

        [ForeignKey("Doctor")]
        public string DoctorLogin { get; set; }

        public virtual Patient Doctor { get; set; }

        public List<string> GetDrugsList()
        {
            if (string.IsNullOrEmpty(ListDrugsJson))
            {
                return new List<string>();
            }

            return JsonConvert.DeserializeObject<List<string>>(ListDrugsJson);
        }

        public void SetDrugsList(List<string> drugs)
        {
            ListDrugsJson = JsonConvert.SerializeObject(drugs);
        }

    }
}
