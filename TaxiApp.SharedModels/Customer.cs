using System;
using System.Collections.Generic;
using System.Text;

namespace TaxiApp.SharedModels
{
    public class Customer
    {
        public int id { get; set; }
        public string username { get; set; }
        public string passwordHash { get; set; }
        public DateTime datedJoined { get; set; }

        public string currentCoordinate { get; set; }
    }
}
