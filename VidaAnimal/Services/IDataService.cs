using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace VidaAnimal.Services
{
    public interface IDataService<T>
    {
        IEnumerable<T> GetAll();
        void Create(T entity);
        T GetSingle(Expression<Func<T, bool>> predicate);
        IEnumerable<T> Query(Expression<Func<T, bool>> predicate);
        void Update(T entity);
        void Delete(T entity);

    }
}
