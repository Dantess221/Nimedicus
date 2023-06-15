using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimedicus.Model.DatabaseDataModels
{
    public class Auth
    {
        [Key]
        [StringLength(50)]
        public string Login { get; set; }

        [StringLength(50)]
        public string Password { get; set; }
    }
}
