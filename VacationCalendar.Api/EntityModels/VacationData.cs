using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationCalendar.Api.EntityModels
{
    public class VacationData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public DateTime VacationDate { get; set; }
        public int UserID { get; set; }
        public int VacationTypeID { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
