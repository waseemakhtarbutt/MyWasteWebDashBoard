using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
//using PMIU.WRMIS.DAL.Repositories;
using System.Data.Entity;
using DrTech.Amal.SQLDatabase;

namespace DrTech.Amal.SQLDataAccess
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal Amal_Entities context;
        internal DbSet<TEntity> dbSet;

        public Repository(Amal_Entities _context)
        {
            if (_context == null)
                throw new ArgumentNullException("context");
            dbSet = _context.Set<TEntity>();
            context = _context;

            context.Configuration.LazyLoadingEnabled = false;

        }

        public virtual TEntity FindById(object id)
        {
            return dbSet.Find(id);
        }
        public virtual TEntity FindByName(object Name)
        {
            return dbSet.Find(Name);
        }
        public virtual void InsertGraph(TEntity entity)
        {
            dbSet.Add(entity);
        }
        public virtual void Update(TEntity entity)
        {

            // dbSet;
            //DbEntityEntry dbEntityEntry =  context.Entry<TEntity>(entity);
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            //SetAuditingData<TEntity>(entity, false);
            context.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Delete(object id)
        {
            var entity = dbSet.Find(id);
            //var objectState = entity as IObjectState;
            //if (objectState != null)
            //    objectState.State = ObjectState.Deleted;
            Delete(entity);
        }
        public virtual void Delete(TEntity entity)
        {
            //dbSet.Attach(entity);
            // DbEntityEntry dbEntityEntry = context.Entry<TEntity>(entity);
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            // SetSoftDelete<TEntity>(entity);
            context.Entry(entity).State = EntityState.Deleted;
            dbSet.Remove(entity);
        }
        public virtual void Insert(TEntity entity)
        {
            //SetAuditingData<TEntity>(entity, true);
            //dbSet.Attach(entity);
            dbSet.Add(entity);
        }
        public void Save()
        {
            context.SaveChanges();
        }
        public virtual RepositoryQuery<TEntity> Query()
        {
            var repositoryGetFluentHelper =
                new RepositoryQuery<TEntity>(this);

            return repositoryGetFluentHelper;
        }
        internal IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, List<Expression<Func<TEntity, object>>> includeProperties = null, int? page = null, int? pageSize = null)
        {
            IQueryable<TEntity> query = dbSet;
            if (includeProperties != null)
                includeProperties.ForEach(i => { query = query.Include(i); });
            if (filter != null)
                query = query.Where(filter);
            if (orderBy != null)
                query = orderBy(query);
            if (page != null && pageSize != null)
                query = query
                    .Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);

            return query;
        }
        public IEnumerable<TEntity> ExecWithStoreProcedure(string query, params object[] parameters)
        {
            return context.Database.SqlQuery<TEntity>(query, parameters);
        }

        public IQueryable<TEntity> GetAll()
        {
            return Query().Get();
        }

        public IQueryable<TEntity> GetPage(int _PageNo, int _PageSize)
        {
            return Query().Get().Skip((_PageNo - 1) * _PageSize).Take(_PageSize);
        }

        //public IQueryable<TSource> GetPageRecords<TSource>(this IQueryable<TSource> source, int page, int pageSize)
        //{
        //    return source.Skip((page - 1) * pageSize).Take(pageSize);
        //}

        //public IEnumerable<TSource> GetPageRecords<TSource>(this IEnumerable<TSource> source, int page, int pageSize)
        //{
        //    return source.Skip((page - 1) * pageSize).Take(pageSize);
        //}

        public List<TSource> GetPageRecords<TSource>(List<TSource> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize).ToList<TSource>();
        }

        //public IEnumerable<TSource> GetPageRecords<TSource>(int page, int pageSize)
        //{
        //    return Query().Get().Skip((page - 1) * pageSize).Take(pageSize).AsEnumerable<TSource>();
        //}

        //public T SetAuditingData<T>(T _Object, bool _IsAdded)
        //{
        //    Type type = _Object.GetType();
        //    foreach (var propInfo in type.GetProperties())
        //    {
        //        switch (propInfo.Name)
        //        {
        //            case Common.Constants.Auditing.CreatedDate:
        //                if (_IsAdded)
        //                    propInfo.SetValue(_Object, DateTime.Now, null);
        //                break;
        //            case Common.Constants.Auditing.CreatedBy:
        //                if (_IsAdded)
        //                    if (propInfo.PropertyType == typeof(System.Nullable<System.Int64>) || propInfo.PropertyType == typeof(System.Int64))
        //                    {
        //                        if (Common.SessionValues.LoggedInUserID != null)
        //                        {
        //                            if (Common.SessionValues.LoggedInUserID > 0)
        //                            {
        //                                propInfo.SetValue(_Object, Common.SessionValues.LoggedInUserID, null);
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (Common.SessionValues.LoggedInUserID != null)
        //                        {
        //                            if (Common.SessionValues.LoggedInUserID > 0)
        //                            {
        //                                propInfo.SetValue(_Object, Convert.ToInt32(Common.SessionValues.LoggedInUserID), null);
        //                            }
        //                        }
        //                    }
        //                break;
        //            case Common.Constants.Auditing.ModifiedDate:
        //                propInfo.SetValue(_Object, DateTime.Now, null);
        //                break;
        //            case Common.Constants.Auditing.ModifiedBy:
        //                if (propInfo.PropertyType == typeof(System.Nullable<System.Int64>) || propInfo.PropertyType == typeof(System.Int64))
        //                {
        //                    if (Common.SessionValues.LoggedInUserID != null && Common.SessionValues.LoggedInUserID > 0)
        //                    {
        //                        propInfo.SetValue(_Object, Common.SessionValues.LoggedInUserID, null);
        //                    }
        //                    //propInfo.SetValue(_Object, Common.SessionValues.LoggedInUserID, null);
        //                }
        //                else
        //                {
        //                    if (Common.SessionValues.LoggedInUserID != null && Common.SessionValues.LoggedInUserID > 0)
        //                    {
        //                        propInfo.SetValue(_Object, Convert.ToInt32(Common.SessionValues.LoggedInUserID), null);
        //                    }                                
        //                    //propInfo.SetValue(_Object, Convert.ToInt32(Common.SessionValues.LoggedInUserID), null);
        //                }
        //                break;
        //            case Common.Constants.Auditing.IsActive:
        //                if (_IsAdded && propInfo.GetValue(_Object) == null)
        //                    propInfo.SetValue(_Object, true, null);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    return _Object;
        //}
        //public T SetSoftDelete<T>(T _Object)
        //{
        //    Type type = _Object.GetType();
        //    foreach (var propInfo in type.GetProperties())
        //    {
        //        switch (propInfo.Name)
        //        {
        //            case Common.Constants.Auditing.IsActive:
        //                propInfo.SetValue(_Object, false, null);
        //                break;
        //        }
        //    }
        //    return _Object;
        //}
    }

}
