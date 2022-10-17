using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    internal class NewCustomerModel
    { 
        [Name("First Name")]
        public string firstName { get; set; }

        [Name("Last Name")]
        public string lastName { get; set; }

        [Name("Street Number")]
        public string streetNumber { get; set; }

        [Name("Street")]
        public string street { get; set; }

        [Name("City")]
        public string city { get; set; }

        [Name("Province")]
        public string province { get; set; }

        [Name("Postal Code")]
        public string postalCode { get; set; }

        [Name("Country")]
        public string country { get; set; }
        [Name("Phone Number")]
        public string phoneNumber { get; set; }
        [Name("email Address")]
        public string emailAddress { get; set; }
        [Name("Date")]
        public string date { get; set; }
    }
}
