using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace GemSwipe.Data
{
    public abstract class RepositoryBase<T> : IRepository<T> where T:class
    {
        protected IList<T> Data;

        protected RepositoryBase()
        {
            Initialize();
        }

        public IList<T> GetAll()
        {
            return Data;
        }

        public int Count()
        {
            return Data.Count;
        }

        public T Get(int id)
        {
            return Data.Single(l => (int) l.GetType().GetRuntimeProperty("Id").GetValue(l) == id);
        }

        private void Initialize()
        {
            var className = typeof(T).Name;
            string jsonString = ResourceLoader.LoadStringAsync($"Data/{className}/{className}.json").Result;
            Data = JsonConvert.DeserializeObject<List<T>>(jsonString);
        }
    }
}
