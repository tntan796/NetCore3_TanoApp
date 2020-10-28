using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TanoApp.Data.Enums;
using TanoApp.Infrastructure.ShareKernel;

namespace TanoApp.Data.Entities
{
    [Table("Contacts")]
    public class Contact: DomainEntity<string>
    {
        public Contact()
        {}
        public Contact(string id, string name, string phone, string email,
            string website, string address, string other, double? longtitude, double? latitude, Status status)
        {
            Id = id;
            Name = name;
            Phone = phone;
            Email = email;
            Website = website;
            Address = address;
            Other = other;
            Long = longtitude;
            Lat = latitude;
            Status = status;
        }
        [Required]
        [StringLength(255)]
        public string Name { set; get; }
        [StringLength(50)]
        public string Phone { set; get; }
        [StringLength(255)]
        public string Email { set; get; }
        [StringLength(255)]
        public string Website { set; get; }
        [StringLength(255)]
        public string Address { set; get; }
        public string Other { set; get; }
        public double? Lat { set; get; }
        public double? Long { set; get; }
        public Status Status { set; get; }
    }
}
