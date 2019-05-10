using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VacationCalendar.Api.EntityModels
{
    public class VacationType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string VacationTypeName { get; set; }

        [ForeignKey("VacationTypeID")]
        public virtual ICollection<VacationData> VacationData { get; set; }
    }
}
