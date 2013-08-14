﻿namespace FoursquareApp.Repos
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    public class EfRepository<T> : IRepository<T> where T : class
    {
        public EfRepository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("An instance of DbContext is required to use this repository.", "context");
            }

            ////SERIALIZE WILL FAIL WITH PROXIED ENTITIES
            //context.Configuration.ProxyCreationEnabled = false;

            ////ENABLING COULD CAUSE ENDLESS LOOPS AND PERFORMANCE PROBLEMS
            //context.Configuration.LazyLoadingEnabled = false;

            this.Context = context;
            this.DbSet = this.Context.Set<T>();
        }

        protected IDbSet<T> DbSet { get; set; }

        protected DbContext Context { get; set; }

        public T Add(T item)
        {
            this.DbSet.Add(item);
            this.Context.SaveChanges();
            return item;
        }

        public T Update(int id, T item)
        {
            this.Context.Entry<T>(item).CurrentValues.SetValues(item);
            this.Context.SaveChanges();
            return item;
        }

        public bool Delete(int id)
        {
            T item = this.DbSet.Find(id);

            if (item != null)
            {
                this.DbSet.Remove(item);
                this.Context.SaveChanges();

                return true;
            }

            return false;
        }

        public bool Delete(T item)
        {
            T result = this.DbSet.Find(item);

            if (result != null)
            {
                this.DbSet.Remove(result);
                this.Context.SaveChanges();

                return true;
            }

            return false;
        }

        public T Get(int id)
        {
            return this.DbSet.Find(id);
        }

        public IQueryable<T> All(string[] includes = null)
        {
            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Length > 0)
            {
                var query = this.DbSet.Include(includes.First());
                foreach (var include in includes.Skip(1))
                {
                    query = query.Include(include);
                }

                return query.AsQueryable();
            }

            return this.DbSet.AsQueryable();
        }

        public IQueryable<T> Find(Expression<Func<T, int, bool>> predicate)
        {
            return this.DbSet.Where(predicate);
        }
    }
}
