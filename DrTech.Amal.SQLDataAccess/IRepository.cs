using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess
{
    public interface IRepository<TEntity> where TEntity : class
    {

        TEntity FindById(object id);
        TEntity FindByName(object Name);
        void InsertGraph(TEntity entity);
        void Update(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);
        void Insert(TEntity entity);
        RepositoryQuery<TEntity> Query();
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetPage(int _PageNo, int _PageSize);
        //IQueryable<TSource> GetPageRecords<TSource>(this IQueryable<TSource> source, int page, int pageSize);
        //IEnumerable<TSource> GetPageRecords<TSource>(this IEnumerable<TSource> source, int page, int pageSize);
        List<TSource> GetPageRecords<TSource>(List<TSource> source, int page, int pageSize);

    }
}
