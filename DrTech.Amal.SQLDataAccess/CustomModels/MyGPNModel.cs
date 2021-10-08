using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
    public class MyGPNModel
    {
        public Int32 ID { get; set; }

        public string FileName { get; set; }
        public string Name { get; set; }

        public int GreenPoints { get; set; }

        public string Level { get; set; }

        public string Type { get; set; }
    }
}
