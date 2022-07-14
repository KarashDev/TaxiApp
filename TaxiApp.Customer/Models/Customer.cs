using System;
using System.Collections.Generic;
using System.Text;

namespace TaxiApp.Customer.Models
{
    public class Customer /*: DomainObject*/
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public DateTime DatedJoined { get; set; }
    }
}
