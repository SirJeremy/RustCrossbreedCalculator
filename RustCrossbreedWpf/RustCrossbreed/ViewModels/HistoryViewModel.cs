using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RustCrossbreed.Models;
using RustCrossbreed.Services;

namespace RustCrossbreed.ViewModels
{
    public class HistoryViewModel : ViewModelBase
    {
        #region Properties
        public ObservableCollection<HistoryModel> History { get => HistoryRepo.GetAll(); }
        public List<HistoryModel> HistorySelected { get; set; }

        private IRepository<HistoryModel> HistoryRepo { get; }

        #endregion

        #region Constructors
        public HistoryViewModel(IRepository<HistoryModel> history)
        {
            HistoryRepo = history ?? throw new ArgumentNullException(nameof(history));
        }
        #endregion

        #region Methods
        public void DeleteSelected()
        {
            foreach (HistoryModel history in HistorySelected.ToArray())
            {
                HistoryRepo.Remove(history);
            }
        }
        public void DeleteAll()
        {
            HistoryRepo.Clear();
        }
        public void OnMoreInfoClick()
        {

        }
        #endregion
    }
}
