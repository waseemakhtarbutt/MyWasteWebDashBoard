using DrTech.Amal.SQLModels;
using System.Collections.Generic;

namespace DrTech.Amal.SQLServices.Models
{
    public class RegiftCollection
    {
        public int OrderID { get; set; }
        public string Quality { get; set; }
        public int GreenPoints { get; set; }
        public List<RegiftSubItem> RegiftSubItems { get ; set ; }
    }
}