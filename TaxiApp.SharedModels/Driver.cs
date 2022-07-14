using System;
using System.Collections.Generic;
using System.Text;

namespace TaxiApp.SharedModels
{
    public class Driver /*: DomainObject*/
    {
        public int id { get; set; }
        public string username { get; set; }
        public string passwordHash { get; set; }
        public DateTime datedJoined { get; set; }
    }
}
