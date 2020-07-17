using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RustCrossbreed.BusinessLogic;
using RustCrossbreed.Services;
using RustCrossbreed.Factories;
using System.Windows.Documents;

namespace RustCrossbreed.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Variables
        private string _geneInput = string.Empty;
        private string _geneInputErrorFeedback = string.Empty;
        #endregion

        #region Constructors
        public MainWindowViewModel(RustCrossbreeder crossbreeder, IBreedRepository breedsRepo, IBreedRepository selectedRepo, IBreedRepository outputRepo)
        {
            Crossbreeder = crossbreeder ?? throw new ArgumentNullException(nameof(crossbreeder));
            BreedsRepo = breedsRepo ?? throw new ArgumentNullException(nameof(breedsRepo));
            SelectedRepo = selectedRepo ?? throw new ArgumentNullException(nameof(selectedRepo));
            OutputRepo = outputRepo ?? throw new ArgumentNullException(nameof(outputRepo));
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

        private RustCrossbreeder Crossbreeder { get; }
        #endregion

        #region Methods
        public void SubmitGene()
        {
            if (GeneInput.Length == 6)
            {
                if (BreedFactory.TryParseBreed(GeneInput, out Breed breed))
                {
                    if (BreedsRepo.Contains(breed.Genes))
                    {
                        GeneInputErrorFeedback = "Cannot add duplicate genes.";
                    }
                    else
                    {
                        BreedsList.Add(breed);
                        ClearGeneInput();
                    }
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
        public void ClearGenes()
        {
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

        }
        public void OnMoreInfoClick()
        {

        }
        public void AddSelectedBreeds()
        {
            foreach (Breed breed in BreedsListSelectedItems)
            {
                if (SelectedRepo.Add(breed))
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
                    .ForEach(breed => OutputRepo.Add(breed));
            }
        }

        public void SaveOutputBreed()
        {
            foreach (Breed breed in OutputListSelectedItems.ToArray())
            {
                if (BreedsRepo.Add(breed))
                    OutputRepo.Remove(breed);
            }
        }
        #endregion
    }
}
