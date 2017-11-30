using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp2._0.Models
{
    public class DataSettings
    {
        public DataSettings()
        {
            Option1 = "Default1";
            Option2 = "Default2";
        }

        public string Option1 { get; set; }
        public string Option2 { get; set; }
    }
}
