using System;
using System.Collections.Generic;
using System.Text;

namespace TaxiApp.SharedModels
{
    public class Driver /*: DomainObject*/
    {
        public int id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public DateTime DatedJoined { get; set; }
    }
}
