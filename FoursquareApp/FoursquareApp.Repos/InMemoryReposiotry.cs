namespace FoursquareApp.Repos
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class InMemoryReposiotry<T> : IRepository<T> where T : class
    {
        private readonly List<T> localRepo = new List<T>();

        public T Add(T item)
        {
            this.localRepo.Add(item);
            return item;
        }

        public T Update(int id, T item)
        {
            this.localRepo[id] = item;
            return item;
        }

        public bool Delete(int id)
        {
            this.localRepo.RemoveAt(id);
            return true;
        }

        public bool Delete(T item)
        {
            this.localRepo.RemoveAll(x => x == item);
            return true;
        }

        public T Get(int id)
        {
            return this.localRepo[id];
        }

        public IQueryable<T> All(string[] includes = null)
        {
            return this.localRepo.AsQueryable();
        }

        public IQueryable<T> Find(Expression<Func<T, int, bool>> predicate)
        {
            //return this.localRepo.Where(predicate);
            throw new NotImplementedException();
        }
    }
}
