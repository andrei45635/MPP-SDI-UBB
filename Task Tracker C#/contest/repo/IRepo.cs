using System.Collections.Generic;
using contest.domain;

namespace contest.repo
{
    public interface IRepository<ID, E> where E : Entity<ID>
    {
    
        IEnumerable<E> FindAll();
    
        E Save(E e);

        bool Delete(E e);

        E Update(E e);

        int Size();
    }
}