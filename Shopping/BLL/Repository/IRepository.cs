using Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BLL.Repository
{
    public interface IRepository<T> where T:BaseEntity
    {
        CrudState Add(T entity);
        CrudState Add(IEnumerable<T> entity);
        List<T> Get();
        List<T> Get(Expression<Func<T, bool>> where);
        CrudState Update(T entity);
        CrudState Delete(string id);
        CrudState Delete(IEnumerable<T> entities);
    }
    public enum CrudState
    {
        Success,
        EntityError,
        ConnectionError,
        NotFound
    }
    public enum ConnectionState
    {
        Success,
        ConnectionError
    }
}
