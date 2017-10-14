using System.Collections.Generic;

namespace GemSwipe.Data
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);

        IList<T> GetAll();

        int Count();
    }
}
