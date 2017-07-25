using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;
namespace MSIT11404project1.Models
{
    public class Repository<T> : IRepository<T> where T : class
    {
        DbContext db = new MSIT11404Entities();
        DbSet<T> set =null;
        public Repository()
        {
            set = db.Set<T>();
        }
        public void Create(T _entity)
        {
            set.Add(_entity);
            
                db.SaveChanges();
            
           
        }

        public void Create(IEnumerable<T> _entyty) {
            foreach (var items in _entyty) {
                set.Add(items);

            }
            db.SaveChanges();
        }
        public void Delete(T _entity)
        {
            db.Entry(_entity).State = EntityState.Deleted;
            db.SaveChanges();
        }

        public IQueryable<T> GetAll()
        {
            return set;
        }

        

        public T GetByID(int id)
        {
            return set.Find(id);
        }

        public void Update(T _entity)
        {
            db.Entry(_entity).State = EntityState.Modified;
            db.SaveChanges();
        }
        //public void Update(T _entity, int id) {

        //    db.Entry(set.Find(id)).CurrentValues.SetValues(_entity);
            
        //    db.SaveChanges();
        //}

        
    }
}