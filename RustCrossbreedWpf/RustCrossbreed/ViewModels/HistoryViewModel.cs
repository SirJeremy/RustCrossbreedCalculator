using RustCrossbreed.Models;
using RustCrossbreed.Services;
using System;
using System.Collections.ObjectModel;

namespace RustCrossbreed.ViewModels
{
    public class HistoryViewModel : ViewModelBase
    {
        #region Properties
        public ObservableCollection<HistoryModel> History { get => HistoryRepo.GetAll(); }

        private IRepository<HistoryModel> HistoryRepo { get; }

        #endregion

        #region Constructors
        public HistoryViewModel(IRepository<HistoryModel> history)
        {
            HistoryRepo = history ?? throw new ArgumentNullException(nameof(history));
        }
        #endregion
    }
}
