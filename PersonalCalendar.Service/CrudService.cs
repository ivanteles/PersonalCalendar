using PersonalCalendar.Data;
using PersonalCalendar.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PersonalCalendar.Service
{
    public interface ICrudService<T> where T : Entity, new()
    {
        int Create(T item);

        void Update(T item);

        void Save();

        void Delete(int id);

        T Find(int id);

        IEnumerable<T> GetAll();

        IEnumerable<T> Where(Expression<Func<T, bool>> predicate);
    }

    public class CrudService<T> : ICrudService<T> where T : Entity, new()
    {
        protected readonly CalendarDB _calendarDB;

        public CrudService(CalendarDB context)
        {
            _calendarDB = context;
        }

        public virtual int Create(T item)
        {
            _calendarDB.Set<T>().Add(item);
            
            Save();

            return item.Id;
        }

        public virtual void Update(T item)
        {
            _calendarDB.Entry(item).State = EntityState.Modified;

            Save();
        }

        public void Save()
        {
            _calendarDB.SaveChanges();
        }

        public virtual void Delete(int id)
        {
            var item = Find(id);

            _calendarDB.Set<T>().Remove(item);

            Save();
        }

        public T Find(int id)
        {
            return _calendarDB.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _calendarDB.Set<T>();
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _calendarDB.Set<T>().Where(predicate);
        }
    }
}
