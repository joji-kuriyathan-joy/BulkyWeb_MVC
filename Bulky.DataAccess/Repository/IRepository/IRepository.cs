using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class 
    {
       // T- Category for any other Genric model
       //Get all category
        IEnumerable<T> GetAll();
        //Get One category
        T Get(Expression<Func<T, bool>> filter);
        //Create a category
        void Add(T entity);
        //remove a category
        void Remove(T entity);
        //remove multiple category
        void RemoveRange(IEnumerable<T> entity);
    }
}
