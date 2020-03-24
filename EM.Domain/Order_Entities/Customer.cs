using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EM.Domain.Order_Entities
{
    public class Customer
    {
        [Key]
        public int CustomerNumber { get; set; }
        public string EMail { get; set; }

        public DateTime BirthDate { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }
        public string PostalCode { get; set; }

        public virtual List<Order> Orders { get; set; }
    }
}
