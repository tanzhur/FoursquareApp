using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FoursquareApp.Repos
{
    public interface IRepository<T>
    {
        T Add(T item);

        T Update(int id, T item);

        bool Delete(int id);

        bool Delete(T item);

        T Get(int id);

        IQueryable<T> All(string[] includes = null);

        IQueryable<T> Find(Expression<Func<T, int, bool>> predicate);
    }
}
