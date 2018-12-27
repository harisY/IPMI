using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPMI.Repository
{
    interface IRepository<T> : IDisposable where T : class
    {
        IEnumerable<T> ExecuteQuery(string spQuery, object[] parameters);
        IEnumerable<T> ExecuteQuery(string spQuery);
        T ExecuteQuerySingle(string spQuery, object[] parameters);
        int ExecuteCommand(string spQuery, object[] parameters);
        string ExecuteQuerySingle1(string spQuery, object[] parameters);
    }
}
