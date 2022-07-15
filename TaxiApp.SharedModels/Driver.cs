using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TaxiApp.SharedModels
{
    public class Driver
    {
        public int id { get; set; }
        public string username { get; set; }
        public string passwordHash { get; set; }
        public DateTime datedJoined { get; set; }

        [NotMapped]
        public Customer customer { get; set; }
    }
}
