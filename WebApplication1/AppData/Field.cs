﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.AppData
{
    public class Field: IField
    { 
        public string Name { get; set; }
        public int? length { get; set; }
    }
}
