
using DrTech.Amal.SQLDatabase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.SQLDataAccess
{
    public class ContextDB : IContextDB
    {

        private bool _disposed;
        private Hashtable _repositories;
        private Amal_Entities _context;
        string sConnectionString;

        public ContextDB()
        {
            Amal_Entities context = new Amal_Entities();

            _context = context ?? throw new ArgumentNullException("context");

            sConnectionString = _context.Database.Connection.ConnectionString;
        }

        //public PHC_Inspection_DBEntities GetContext()
        //{
        //    return DBcontext;
        //}

        public T ExtRepositoryFor<T>() where T : class
        {
            //return  T();
            if (_context == null)
                _context = new Amal_Entities();

            return (T)Activator.CreateInstance(typeof(T), _context);
        }

        public IRepository<T> Repository<T>() where T : class
        {
            //  return new Repository<T>(DBcontext);
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                            .MakeGenericType(typeof(T)), _context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)_repositories[type];

            // return Activator.CreateInstance(typeof(T));
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();
            _disposed = true;
        }

        //public IEnumerable<DataRow> ExecuteDataSet(string spName, params object[] parameterValues)
        //{
        //    DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(sConnectionString, spName, parameterValues);

        //    //var empList = ds.Tables[0].AsEnumerable().Select(dataRow => new
        //    //{
        //    //    CustomerName = dataRow.Field<string>("CustomerName"),
        //    //    OrderNumber = dataRow.Field<string>("OrderNumber")
        //    //}).ToList();

        //    return ds.Tables[0].AsEnumerable();
        //}

        //public int ExecuteNonQuery(string spName, params object[] parameterValues)
        //{
        //    int result = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(sConnectionString, spName, parameterValues);
        //    return result;
        //}

        //public DataTable ExecuteStoredProcedureDataTable(string spName, params object[] parameterValues)
        //{
        //    string sConnectionString = _context.Database.Connection.ConnectionString;

        //    DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(sConnectionString, spName, parameterValues);
        //    return ds.Tables[0];

        //}

        //public DataSet ExecuteStoredProcedureDataSet(string spName, params object[] parameterValues)
        //{
        //    string sConnectionString = _context.Database.Connection.ConnectionString;

        //    return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(sConnectionString, spName, parameterValues);
        //}

        //public int ExecuteScalar(string spName, params object[] parameterValues)
        //{
        //    object result = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(sConnectionString, spName, parameterValues);
        //    return Convert.ToInt32(result);
        //}
        //public double ExecuteScalarDouble(string spName, params object[] parameterValues)
        //{
            
        //    object result = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(sConnectionString, spName, parameterValues);
        //    return Convert.ToDouble(result);
        //}
    }

}
