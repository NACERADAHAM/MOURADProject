﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Web.Models
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }
        public string UserId { get; set; }
        public string CouponCode { get; set; }
        public Double OrderTotal { get; set; }
        public Double DiscountTotal { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public DateTime PickupDateTime { get; set; }
        public  string  Phone { get; set; }
        public string Email { get; set; }
        public string CartNumber { get; set; }
        public string CVV { get; set; }
        public string ExpiryMonthYear { get; set; }
    }
}
