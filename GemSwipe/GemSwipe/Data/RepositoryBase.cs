using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace GemSwipe.Data
{
    public abstract class RepositoryBase<T> : IRepository<T> where T:class
    {
        private IList<T> _data;

        protected RepositoryBase()
        {
            Initialize();
        }

        public IList<T> GetAll()
        {
            return _data;
        }

        public int Count()
        {
            return _data.Count;
        }

        public T Get(int id)
        {
            return _data.Single(l => (int) l.GetType().GetRuntimeProperty("Id").GetValue(l) == id);
        }

        private void Initialize()
        {
            var className = typeof(T).Name;
            string jsonString = ResourceLoader.LoadStringAsync($"Data/{className}/{className}.json").Result;
            _data = JsonConvert.DeserializeObject<List<T>>(jsonString);
        }
    }
}
