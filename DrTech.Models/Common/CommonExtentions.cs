using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrTech.Models.Common
{
    public static class CommonExtentions
    {
        public static List<TModel> ToSortByCreationDateDescendingOrder<TModel>(this List<TModel> list) where TModel : BaseModel
        {
            list = list?.OrderByDescending(p => p.CreatedAt).ToList();
            return list;
        }
            
    }
}
