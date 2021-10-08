using DrTech.Amal.SQLModels;
using System.Collections.Generic;

namespace DrTech.Amal.SQLServices.Models
{
    public class RecycleCollection
    {
        public int OrderID { get; set; }
        public int Cash{ get; set; }
        public int GreenPoints { get; set; }
        public List<RecycleSubItem> RecycleSubItems { get; set; }
    }
}