﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TanoApp.Data.Enums;

namespace TanoApp.Application.ViewModels.Products
{
    public class BillViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string CustomerName { set; get; }
        [Required]
        [MaxLength(255)]
        public string CustomerAddress { set; get; }
        [Required]
        [MaxLength(50)]
        public string CustomerMobile { set; get; }
        [Required]
        [MaxLength(255)]
        public string CustomerMessage { set; get; }
        public PaymentMethod PaymentMethod { set; get; }
        public BillStatus BillStatus { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
        public Status Status { set; get; }
        public Guid? CustomerId { set; get; }
        public List<BillDetailViewModel> BillDetails { set; get; }
    }
}
