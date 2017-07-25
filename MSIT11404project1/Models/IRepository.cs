using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace MSIT11404project1.Models
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        void Delete(T _entity);

        T GetByID(int id=0);
        void Create(IEnumerable<T> _entities);
        void Update(T _entity);
        void Create(T _entity);
    }
}