using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TanoApp.Data.Enums;
using TanoApp.Data.Interfaces;
using TanoApp.Infrastructure.ShareKernel;

namespace TanoApp.Data.Entities
{
    [Table("Languages")]
    public class Language: DomainEntity<string>, ISwitchable
    {
        [Required]
        [StringLength(128)]
        public string Name { set; get; }
        public bool IsDefault { set; get; }
        public string Resources { set; get; }
        public Status Status { set; get; }

    }
}
