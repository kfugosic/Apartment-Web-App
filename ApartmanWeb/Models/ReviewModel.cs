using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApartmanWeb.Models
{
    public class ReviewModel
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string Review { get; set; }
        public string Suggestions { get; set; }
        public int Score { get; set; }
        public bool GuestPermission { get; set; }
        public bool Approved { get; set; }

    }
}

