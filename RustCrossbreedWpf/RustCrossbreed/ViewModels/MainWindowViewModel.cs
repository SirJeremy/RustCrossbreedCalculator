using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RustCrossbreed.Models;
using RustCrossbreed.BusinessLogic;
using RustCrossbreed.Services;
using RustCrossbreed.Factories;

namespace RustCrossbreed.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Variables
        private string _geneInput = string.Empty;
        private string _geneInputErrorFeedback = string.Empty;
        #endregion

        #region Constructors
        public MainWindowViewModel(RustCrossbreeder crossbreeder, IBreedRepository breedsRepo, IBreedRepository selectedRepo, 
            IBreedRepository outputRepo, IRepository<HistoryModel> history)
        {
            Crossbreeder = crossbreeder ?? throw new ArgumentNullException(nameof(crossbreeder));
            BreedsRepo = breedsRepo ?? throw new ArgumentNullException(nameof(breedsRepo));
            SelectedRepo = selectedRepo ?? throw new ArgumentNullException(nameof(selectedRepo));
            OutputRepo = outputRepo ?? throw new ArgumentNullException(nameof(outputRepo));
            History = history ?? throw new ArgumentNullException(nameof(history));

            BreedsListSelectedItems = new List<Breed>();
            SelectedListSelectedItems = new List<Breed>();
            OutputListSelectedItems = new List<Breed>();
        }
        #endregion

        #region Properties
        public string GeneInput
        {
            get => _geneInput;
            set => SetProperty(ref _geneInput, value);
        }
        public string GeneInputErrorFeedback
        {
            get => _geneInputErrorFeedback;
            set => SetProperty(ref _geneInputErrorFeedback, value);
        }
        public ObservableCollection<Breed> BreedsList
        {
            get => BreedsRepo.GetAll();
        }
        public ObservableCollection<Breed> SelectedList
        {
            get => SelectedRepo.GetAll();
        }
        public ObservableCollection<Breed> OutputList
        {
            get => OutputRepo.GetAll();
        }

        public List<Breed> BreedsListSelectedItems { get; set; }
        public List<Breed> SelectedListSelectedItems { get; set; }
        public List<Breed> OutputListSelectedItems { get; set; }

        private IBreedRepository BreedsRepo { get; }
        private IBreedRepository SelectedRepo { get; }
        private IBreedRepository OutputRepo { get; }
        private IRepository<HistoryModel> History { get; }

        private RustCrossbreeder Crossbreeder { get; }
        #endregion

        #region Methods
        public void SubmitGene()
        {
            if (GeneInput.Length == 6)
            {
                if (BreedFactory.TryParseBreed(GeneInput, out Breed breed))
                {
                    if (BreedsRepo.TryAdd(breed))
                    {
                        ClearGeneInput();
                        History.Add(new HistoryModel(EHistoryAction.Added, breed));
                    }
                    else
                        GeneInputErrorFeedback = "Cannot add duplicate genes.";
                }
                else
                    GeneInputErrorFeedback = "Unable to read genes. Make sure you only enter valid genes.";
            }
            else
                GeneInputErrorFeedback = "Breed must have 6 genes.";
        }

        public void ClearGeneInput()
        {
            GeneInput = string.Empty;
            GeneInputErrorFeedback = string.Empty;
        }
        public void DeleteAllBreeds()
        {
            foreach(Breed breed in BreedsList)
            {
                History.Add(new HistoryModel(EHistoryAction.Removed, breed));
            }

            BreedsList.Clear();
            SelectedList.Clear();
            OutputList.Clear();
        }
        public void ClearSelectedGenes()
        {
            SelectedList.Clear();
            OutputList.Clear();
        }
        public void ClearGenesOutput()
        {
            OutputRepo.Clear();
        }

        public void DeleteSelectedBreeds()
        {
            bool itemRemoved = false;
            foreach (Breed breed in BreedsListSelectedItems.ToArray())
            {
                if (BreedsRepo.Remove(breed))
                {
                    History.Add(new HistoryModel(EHistoryAction.Removed, breed));
                    while (SelectedRepo.Remove(breed)) //see RemoveSelectedBreeds() for more info
                    {
                        itemRemoved |= true;
                    }
                }
            }
            if(itemRemoved)
                ClearGenesOutput();
        }
        public void OpenHistoryWindow()
        {
            // creating a window here violates MVVM, but thats a problem for another day
            var historyWindow = new Views.HistoryWindow(new HistoryViewModel(History, BreedsRepo));
            historyWindow.Show();
        }
        public void OnMoreInfoClick()
        {
            List<Breed> selection;

            //find the currently selected
            //only the most recently selected listview can have a selection, enforced in view, maybe enforce in viewmodel later
            if (BreedsListSelectedItems.Count > 0)
                selection = BreedsListSelectedItems;
            else if (SelectedListSelectedItems.Count > 0)
                selection = SelectedListSelectedItems;
            else if (OutputListSelectedItems.Count > 0)
                selection = OutputListSelectedItems;
            else
                return;

            foreach (Breed breed in selection)
            {
                Breed[] children = BreedsRepo.FindChildren(breed).ToArray();
                int?[] parentGens = FindParentGenerations(breed);
                var vm = new MoreInfoViewModel(breed, children, parentGens);

                //creating a window here violates MVVM, but thats a problem for another day
                var moreInforWindow = new Views.MoreInfoWindow(vm);
                moreInforWindow.Show();
            }
        }
        public void AddSelectedBreeds()
        {
            foreach (Breed breed in BreedsListSelectedItems)
            {
                if (SelectedRepo.TryAdd(breed))
                    ClearGenesOutput();
            }
        }

        public void RemoveSelectedBreeds()
        {
            bool itemRemoved = false;
            foreach (Breed breed in SelectedListSelectedItems.ToArray())
            {
                //this is done multiple times since the selection list can have duplicates
                //maybe improve this later, translation: this is kinda jank but i will forget about it
                while (SelectedRepo.Remove(breed))
                {
                    itemRemoved |= true;
                }
            }
            if(itemRemoved)
                ClearGenesOutput();
        }
        public void CrossbreedSelectedBreeds()
        {
            if (Crossbreeder.IsValidCountForCrossbreed(SelectedList.Count))
            {
                OutputRepo.Clear();
                Crossbreeder.Crossbreed(SelectedList)
                    .ForEach(breed => OutputRepo.TryAdd(breed));
            }
        }

        public void SaveOutputBreed()
        {
            foreach (Breed breed in OutputListSelectedItems.ToArray())
            {
                if (BreedsRepo.TryAdd(breed))
                {
                    History.Add(new HistoryModel(EHistoryAction.Crossbred, breed));
                    OutputRepo.Remove(breed);
                }
            }
        }


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
