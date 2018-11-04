using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wcfService1.Database
{
    public class CustOrderHist
    {
        //[Key]
        //public int id { get; set; }

        [Key]
        public string ProductName { get; set; }

        public int Total { get; set; }
    }
}