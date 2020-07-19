using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RustCrossbreed.BusinessLogic;
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
        private IBreedRepository BreedsRepo { get; }
        #endregion

        #region Constructors
        public HistoryViewModel(IRepository<HistoryModel> history, IBreedRepository breeds)
        {
            HistoryRepo = history ?? throw new ArgumentNullException(nameof(history));
            BreedsRepo = breeds ?? throw new ArgumentNullException(nameof(breeds));
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
            foreach (HistoryModel history in HistorySelected)
            {
                Breed[] children = BreedsRepo.FindChildren(history.Breed).ToArray();
                int?[] parentGens = FindParentGenerations(history.Breed);
                var vm = new MoreInfoViewModel(history.Breed, children, parentGens);

                //creating a window here violates MVVM, but thats a problem for another day
                var moreInforWindow = new Views.MoreInfoWindow(vm);
                moreInforWindow.Show();
            }
        }

        //this is just copypasta'd from MainWindowViewModel, will probably just move this out when I refactor
        private int?[] FindParentGenerations(Breed breed)
        {
            int?[] parentGenenerations = new int?[breed.ParentGenes.Length];

            for (int i = 0; i < breed.ParentGenes.Length; i++)
            {
                //if found, parent's generation is assigned, else null is assigned
                parentGenenerations[i] = BreedsRepo.Get(breed.ParentGenes[i])?.Generation;
            }

            return parentGenenerations;
        }
        #endregion
    }
}
