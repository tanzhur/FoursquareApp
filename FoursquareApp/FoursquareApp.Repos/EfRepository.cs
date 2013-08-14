using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FoursquareApp.Repos
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        public T Add(T item)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public T Update(int id, T item)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public bool Delete(T item)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public T Get(int id)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public IQueryable<T> All(string[] includes = null)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public IQueryable<T> Find(Expression<Func<T, int, bool>> predicate)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }
    }
}
