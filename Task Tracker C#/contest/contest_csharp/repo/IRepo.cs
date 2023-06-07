using System.Collections.Generic;
using contest_csharp.domain;

namespace contest_csharp.repo
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