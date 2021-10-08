using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DrTech.Amal.SQLDataAccess
{
    public interface IContextDB : IDisposable
    {
        T ExtRepositoryFor<T>() where T : class;
        void Dispose();
        void Save();
        void Dispose(bool disposing);

        //PHC.Inspection.DataAccess.PHC_Inspection_DBEntities GetContext();
        IRepository<T> Repository<T>() where T : class;

        //IEnumerable<DataRow> ExecuteDataSet(string spName, params object[] parameterValues);

        //DataTable ExecuteStoredProcedureDataTable(string spName, params object[] parameterValues);

        //DataSet ExecuteStoredProcedureDataSet(string spName, params object[] parameterValues);
    }
}
