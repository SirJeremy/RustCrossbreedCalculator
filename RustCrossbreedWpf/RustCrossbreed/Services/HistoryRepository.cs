using RustCrossbreed.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RustCrossbreed.Services
{
    public class HistoryRepository : IRepository<HistoryModel>
    {
        #region Properties
        private ObservableCollection<HistoryModel> History { get; }
        #endregion

        #region Constructors
        public HistoryRepository()
        {
            History = new ObservableCollection<HistoryModel>();
        }
        public HistoryRepository(IEnumerable<HistoryModel> history)
        {
            History = new ObservableCollection<HistoryModel>(history);
        }
        #endregion

        #region IRepository
        public void Add(HistoryModel item)
        {
            History.Add(item);
        }

        public void Clear()
        {
            History.Clear();
        }

        public bool Contains(HistoryModel item)
        {
            return History.Contains(item);
        }

        public int Count()
        {
            return History.Count;
        }

        public HistoryModel Get(int index)
        {
            return History[index];
        }

        public ObservableCollection<HistoryModel> GetAll()
        {
            return History;
        }

        public int GetIndex(HistoryModel item)
        {
            return History.IndexOf(item);
        }

        public bool Remove(HistoryModel item)
        {
            return History.Remove(item);
        }

        public void RemoveAt(int index)
        {
            History.RemoveAt(index);
        }
        #endregion
    }
}
