using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
    public class CommentsViewModel
    {
        public CommentsViewModel()
        { }

        public int ID { get; set; }
        public string Comments { get; set; }
        public string Date { get; set; }
        public string User { get; set; }
        public string Phone { get; set; }
        public int RID { get; set; }
    }
}
