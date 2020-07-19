using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RustCrossbreed.BusinessLogic;

namespace RustCrossbreed.Services
{
    //this probably shouldn't be considered a repository, but ¯\_(ツ)_/¯
    //this also probably shouldn't give access to the backing collection, but ¯\_(ツ)_/¯
    //feels like i did enough to show what I am attempting (it's up to you to decide if it's a good idea or madness)
    //basically just wanted a wrapper for a collection to eforce a no dupes rule and some helpers, but didn't know
    //  how to properly sync the list copy in viewmodel and this (or even if that is the right way)
    //given how this is a small program anyways, and I am coding like this just to practice, it should be fine
    public class BreedRepository : IBreedRepository
    {
        //#region Variables
        //private const int defaultCapacity = 50;
        //#endregion

        #region Properties
        private ObservableCollection<Breed> Breeds { get; }
        private bool AllowDuplicates { get; }
        #endregion

        #region Constructors
        public BreedRepository(bool allowDuplicates = false)
        {
            AllowDuplicates = allowDuplicates;
            Breeds = new ObservableCollection<Breed>();
        }
        public BreedRepository(IEnumerable<Breed> breeds, bool allowDuplicates = false)
        {
            AllowDuplicates = allowDuplicates;
            Breeds = new ObservableCollection<Breed>(breeds);
        }
        #endregion

        #region IBreedRepository
        //does not allow duplicate genes, even if generations and parents are different
        public void Add(Breed breed)
        {
            TryAdd(breed);
        }
        public bool TryAdd(Breed breed)
        {
            if (!AllowDuplicates && Contains(breed.ToString()))
                return false;
            Breeds.Add(breed);
            return true;
        }

        public void Clear()
        {
            Breeds.Clear();
        }

        //using Breed will check for gene and parent equality
        public bool Contains(Breed breed)
        {
            return Breeds.Contains(breed);
        }
        public bool Contains(Breed breed, out int breedIndex)
        {
            for (int i = 0; i < Breeds.Count; i++)
            {
                if (Breeds[i] == breed)
                {
                    breedIndex = i;
                    return true;
                }
            }
            breedIndex = -1;
            return false;
        }

        public bool Contains(IEnumerable<EGene> genes)
        {
            return Contains(genes, out _, out _);
        }
        public bool Contains(IEnumerable<EGene> genes, out Breed breed)
        {
            return Contains(genes, out breed, out _);
        }
        public bool Contains(IEnumerable<EGene> genes, out int breedIndex)
        {
            return Contains(genes, out _, out breedIndex);
        }
        public bool Contains(IEnumerable<EGene> genes, out Breed breed, out int breedIndex)
        {
            for (int i = 0; i < Breeds.Count; i++)
            {
                if (Breeds[i].Genes.SequenceEqual(genes))
                {
                    breed = Breeds[i];
                    breedIndex = i;
                    return true;
                }
            }

            breed = null;
            breedIndex = -1;
            return false;
        }

        public bool Contains(string genes)
        {
            return Contains(genes, out _, out _);
        }
        public bool Contains(string genes, out Breed breed)
        {
            return Contains(genes, out breed, out _);
        }
        public bool Contains(string genes, out int breedIndex)
        {
            return Contains(genes, out _, out breedIndex);
        }
        public bool Contains(string genes, out Breed breed, out int breedIndex)
        {
            for (int i = 0; i < Breeds.Count; i++)
            {
                if (Breeds[i].ToString() == genes)
                {
                    breed = Breeds[i];
                    breedIndex = i;
                    return true;
                }
            }

            breed = null;
            breedIndex = -1;
            return false;
        }

        public int Count()
        {
            return Breeds.Count();
        }

        public Breed Get(int index)
        {
            if (index < 0 || index >= Breeds.Count) throw new ArgumentOutOfRangeException(nameof(index));

            return Breeds[index];
        }
        public Breed Get(IEnumerable<EGene> genes)
        {
            Contains(genes, out Breed output);
            return output;
        }
        public Breed Get(string genes)
        {
            Contains(genes, out Breed output);
            return output;
        }

        public ObservableCollection<Breed> GetAll()
        {
            return Breeds;
        }

        public int GetIndex(Breed breed)
        {
            return Breeds.IndexOf(breed);
        }
        public int GetIndex(IEnumerable<EGene> genes)
        {
            Contains(genes, out int output);
            return output;
        }
        public int GetIndex(string genes)
        {
            Contains(genes, out int output);
            return output;
        }

        public List<Breed> FindChildren(Breed parentBreed)
        {
            var children = new List<Breed>();

            //check each breed
            foreach (Breed breed in Breeds)
            {
                //check parents of breed to find match
                foreach (string parentGene in breed.ParentGenes)
                {
                    if (parentGene == parentBreed.GenesString)
                    {
                        children.Add(breed);
                        break;
                    }
                }
            }

            return children;
        }

        public bool Remove(Breed breed)
        {
            return Breeds.Remove(breed);
        }
        public bool Remove(IEnumerable<EGene> genes)
        {
            if (Contains(genes, out int i))
            {
                Breeds.RemoveAt(i);
                return true;
            }
            return false;
        }
        public bool Remove(string genes)
        {
            if (Contains(genes, out int i))
            {
                Breeds.RemoveAt(i);
                return true;
            }
            return false;
        }
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Breeds.Count) throw new ArgumentOutOfRangeException(nameof(index));
            Breeds.RemoveAt(index);
        }
        #endregion
    }
}
