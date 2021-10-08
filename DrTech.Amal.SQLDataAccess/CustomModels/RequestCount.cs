using DrTech.Amal.SQLModels;
using System.Collections.Generic;

namespace DrTech.Amal.SQLDataAccess.CustomModels
{
    public class ArsRequestCount
    {
        public int TotalReplant { get; set; } = 0;
        public int TotalRefuse { get; set; } = 0;
        public int TotalReuse { get; set; } = 0;
        public int ToralReduse { get; set; } = 0;
        public int TotalRecycle { get; set; } = 0;
        public int TotalRegift { get; set; } = 0;
        public int TotalReport { get; set; } = 0;
        public int TotalBin { get; set; } = 0;
    }
}