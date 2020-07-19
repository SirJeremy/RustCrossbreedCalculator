using System.Collections.Generic;
using System.Collections.ObjectModel;
using RustCrossbreed.BusinessLogic;

namespace RustCrossbreed.Services
{
    public interface IBreedRepository : IRepository<Breed>
    {
        //returns true if successfully added
        public bool TryAdd(Breed breed);

        public Breed Get(IEnumerable<EGene> genes);
        public Breed Get(string genes);
        public int GetIndex(IEnumerable<EGene> genes);
        public int GetIndex(string genes);

        public List<Breed> FindChildren(Breed parentBreed);

        //true if successfully removed
        public bool Remove(IEnumerable<EGene> genes);
        public bool Remove(string genes);

        //the Breed one is for exact match
        public bool Contains(Breed breed, out int breedIndex);
        public bool Contains(IEnumerable<EGene> genes);
        public bool Contains(IEnumerable<EGene> genes, out Breed breed);
        public bool Contains(IEnumerable<EGene> genes, out int breedIndex);
        public bool Contains(IEnumerable<EGene> genes, out Breed breed, out int breedIndex);
        public bool Contains(string genes);
        public bool Contains(string genes, out Breed breed);
        public bool Contains(string genes, out int breedIndex);
        public bool Contains(string genes, out Breed breed, out int breedIndex);
    }
}
