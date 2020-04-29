using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.Extensions.Configuration;

namespace WebApplication1.Models
{

    public class DataFieldsViewModel
    {
        
        public string DataFields { get; set; }

        public string CustomField { get; set; }

        public string DataMapping { get; set; }


    }

    public class InPutField
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}
