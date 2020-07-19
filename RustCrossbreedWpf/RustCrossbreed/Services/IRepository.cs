using RustCrossbreed.Models;
using System.Collections.ObjectModel;

namespace RustCrossbreed.Services
{
    public interface IRepository<T>
    {
        public void Add(T item);

        public bool Remove(T item);
        public void RemoveAt(int index);

        public T Get(int index);
        public int GetIndex(T item);
        public ObservableCollection<T> GetAll();

        public bool Contains(T item);

        public void Clear();

        public int Count();
    }
}
