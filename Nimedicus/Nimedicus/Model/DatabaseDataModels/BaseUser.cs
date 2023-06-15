using Nimedicus.Utils.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimedicus.Model.DatabaseDataModels
{
    public abstract class BaseUser
    {
        [Key]
        [StringLength(50)]
        public string Login { get; set; }

        public Sex Sex { get; set; }

        public DateTime BirthdayDate { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(100)]
        public string Mail { get; set; }

    }
}
